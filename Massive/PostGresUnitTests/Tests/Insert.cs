using Npgsql;
using PostGreSQLUnitTests.Fixtures;
using PostGreSQLUnitTests.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PostGreSQLUnitTests.Tests
{
    public class Insert : IUseFixture<SchemaFixture>, IDisposable
    {
        private SchemaFixture _schemaFixture = null;
        public void SetFixture(SchemaFixture fixture)
        {
            _schemaFixture = fixture;
        }
        public void Dispose()
        {
            _schemaFixture.CleanUp();
        }

        [Fact]
        public void CanInsertAnItem()
        {
            dynamic model = new ExpandoObject();
            model.name = "Widget";

            dynamic tbl = new Items();
            dynamic result = tbl.Insert(model);

            var connString = ConfigurationManager.ConnectionStrings["Massive.PostGreSQLUnitTests"].ConnectionString;

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "SELECT * FROM items WHERE id = @Id";
                    cmd.Parameters.Add("@Id", NpgsqlTypes.NpgsqlDbType.Bigint);
                    cmd.Parameters[0].Value = result.ID;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Assert.Equal(model.name, reader.GetString(reader.GetOrdinal("name")));
                        }
                        else
                        {
                            throw new Exception("Didn't find item with returned id");
                        }
                    }
                }
            }
        }
    }
}
