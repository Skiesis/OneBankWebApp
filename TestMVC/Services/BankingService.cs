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
    public class BankingService
    {
        private SqlConnection _connection;

        public BankingService(IDatabaseSettings settings)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = settings.ConnectionString;

            _connection = new SqlConnection(builder.ConnectionString);
        }

        public List<Product> getUserProducts(string user_id)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"select * from products where user_id = '{user_id}'";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Declare List
            List<Product> productCollection = new List<Product>();

            //Get data into list
            using (var reader = sqlQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product();
                    product.id = reader.GetValue(0).ToString();
                    product.acc_number = reader.GetValue(1).ToString(); 
                    product.currency = reader.GetValue(2).ToString(); 
                    product.balance = reader.GetValue(3).ToString(); 
                    product.available_balance = reader.GetValue(4).ToString(); 
                    product.user_id = reader.GetValue(5).ToString(); 
                    product.product_type = reader.GetValue(6).ToString();

                    productCollection.Add(product);
                }
            }

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return data
            return productCollection;
        }

        public Product getProductBy(string field, string value)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"select * from products where {field} = '{value}'";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Get data into user object
            Product product = new Product();
            using (var reader = sqlQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    product.id = reader.GetValue(0).ToString();
                    product.acc_number = reader.GetValue(1).ToString();
                    product.currency = reader.GetValue(2).ToString();
                    product.balance = reader.GetValue(3).ToString();
                    product.available_balance = reader.GetValue(4).ToString();
                    product.user_id = reader.GetValue(5).ToString();
                    product.product_type = reader.GetValue(6).ToString();
                }
            }

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return product
            return product;
        }

        public Product getProductByOf(string field, string value, string user_id)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"select * from products where {field} = '{value}' and user_id = {user_id}";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Get data into user object
            Product product = new Product();
            using (var reader = sqlQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    product.id = reader.GetValue(0).ToString();
                    product.acc_number = reader.GetValue(1).ToString();
                    product.currency = reader.GetValue(2).ToString();
                    product.balance = reader.GetValue(3).ToString();
                    product.available_balance = reader.GetValue(4).ToString();
                    product.user_id = reader.GetValue(5).ToString();
                    product.product_type = reader.GetValue(6).ToString();
                }
            }

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return product
            return product;
        }

        public List<Transaction> getTransactionsOf(string product)
        {
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"select * from transactions where acc_from = '{product}' or acc_to = '{product}'";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Declare List
            List<Transaction> transactionCollection = new List<Transaction>();

            //Get data into list
            using (var reader = sqlQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    Transaction transaction = new Transaction();
                    transaction.id = reader.GetValue(0).ToString();
                    transaction.acc_from = reader.GetValue(1).ToString();
                    transaction.acc_to = reader.GetValue(2).ToString();
                    transaction.trans_type = reader.GetValue(3).ToString();
                    transaction.date_done = reader.GetValue(4).ToString();
                    transaction.date_completed = reader.GetValue(5).ToString();
                    transaction.status = reader.GetValue(6).ToString();
                    transaction.done_by = reader.GetValue(7).ToString();
                    transaction.description = reader.GetValue(8).ToString();
                    transaction.done_to = reader.GetValue(9).ToString();
                    transaction.bank = reader.GetValue(10).ToString();
                    transaction.amount = reader.GetValue(11).ToString();

                    transactionCollection.Add(transaction);
                }
            }

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

            //Return data
            return transactionCollection;
        }

        public void makeSelfTransaction(Transaction transaction)
        {
            Product product_from = getProductBy("acc_number", transaction.acc_from);
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"insert into transactions(acc_from, acc_to, trans_type, date_done, status, done_by, description, done_to, bank, amount) values('{transaction.acc_from}', '{transaction.acc_to}', '{transaction.trans_type}', '{DateTime.Now.ToString()}', '{transaction.status}', '{transaction.done_by}', '{transaction.description}', '{transaction.done_to}', '{transaction.bank}', '{transaction.amount}')";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Update available balance from product from
            sql = $"update products set available_balance = '{(Convert.ToInt32(product_from.available_balance) - Convert.ToInt32(transaction.amount))}' where acc_number = '{product_from.acc_number}'";
            sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

        }

        public void makeOtherTransaction(Transaction transaction)
        {
            Product product_from = getProductBy("acc_number", transaction.acc_from);
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"insert into transactions(acc_from, acc_to, trans_type, date_done, status, done_by, description, done_to, bank, amount) values('{transaction.acc_from}', '{transaction.acc_to}', '{transaction.trans_type}', '{DateTime.Now.ToString()}', '{transaction.status}', '{transaction.done_by}', '{transaction.description}', '{transaction.done_to}', '{transaction.bank}', '{transaction.amount}')";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Update available balance from product from
            sql = $"update products set available_balance = '{(Convert.ToInt32(product_from.available_balance) - Convert.ToInt32(transaction.amount))}' where acc_number = '{product_from.acc_number}'";
            sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

        }

        public void makeInterbankingTransaction(Transaction transaction)
        {
            Product product_from = getProductBy("acc_number", transaction.acc_from);
            //Open connection
            _connection.Open();

            //Define command
            string sql = $"insert into transactions(acc_from, acc_to, trans_type, date_done, status, done_by, description, done_to, bank, amount) values('{transaction.acc_from}', '{transaction.acc_to}', '{transaction.trans_type}', '{DateTime.Now.ToString()}', '{transaction.status}', '{transaction.done_by}', '{transaction.description}', '{transaction.done_to}', '{transaction.bank}', '{transaction.amount}')";
            SqlCommand sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Update available balance from product from
            sql = $"update products set available_balance = '{(Convert.ToInt32(product_from.available_balance) - Convert.ToInt32(transaction.amount))}' where acc_number = '{product_from.acc_number}'";
            sqlQuery = new SqlCommand(sql, _connection);

            //Execute command
            sqlQuery.ExecuteNonQuery();

            //Close connection
            sqlQuery.Dispose();
            _connection.Close();

        }

    }
}
