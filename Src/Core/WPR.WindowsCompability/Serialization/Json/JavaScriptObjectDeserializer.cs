using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace WPR.WindowsCompability.Serialization.Json;

public sealed class JavaScriptObjectDeserializer
{
    private readonly string json;

    public JavaScriptObjectDeserializer(string json, bool throwOnError)
    {
        ArgumentNullException.ThrowIfNull(json);
        this.json = json;
    }

    public object? BasicDeserialize()
    {
        using JsonDocument document = JsonDocument.Parse(NormalizeLegacyNumbers(json),
            new JsonDocumentOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            });
        return Convert(document.RootElement);
    }

    private static string NormalizeLegacyNumbers(string value)
    {
        StringBuilder? normalized = null;
        bool inString = false;
        bool escaped = false;

        for (int index = 0; index < value.Length; index++)
        {
            char current = value[index];
            if (inString)
            {
                if (escaped)
                {
                    escaped = false;
                }
                else if (current == '\\')
                {
                    escaped = true;
                }
                else if (current == '"')
                {
                    inString = false;
                }
                continue;
            }

            if (current == '"')
            {
                inString = true;
                continue;
            }

            if (current != '.' || index == 0 || !char.IsDigit(value[index - 1]))
            {
                continue;
            }

            int next = index + 1;
            while (next < value.Length && char.IsWhiteSpace(value[next]))
            {
                next++;
            }
            if (next < value.Length && value[next] is not (',' or '}' or ']'))
            {
                continue;
            }

            normalized ??= new StringBuilder(value);
            normalized.Insert(index + 1, '0');
            value = normalized.ToString();
            index++;
        }

        return normalized?.ToString() ?? value;
    }

    private static object? Convert(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var dictionary = new Dictionary<string, object?>();
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    dictionary.Add(property.Name, Convert(property.Value));
                }
                return dictionary;

            case JsonValueKind.Array:
                var list = new ArrayList();
                foreach (JsonElement item in element.EnumerateArray())
                {
                    list.Add(Convert(item));
                }
                return list;

            case JsonValueKind.String:
                return element.GetString();

            case JsonValueKind.Number:
                if (element.TryGetInt32(out int int32))
                {
                    return int32;
                }
                if (element.TryGetInt64(out long int64))
                {
                    return int64;
                }
                if (element.TryGetDecimal(out decimal decimalValue))
                {
                    return decimalValue;
                }
                return element.GetDouble();

            case JsonValueKind.True:
                return true;

            case JsonValueKind.False:
                return false;

            case JsonValueKind.Null:
                return null;

            default:
                throw new FormatException($"Unsupported JSON token '{element.ValueKind}'.");
        }
    }
}
