using System.ComponentModel.DataAnnotations.Schema;

namespace Lithe.Tests;

public class ConnectionExtensionsTest
{
    class Teste
    {
        public Guid TesteId { get; set; }
        [Column("ST_AsEWKT(\"Coordinates\")")]
        public string Name { get; set; }
    }

    [Fact]
    public void Test1()
    {
        var sql = ConnectionExtensions.BuildSelect<Teste>();
    }
}
