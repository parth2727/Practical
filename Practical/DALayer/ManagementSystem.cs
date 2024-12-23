using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer
{
    public class ManagementSystem
    {
        private readonly string _connectionString;
        public ManagementSystem(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int ExecuteNonQuery(string query, NpgsqlParameter[] parameters, bool IsStoredProcedure)
        {
            int rowAffected = 0;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (IsStoredProcedure)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    rowAffected = command.ExecuteNonQuery();
                }
            }
            return rowAffected;
        }
        public DataTable ExecuteQuery(string query, params NpgsqlParameter[] parameters)
        {
            var dataTable = new DataTable();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public DataTable ExecuteReader(string query)
        {
            var dataTable = new DataTable();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public DataTable ExecuteReader(string query, NpgsqlParameter[] parameters, bool IsStroredProcedure)
        {
            var dataTable = new DataTable();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (IsStroredProcedure)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    try
                    {
                        using (var adapter = new NpgsqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exception here (e.g., logging)
                        Console.WriteLine($"Error executing query: {ex.Message}");
                    }
                }
            }
            return dataTable;
        }
    }
}