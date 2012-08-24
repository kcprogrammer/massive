using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostGreSQLUnitTests.Fixtures
{
    public class SchemaFixture : IDisposable
    {
        public SchemaFixture()
        {
            var connString = ConfigurationManager.ConnectionStrings["Massive.PostGreSQLUnitTests"].ConnectionString;

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                CreateItemsTable(conn);
            }

            DropSchema = true;
        }

        private bool DropSchema = false;
        public void CleanUp()
        {
            if (DropSchema)
            {
                var connString = ConfigurationManager.ConnectionStrings["Massive.PostGreSQLUnitTests"].ConnectionString;

                 using (var conn = new NpgsqlConnection(connString))
                 {
                     conn.Open();

                     DropItemsTable(conn);
                 }

                DropSchema = false;
            }
        }

        public void Dispose()
        {
            this.CleanUp();
        }


        private void CreateItemsTable(NpgsqlConnection conn)
        {
            string sql = "CREATE TABLE items " +
                        "( " +
                        "  id bigserial, " +
                        "  name varchar(100), " +
                        "  CONSTRAINT pk_items PRIMARY KEY (id) " +
                        ") " +
                        "WITH ( " +
                        "OIDS = FALSE " +
                        "); " +
                        "ALTER TABLE items OWNER TO postgres;";
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    var msg = exc.ToString();
                    throw;
                }

            }
        }
        private void DropItemsTable(NpgsqlConnection conn)
        {
            string sql = "DROP TABLE items;";
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
