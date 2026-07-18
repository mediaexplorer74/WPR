using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using WPR.Common;

namespace WPR.WindowsCompability;

internal static class IsolatedStorageSettingsSerializer
{
    private static readonly DataContractResolver TypeResolver = new SettingsTypeResolver();

    public static byte[] Serialize(Dictionary<string, object> settings)
    {
        using var stream = new MemoryStream();
        CreateSerializer().WriteObject(stream, settings);
        return stream.ToArray();
    }

    public static Dictionary<string, object> Deserialize(Stream stream)
    {
        return (Dictionary<string, object>?)CreateSerializer().ReadObject(stream) ?? new();
    }

    public static string? GetApplicationSettingsPath()
    {
        string? directory = GetApplicationStoragePath();
        return directory == null ? null : Path.Combine(directory, "__LocalSettings");
    }

    public static string? GetApplicationStoragePath()
    {
        string? productId = Application.Current.ProductId;
        if (Configuration.Current == null || !Guid.TryParse(productId, out Guid parsedProductId))
        {
            return null;
        }

        string directory = Configuration.Current.DataPath(Path.Combine(
            "AppData", parsedProductId.ToString("D"), "IsolatedStorage"));
        Directory.CreateDirectory(directory);
        Directory.CreateDirectory(Path.Combine(directory, "Shared", "Media"));
        Directory.CreateDirectory(Path.Combine(directory, "Shared", "ShellContent"));
        Directory.CreateDirectory(Path.Combine(directory, "Shared", "Transfers"));
        return directory;
    }

    public static void Save(string path, Dictionary<string, object> settings)
    {
        byte[] serialized = Serialize(settings);
        string temporaryPath = path + ".tmp";
        File.WriteAllBytes(temporaryPath, serialized);
        File.Move(temporaryPath, path, overwrite: true);
    }

    private static DataContractSerializer CreateSerializer()
    {
        return new DataContractSerializer(typeof(Dictionary<string, object>),
            new DataContractSerializerSettings { DataContractResolver = TypeResolver });
    }

    private sealed class SettingsTypeResolver : DataContractResolver
    {
        private const string TypeNamespace = "urn:wpr:isolated-storage-settings:type";

        public override bool TryResolveType(Type type, Type? declaredType,
            DataContractResolver knownTypeResolver, out XmlDictionaryString? typeName,
            out XmlDictionaryString? typeNamespace)
        {
            if (knownTypeResolver.TryResolveType(type, declaredType, null!, out typeName,
                out typeNamespace))
            {
                return true;
            }

            string assemblyQualifiedName = type.AssemblyQualifiedName
                ?? throw new SerializationException($"Settings type '{type}' has no assembly-qualified name.");
            string encodedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(assemblyQualifiedName))
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
            var dictionary = new XmlDictionary();
            typeName = dictionary.Add("t_" + encodedName);
            typeNamespace = dictionary.Add(TypeNamespace);
            return true;
        }

        public override Type? ResolveName(string typeName, string? typeNamespace, Type? declaredType,
            DataContractResolver knownTypeResolver)
        {
            if (typeNamespace != TypeNamespace || !typeName.StartsWith("t_", StringComparison.Ordinal))
            {
                return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null!);
            }

            string encodedName = typeName.Substring(2).Replace('-', '+').Replace('_', '/');
            encodedName = encodedName.PadRight(encodedName.Length + ((4 - encodedName.Length % 4) % 4), '=');
            try
            {
                string assemblyQualifiedName = Encoding.UTF8.GetString(Convert.FromBase64String(encodedName));
                return Type.GetType(assemblyQualifiedName, throwOnError: false)
                    ?? throw new SerializationException(
                        $"Isolated setting type '{assemblyQualifiedName}' is unavailable.");
            }
            catch (FormatException exception)
            {
                throw new SerializationException("The isolated setting type name is invalid.", exception);
            }
        }
    }
}
