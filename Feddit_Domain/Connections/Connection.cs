using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Feddit_Domain.Models;
using System.ComponentModel;

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
        public async Task<Users> GetUserByMail(string mail)
        {
            SqlCommand command = await MySqlCommand("spGetUserInfoByUserEmail");
            command.Parameters.AddWithValue("@Mail", mail);
            try
            {
                _sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Users user = new Users(
                            reader.GetGuid("UserID"),
                            reader.GetString("Mail"),
                            reader.GetString("Name"),
                            reader.GetString("Password"),
                            reader.GetBoolean("IsDeleted"),
                            reader.GetBoolean("SuperAdmin"));
                    return await Task.FromResult(user);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally { _sqlConnection.Close(); }
            return await Task.FromResult<Users>(null);
        }
        public async Task<Users>UpdateUser(Users user)
        {
            SqlCommand command = await MySqlCommand("spUpdateUser");
            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@Mail", user.Email);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);
            command.Parameters.AddWithValue("@SuperAdmin", user.Admin);
            try
            {
                _sqlConnection.Open();
                command.ExecuteNonQuery();
            }
            finally { _sqlConnection.Close(); }
            return null;
        }
        public async Task DeleteUserById(Guid id)
        {
            SqlCommand command = await MySqlCommand("spDeleteUser");
            command.Parameters.AddWithValue("@userid", id);
            try
            {
                _sqlConnection.Open();
                command.ExecuteNonQuery();
            }
            finally { _sqlConnection.Close(); }
        }
        public async Task<Users> LoginUsers(string email, string password)
        {
            SqlCommand command = await MySqlCommand("SPLoginUser");
            command.Parameters.AddWithValue("@Mail", email);
            command.Parameters.AddWithValue("@Password", password);
            try
            {
                _sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (email == reader.GetString("Mail") && password == reader.GetString("Password"))
                    {
                        Users userinfo = new Users(reader.GetGuid("UserID"),
                            reader.GetString("Mail"),
                            reader.GetString("Name"),
                            reader.GetString("Password"),
                            reader.GetBoolean("IsDeleted"),
                            reader.GetBoolean("SuperAdmin"));
                        return userinfo;
                    }
                    if (email != reader.GetString("Mail") || password != reader.GetString("Password"))
                    {
                        return new();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally { _sqlConnection.Close(); }
            return new();
        }
        public async Task CreateSubfeddit(string subfedditname, DateTime currenttime)
        {
            SqlCommand command = await MySqlCommand("SPCreateSubfeddit");
            command.Parameters.AddWithValue("@SubfedditName", subfedditname);
            command.Parameters.AddWithValue("@SubfedditCreatedAt", currenttime);
            try
            {
                _sqlConnection.Open();
                command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<List<SubFeddits>> GetAllSubFeddits()
        {
            List<SubFeddits> temp = new List<SubFeddits>();
            SqlCommand command = await MySqlCommand("SPGetAllSubFeddits");
            try
            {
                _sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) 
                {
                    SubFeddits temp2 = new();
                    temp2.SubFedditId = reader.GetGuid("SubFedditId");
                    temp2.SubFedditName = reader.GetString("SubFedditName");
                    temp2.TimeCreated = reader.GetDateTime("SubFedditCreatedAt");
                    temp.Add(temp2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally { _sqlConnection.Close(); }
            return temp;
        }

    }
}
