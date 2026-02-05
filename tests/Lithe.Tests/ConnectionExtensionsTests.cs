using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lithe.Tests;

public class ConnectionExtensionsTest
{
    class Teste
    {
        public Guid TesteId { get; set; }

        [Key]
        public Guid PrimaryKey { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Transform("ST_TRANSFORM(\"Coordinates\", 4326)")]
        public string Coordinates { get; set; }
    }

    [Fact]
    public void BuildSelect()
    {
        var sql = ConnectionExtensions.BuildSelect<Teste>();
    }

    [Fact]
    public void BuildInsert()
    {
        var sql = ConnectionExtensions.BuildInsert<Teste>();
    }
}
