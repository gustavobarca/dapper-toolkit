using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;

namespace Lithe;

public static class ConnectionExtensions
{
    private static readonly ConcurrentDictionary<Type, string> ColumnListCache = new();
    private static readonly ConcurrentDictionary<Type, string> TableNameCache = new();
    private static readonly ConcurrentDictionary<Type, string> KeyNameCache = new();

    public static Task<T?> GetAsync<T>(this IDbConnection connection, object id)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(id);

        var sql = BuildSelect<T>();

        return connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, new { Id = id }));
    }

    public static string BuildSelect<T>()
    {
        var type = typeof(T);
        var table = TableNameCache.GetOrAdd(type, t => t.Name);
        var key = KeyNameCache.GetOrAdd(type, FindKeyProperty);
        var columns = ColumnListCache.GetOrAdd(type, BuildColumns);

        return $"select {columns} from \"{table}\" where \"{key}\" = @Id";
    }

    private static string BuildColumns(Type type)
    {
        var props = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .Select(p =>
            {
                var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                return string.IsNullOrWhiteSpace(columnAttribute?.Name) ? $"\"{p.Name}\"" : $"{columnAttribute!.Name} as \"{p.Name}\"";
            })
            .ToArray();

        if (props.Length == 0)
        {
            throw new InvalidOperationException($"Type '{type.FullName}' has no readable public properties.");
        }

        return string.Join(", ", props);
    }

    private static string FindKeyProperty(Type type)
    {
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var typeId = type.Name + "Id";

        for (var i = 0; i < props.Length; i++)
        {
            var name = props[i].Name;

            if (string.Equals(name, typeId, StringComparison.OrdinalIgnoreCase))
            {
                return name;
            }
        }

        throw new InvalidOperationException($"Type '{type.FullName}' must have an 'Id' or '{type.Name}Id' property, or pass keyName explicitly.");
    }
}
