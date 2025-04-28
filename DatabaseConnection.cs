using Npgsql;
using System;

namespace PracticaBd
{
    public class DatabaseConnection
    {
        private string connectionString = "Server=localhost;Port=5432;Database=PractitcaBd;User Id=postgres;Password=admin";
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
