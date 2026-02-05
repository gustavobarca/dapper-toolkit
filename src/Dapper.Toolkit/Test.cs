using System.Data;
using Dapper.Toolkit;

public class Teste
{
    public Guid Guid { get; set; }
}

public class Test
{
    public async Task Main(IDbConnection conn)
    {
        var result = await conn.GetAsync<Teste>(Guid.NewGuid());
    }
}