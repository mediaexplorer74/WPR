using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPR;

internal sealed class GameStateTrace : IDisposable
{
    internal const string PathEnvironmentVariable = "WPR_GAME_TRACE_PATH";

    private readonly object _target;
    private readonly string _path;
    private readonly TimeSpan _interval;
    private readonly DateTime _started = DateTime.UtcNow;
    private readonly CancellationTokenSource _cancellation = new();
    private readonly Task _worker;
    private readonly object _fileLock = new();
    private readonly Dictionary<string, ExceptionObservation> _firstChanceExceptions = new(StringComparer.Ordinal);
    private bool _disposed;

    private GameStateTrace(object target, string path, TimeSpan interval)
    {
        _target = target;
        _path = Path.GetFullPath(path);
        _interval = interval;
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path,
            $"WPR game state trace started {DateTime.UtcNow:o}{Environment.NewLine}" +
            $"Target: {target.GetType().AssemblyQualifiedName}{Environment.NewLine}");
        AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;
        WriteSnapshot("start");
        _worker = Task.Run(TraceLoop);
    }

    public static GameStateTrace? TryStart(Game game)
    {
        string? path = Environment.GetEnvironmentVariable(PathEnvironmentVariable);
        return string.IsNullOrWhiteSpace(path)
            ? null
            : Start(game, path, TimeSpan.FromSeconds(1));
    }

    internal static GameStateTrace Start(object target, string path, TimeSpan interval)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (interval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(interval));
        }
        return new GameStateTrace(target, path, interval);
    }

    private async Task TraceLoop()
    {
        try
        {
            while (true)
            {
                await Task.Delay(_interval, _cancellation.Token);
                WriteSnapshot("interval");
            }
        }
        catch (OperationCanceledException) when (_cancellation.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            Append($"TRACE ERROR: {exception}{Environment.NewLine}");
        }
    }

    private void WriteSnapshot(string reason)
    {
        var text = new StringBuilder();
        text.AppendLine();
        text.AppendLine($"[{DateTime.UtcNow:o}] {reason} elapsed={(DateTime.UtcNow - _started).TotalMilliseconds:F0}ms thread={Environment.CurrentManagedThreadId}");
        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);
        AppendObject(text, _target, "game", depth: 0, visited);
        Append(text.ToString());
    }

    private static void AppendObject(
        StringBuilder text, object target, string prefix, int depth, HashSet<object> visited)
    {
        if (!visited.Add(target))
        {
            text.AppendLine($"{prefix}=<cycle:{target.GetType().FullName}>");
            return;
        }

        Type type = target.GetType();
        text.AppendLine($"{prefix}.type={type.FullName}");
        foreach (FieldInfo field in type.GetFields(
                     BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            string fieldPath = $"{prefix}.{field.Name}";
            object? value;
            try
            {
                value = field.GetValue(target);
            }
            catch (Exception exception)
            {
                text.AppendLine($"{fieldPath}=<read-error:{exception.GetType().Name}>");
                continue;
            }

            text.AppendLine($"{fieldPath}={Describe(value)}");
            if (depth < 2 && value != null && ShouldExpand(field.Name, value))
            {
                AppendObject(text, value, fieldPath, depth + 1, visited);
            }
        }
    }

    private static bool ShouldExpand(string fieldName, object value)
    {
        if (value is string or Thread || value.GetType().IsPrimitive || value.GetType().IsEnum)
        {
            return false;
        }
        return fieldName.Contains("state", StringComparison.OrdinalIgnoreCase) ||
               fieldName.Contains("screen", StringComparison.OrdinalIgnoreCase) ||
               fieldName.Contains("manager", StringComparison.OrdinalIgnoreCase) ||
               fieldName.Contains("loading", StringComparison.OrdinalIgnoreCase);
    }

    private static string Describe(object? value)
    {
        if (value == null)
        {
            return "<null>";
        }
        if (value is Thread thread)
        {
            return $"Thread(Id={thread.ManagedThreadId}, IsAlive={thread.IsAlive}, State={thread.ThreadState})";
        }
        if (value is string text)
        {
            return $"\"{text}\"";
        }
        Type type = value.GetType();
        if (type.IsPrimitive || type.IsEnum || value is decimal or DateTime or DateTimeOffset or TimeSpan or Guid)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "<null>";
        }
        return $"<{type.FullName}>";
    }

    private void Append(string text)
    {
        lock (_fileLock)
        {
            File.AppendAllText(_path, text);
        }
    }

    private void OnFirstChanceException(object? sender, FirstChanceExceptionEventArgs args)
    {
        try
        {
            Exception exception = args.Exception;
            string signature = $"{exception.GetType().FullName}: {exception.Message}";
            bool firstOccurrence;
            lock (_firstChanceExceptions)
            {
                if (_firstChanceExceptions.TryGetValue(signature, out ExceptionObservation? observation))
                {
                    observation.Count++;
                    observation.LastSeenUtc = DateTime.UtcNow;
                    observation.LastThreadId = Environment.CurrentManagedThreadId;
                    return;
                }
                if (_firstChanceExceptions.Count >= 100)
                {
                    return;
                }
                _firstChanceExceptions.Add(signature, new ExceptionObservation
                {
                    Count = 1,
                    FirstSeenUtc = DateTime.UtcNow,
                    LastSeenUtc = DateTime.UtcNow,
                    FirstThreadId = Environment.CurrentManagedThreadId,
                    LastThreadId = Environment.CurrentManagedThreadId
                });
                firstOccurrence = true;
            }
            if (!firstOccurrence)
            {
                return;
            }
            Append($"{Environment.NewLine}FIRST CHANCE {DateTime.UtcNow:o} thread={Environment.CurrentManagedThreadId}{Environment.NewLine}" +
                   $"{exception}{Environment.NewLine}");
        }
        catch
        {
            // Diagnostics must never change game behavior.
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        AppDomain.CurrentDomain.FirstChanceException -= OnFirstChanceException;
        _cancellation.Cancel();
        try
        {
            _worker.GetAwaiter().GetResult();
        }
        finally
        {
            WriteSnapshot("stop");
            WriteExceptionSummary();
            _cancellation.Dispose();
        }
    }

    private void WriteExceptionSummary()
    {
        var text = new StringBuilder();
        text.AppendLine();
        text.AppendLine("FIRST CHANCE SUMMARY");
        lock (_firstChanceExceptions)
        {
            foreach ((string signature, ExceptionObservation observation) in _firstChanceExceptions)
            {
                text.AppendLine(
                    $"count={observation.Count} first={observation.FirstSeenUtc:o} last={observation.LastSeenUtc:o} " +
                    $"firstThread={observation.FirstThreadId} lastThread={observation.LastThreadId} signature={signature}");
            }
        }
        Append(text.ToString());
    }

    private sealed class ExceptionObservation
    {
        public int Count { get; set; }
        public DateTime FirstSeenUtc { get; set; }
        public DateTime LastSeenUtc { get; set; }
        public int FirstThreadId { get; set; }
        public int LastThreadId { get; set; }
    }
}
