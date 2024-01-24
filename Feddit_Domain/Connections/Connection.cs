using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Feddit_Domain.Models;
using System.ComponentModel;
using System.Collections.Specialized;

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
                await command.ExecuteNonQueryAsync();
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
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally { _sqlConnection.Close(); }

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
        public async Task<List<SubFedditPosts>> GetAllSubfedditPosts(Guid SubFedditId)
        {
            
            List<SubFedditPosts> temp = new List<SubFedditPosts>();
            SqlCommand command = await MySqlCommand("SPGetAllSubFedditPosts");
            command.Parameters.AddWithValue("@SubFedditId", SubFedditId);
            try
            {
                _sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SubFedditPosts temp2 = new();
                    temp2.PostId = reader.GetGuid("PostId"); 
                    temp2.UserId = reader.GetGuid("UserId");
                    temp2.SubFedditId = reader.GetGuid("SubFedditId");
                    temp2.PostPic = reader.IsDBNull(reader.GetOrdinal("PicturePath")) ? null : reader.GetString("PicturePath");
                    temp2.PostTitle = reader.GetString("Title");
                    temp2.PostContent = reader.GetString("Content");
                    temp2.DateCreated = reader.GetDateTime("CreatedAt");
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
        public async Task<SubFeddits> GetSubFedditByName(string subfedditname)
        {
            SqlCommand command = await MySqlCommand("SPGetSubfedditByName");
            command.Parameters.AddWithValue("@SubFedditName", subfedditname);
            try
            {
                _sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SubFeddits subfeddit = new SubFeddits(
                        reader.GetGuid("SubFedditId"),
                        reader.GetString("SubFedditName"),
                        reader.GetDateTime("SubFedditCreatedAt")
                        );
                    return subfeddit;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally { _sqlConnection.Close(); }
            return await Task.FromResult<SubFeddits>(null);
        }
        public async Task<SubFedditPosts> GetPostById(Guid PostId)
        {
            SqlCommand command = await MySqlCommand("SPGetPostById");
            command.Parameters.AddWithValue("@PostId", PostId);
            try
            {
                _sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    SubFedditPosts temp = new SubFedditPosts(
                        reader.GetGuid("PostId"),
                        reader.GetGuid("UserId"),
                        reader.GetGuid("SubFedditId"),
                        reader.IsDBNull(reader.GetOrdinal("PicturePath")) ? null : reader.GetString("PicturePath"),
                        reader.GetString("Title"),
                        reader.GetString("Content"),
                        reader.GetDateTime("CreatedAt"));
                    return temp;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally { _sqlConnection.Close(); }
            return await Task.FromResult<SubFedditPosts>(null);
        }
        public async Task<Comments> CommentOnPost(Guid postid, Guid userid, string title, string content, DateTime createdat)
        {
            SqlCommand command = await MySqlCommand("SpAddComment");
            command.Parameters.AddWithValue("@PostID", postid);
            command.Parameters.AddWithValue("@UserID", userid);
            command.Parameters.AddWithValue("@CommentTitle", title);
            command.Parameters.AddWithValue("@Content", content);
            command.Parameters.AddWithValue("@CreatedAt", createdat);
            try
            {
                _sqlConnection.Open();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally {_sqlConnection.Close(); }
            return new();
        }
        public async Task<SubFedditPosts> AddPostToSubFeddit(Guid userId, Guid subFedditId, string title, string content )
        {
            SqlCommand command = await MySqlCommand("SPAddSubFedditPost");
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@SubFedditId", subFedditId);
            command.Parameters.AddWithValue("@Title",title);
            command.Parameters.AddWithValue("@Content", content );
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
            finally { _sqlConnection.Close(); }
            return new();
        }
        public async Task<List<Comments>> GetAllCommentsAsync(Guid postid)
        {
            List<Comments> temp = new List<Comments>();
            SqlCommand command = await MySqlCommand("SPGetAllCommentsOnPost");
            command.Parameters.AddWithValue("@PostId", postid);
            try
            {
                _sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Comments temp2 = new();
                    temp2.CommentId = reader.GetGuid("CommentID");
                    temp2.PostId = reader.GetGuid("PostID");
                    temp2.UserId = reader.GetGuid("UserID");
                    temp2.CommentTitle = reader.GetString("CommentTitle");
                    temp2.CommentContent = reader.GetString("Content");
                    temp2.CurrentTime = reader.GetDateTime("CreatedAt");
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
        public async Task<List<Comments>> GetAllCommentsFromSpecificUserAsync(Guid Userid)
        {
            List<Comments> temp = new List<Comments>();
            SqlCommand command = await MySqlCommand("SPGetAllCommentsFromUser");
            command.Parameters.AddWithValue("@userid", Userid);
            try
            {
                _sqlConnection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Comments temp2 = new();
                    temp2.CommentId = reader.GetGuid("CommentID");
                    temp2.PostId = reader.GetGuid("PostID");
                    temp2.UserId = reader.GetGuid("UserID");
                    temp2.CommentTitle = reader.GetString("CommentTitle");
                    temp2.CommentContent = reader.GetString("Content");
                    temp2.CurrentTime = reader.GetDateTime("CreatedAt");
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
        public async Task<List<SubFedditPosts>> GetAllPostsFromSpecificUserAsync(Guid Userid)
        {
            List<SubFedditPosts> temp = new List<SubFedditPosts>();
            SqlCommand command = await MySqlCommand("SPGetAllPostsFromUser");
            command.Parameters.AddWithValue("@userid", Userid);
            try
            {
                _sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SubFedditPosts temp2 = new();
                    temp2.PostId = reader.GetGuid("PostID");
                    temp2.UserId = reader.GetGuid("UserID");
                    temp2.PostTitle = reader.GetString("Title");
                    temp2.PostContent = reader.GetString("Content");
                    temp2.DateCreated = reader.GetDateTime("CreatedAt");
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
        public async Task<SubFedditPosts> UpdatePost(SubFedditPosts post)
        {
            SqlCommand command = await MySqlCommand("SPUpdatePost");
            command.Parameters.AddWithValue("@PostId", post.PostId);
            command.Parameters.AddWithValue("@Title", post.PostTitle);
            command.Parameters.AddWithValue("@Content", post.PostContent);
            try
            {
                _sqlConnection.Open();
                command.ExecuteNonQuery();
            }
            finally { _sqlConnection.Close(); }
            return null;
        }
    }
}
