# Dapper.Toolkit

Lightweight helpers for Dapper, currently focused on a `GetAsync<T>` extension for `IDbConnection`.

## Install (dotnet CLI)

This repository is a library project. The most reliable way to use it from another project is a project reference.

1. Clone this repo (or copy it into your solution).
2. From your consuming project folder, add a reference:

```bash
dotnet add reference ../path/to/dapper-toolkit/src/Dapper.Toolkit/Dapper.Toolkit.csproj
```

3. Build your solution:

```bash
dotnet build
```

If you later publish this as a NuGet package, you would install it like:

```bash
dotnet add package Dapper.Toolkit
```

## Usage: `GetAsync<T>`

`GetAsync<T>` builds a `SELECT` statement and queries by primary key. It follows these conventions:

- Table name: `typeof(T).Name`
- Key column: `{TypeName}Id` (case-insensitive)
- Columns: all public readable instance properties

Example:

```csharp
using System.Data;
using Dapper;
using Dapper.Toolkit;

public sealed class User
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static async Task<User?> LoadUserAsync(IDbConnection connection, int id)
{
    // Make sure the connection is open if your provider requires it.
    return await connection.GetAsync<User>(id);
}
```

If your key property does not follow the `{TypeName}Id` convention, `GetAsync<T>` will throw an `InvalidOperationException`.
