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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace TestMVC.Services
{
    public class RecipientService
    {
        private SqlConnection _connection;

        public RecipientService(IDatabaseSettings settings)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = settings.ConnectionString;

            _connection = new SqlConnection(builder.ConnectionString);
        }

        public List<Recipient> Get(string user_id)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"select * from recipients where user_id = '{user_id}' and status = 1";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Declare List
            List<Recipient> recipientsCollection = new List<Recipient>();

            //Get data into list
            using (var reader = sqlQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    Recipient recipient = new Recipient();
                    recipient.id = reader.GetValue(0).ToString();
                    recipient.user_id = reader.GetValue(1).ToString();
                    recipient.recipient_id = reader.GetValue(2).ToString();
                    recipient.recipient_acc = reader.GetValue(3).ToString();
                    recipient.alias = reader.GetValue(4).ToString();
                    recipient.status = reader.GetValue(5).ToString();
                    recipient.fullname = reader.GetValue(6).ToString();

                    recipientsCollection.Add(recipient);
                }
            }

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return data
            return recipientsCollection;
        }

        public Recipient getBy(string field, string value)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"select * from recipients where {field} = '{value}'";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Get data into object
            Recipient recipient = new Recipient();
            using (var reader = sqlQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    recipient.id = reader.GetValue(0).ToString();
                    recipient.user_id = reader.GetValue(1).ToString();
                    recipient.recipient_id = reader.GetValue(2).ToString();
                    recipient.recipient_acc = reader.GetValue(3).ToString();
                    recipient.alias = reader.GetValue(4).ToString();
                    recipient.status = reader.GetValue(5).ToString();
                    recipient.fullname = reader.GetValue(6).ToString();
                }
            }

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return recipient
            return recipient;
        }

        public Recipient Create(Recipient recipient, string user_id)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"insert into recipients(user_id, recipient_id, recipient_acc, alias, status, fullname) values('{user_id}', '{recipient.recipient_id}', '{recipient.recipient_acc}', '{recipient.alias}', '1', '{recipient.fullname}')";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return created recipient
            return recipient;
        }

        public void Update(Recipient recipient)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"update recipients set recipient_acc = '{recipient.recipient_acc}', alias = '{recipient.alias}' where id = '{recipient.id}'";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();
        }

        public void Remove(string id)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"update recipients set status = '0' where id = '{id}'";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();
        }
            
    }
}

