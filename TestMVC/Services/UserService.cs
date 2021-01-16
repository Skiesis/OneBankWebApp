using System;
using System.Collections.Generic;
using System.Linq;
using TestMVC.Models;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using Json.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.HttpOverrides;

namespace TestMVC.Services
{
    public class UserService
    {
        private SqlConnection _connection;

        public UserService(IDatabaseSettings settings)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = settings.ConnectionString;

            _connection = new SqlConnection(builder.ConnectionString);
        }

        public User create(User data)
        {
            //Get hashed password
            string password = data.password;
            data.password = hashPassword(password);
            data.status = "1";

            //Open connection
            _connection.Open();

            //Define command
            string sql = $"insert into users(first_name, last_name, password, image_src, email, status, telephone) values('{data.first_name}', '{data.last_name}', '{data.password}', '{data.image_src}', '{data.email}', '{data.status}', '{data.telephone}')";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return created user
            return data;
        }

        public User getBy(string field, string value)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"select * from users where {field} = '{value}'";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Get data into user object
            User user = new User();
            using (var reader = sqlQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.id = reader.GetValue(0).ToString();
                    user.first_name = reader.GetValue(1).ToString(); 
                    user.last_name = reader.GetValue(2).ToString(); 
                    user.password = reader.GetValue(3).ToString(); 
                    user.image_src = reader.GetValue(4).ToString();
                    user.email = reader.GetValue(5).ToString();
                    user.user_core_id = reader.GetValue(6).ToString(); 
                    user.status = reader.GetValue(7).ToString(); 
                    user.telephone = reader.GetValue(8).ToString();
                    user.tax_id = reader.GetValue(9).ToString();
                }
            }

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return user
            return user;
        }

        public bool checkPasswordMatch(string password, string passwordHashed)
        {

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(passwordHashed);

            // Get salt
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Get result
            for (var i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public void openSession(User user, string ip)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"insert into session_w(ip_address, date_time, user_id) values('{ip}', '{DateTime.Now.ToString()}', '{user.id}')";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();
        }

        public void closeSession(string ip)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"update session_w set status = '0' where ip_address = '{ip}'";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();
        }

        private string hashPassword(string password)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            var hash = pbkdf2.GetBytes(20);

            // Combine salt and hash
            var hashBytes = new byte[16 + 20];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Return hashed password
            return base64Hash;
        }
    }
}
