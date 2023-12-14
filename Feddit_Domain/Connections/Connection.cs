using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Feddit_Domain.Models;

namespace Feddit_Domain.Connections
{
    public class Connection : IConnection
    {
        private readonly string _connectionString;
        private readonly SqlConnection _sqlConnection;
        public Connection(string connection)
        {
            _connectionString = connection;
            _sqlConnection = new SqlConnection(_connectionString);
        }
        private Task<SqlCommand> MySqlCommand(string storedprocedure)
        {
            SqlCommand myCommand = new(storedprocedure)
            {
                CommandType = CommandType.StoredProcedure,
                Connection = _sqlConnection
            };
            return Task.FromResult(myCommand);
        }
        public async Task CreateUser(string mail, string password, string name)
        {
            SqlCommand command = await MySqlCommand("spCreateUser");
            command.Parameters.AddWithValue("@Mail", mail.ToLower());
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@Name", name);
            try
            {
                _sqlConnection.Open();
                command.ExecuteNonQuery();
            }
            finally { _sqlConnection.Close(); }
        }
        public async Task GetUserByMail(string mail)
        {
            SqlCommand command = await MySqlCommand("spGetUserInfoByUserEmail");
            command.Parameters.AddWithValue("@Mail", mail);
        }
        public async Task UpdateUser(Users user)
        {
            SqlCommand command = await MySqlCommand("spUpdateUser");
            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);
            command.Parameters.AddWithValue("@SuperAdmin", user.admin);
            try
            {
                _sqlConnection.Open();
                command.ExecuteNonQuery();
            }
            finally { _sqlConnection.Close(); }
        }
        public async Task DeleteUserById(int id)
        {
            SqlCommand command = await MySqlCommand("spDeleteUserById");
            command.Parameters.AddWithValue("@userid", id);
            try
            {
                _sqlConnection.Open();
                command.ExecuteNonQuery();
            }
            finally { _sqlConnection.Close(); }
        }


    }
}
