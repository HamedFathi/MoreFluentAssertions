using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreFluentAssertions
{
    public static class JsonExtensions
    {
        public static IEnumerable<string> GetKeys(this JsonElement doc)
        {
            var queu = new Queue<(string ParentPath, JsonElement element)>();
            queu.Enqueue(("", doc));
            while (queu.Any())
            {
                var (parentPath, element) = queu.Dequeue();
                switch (element.ValueKind)
                {
                    case JsonValueKind.Object:
                        parentPath = parentPath == ""
                            ? "$."
                            : parentPath + ".";
                        foreach (var nextEl in element.EnumerateObject())
                        {
                            queu.Enqueue(($"{parentPath}{nextEl.Name}", nextEl.Value));
                        }
                        yield return parentPath.Trim('.') + "-object";
                        break;
                    case JsonValueKind.Array:
                        foreach (var (nextEl, i) in element.EnumerateArray().Select((jsonElement, i) => (jsonElement, i)))
                        {
                            if (string.IsNullOrEmpty(parentPath))
                                parentPath = "$.";
                            queu.Enqueue(($"{parentPath}[{i}]", nextEl));
                        }
                        yield return parentPath.Trim('.') + "-array"; ;
                        break;
                    case JsonValueKind.String:
                        var isDate = DateTime.TryParse(element.ToString(), out _);
                        var type = isDate ? "-date" : "-string";
                        yield return parentPath.Trim('.') + type;
                        break;
                    case JsonValueKind.Number:
                        yield return parentPath.Trim('.') + "-number"; ;
                        break;
                    case JsonValueKind.Undefined:
                        yield return parentPath.Trim('.') + "-undefined";
                        break;
                    case JsonValueKind.Null:
                        yield return parentPath.Trim('.') + "-null";
                        break;
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        yield return parentPath.Trim('.') + "-boolean"; ;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
