using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace P3TournamentPlanner.Server {
    public class DatabaseQuerys {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GeneralDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public void RunQuery(string query) {
            using(SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public T SelectQuery<T>(string query, string col) {
            using(SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try {
                    while(reader.Read()) {
                        T eksempel = (T)reader[col];
                        return eksempel;
                    }
                } finally {
                    connection.Close();
                }
            }
            return default(T);
        }

        public DataTable PullTable(SqlCommand command) {

            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    command.Connection = connection;
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(table);
                    connection.Close();
                    da.Dispose();
                    return table;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public void InsertToTable(SqlCommand command) {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } 
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DeleteRow(SqlCommand command)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}