using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BO;
internal static class Tools
{
    public static string ToStringProperty<T>(T t)
    {
        if (t == null)
        {
            return string.Empty;
        }

        Type type = t.GetType();
        PropertyInfo[] properties = type.GetProperties();

        string result = "{ " + string.Join(", ", properties
            .Select(property => $"{property.Name}: {GetValueAsString(property.GetValue(t)!)}")) + " }";

        return result;
    }

    private static string GetValueAsString(object value)
    {
        if (value is IEnumerable enumerableValue && !(value is string))
        {
            return $"[{string.Join(", ", enumerableValue.Cast<object>())}]";
        }
        else
        {
            return value?.ToString() ?? "null";
        }
    }

    private static readonly Comparer s_comparer = new();

    internal class Comparer : IEqualityComparer<IEnumerable<int>>
    {
        public bool Equals(IEnumerable<int>? d1, IEnumerable<int>? d2) => d1!.SequenceEqual(d2!);
        public int GetHashCode([DisallowNull] IEnumerable<int> obj) =>
            obj.OrderBy(x => x)
              .Aggregate(17, (current, val) => current * 23 + val.GetHashCode());
    }


}

