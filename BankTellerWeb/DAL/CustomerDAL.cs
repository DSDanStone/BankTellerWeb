using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTellerWeb.Models;
using System.Data.SqlClient;

namespace BankTellerWeb.DAL
{
	public class CustomerDAL : ICustomerDAL
	{
		private readonly string ConnectionString;

		/// <summary>
		/// Creates a new CustomerDAL object
		/// </summary>
		/// <param name="connectionString">The connection string for the relevant database</param>
		public CustomerDAL(string connectionString)
		{
			this.ConnectionString = connectionString;
		}

		/// <summary>
		/// Confirms the user's login
		/// </summary>
		/// <param name="username">The user's provided username</param>
		/// <param name="password">The user's provided password</param>
		/// <returns>-1 for failed login or the customer_id for valid login</returns>
		public int ConfirmLogin(LoginCredentials login)
		{
			// Initialize output variable
			int id = -1;

			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();

					string sql = $"SELECT customer_id FROM customers WHERE username = @username AND password = @password;";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@username", login.UserName);
					cmd.Parameters.AddWithValue("@password", login.Password);

					// Confirm the existence of the login and assign the custoemr_id to id
					object userObj = cmd.ExecuteScalar();
					if (userObj != null)
					{
						id = Convert.ToInt32(userObj);
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}

			// Return the customer Id
			return id;
		}

		/// <summary>
		/// Add a new user to the database
		/// </summary>
		/// <param name="customer">The customer to add to the database</param>
		/// <returns>The new user ID of the created user or -1 if it fails</returns>
		public int CreateNewUser(BankCustomer customer)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					string sql = "INSERT INTO customers (first_name, last_name, username, password, address1, address2, city, state, zipcode) VALUES (@fname, @lname, @userName, @password, @address1, @address2, @city, @state, @zip);";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@fname", customer.FirstName);
					cmd.Parameters.AddWithValue("@lname", customer.LastName);
					cmd.Parameters.AddWithValue("@userName", customer.Login.UserName);
					cmd.Parameters.AddWithValue("@password", customer.Login.Password);
					cmd.Parameters.AddWithValue("@address1", customer.Address.Address1);
					cmd.Parameters.AddWithValue("@address2", customer.Address.Address2);
					cmd.Parameters.AddWithValue("@city", customer.Address.City);
					cmd.Parameters.AddWithValue("@state", customer.Address.State);
					cmd.Parameters.AddWithValue("@zip", customer.Address.Zip);
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return ConfirmLogin(customer.Login);
		}

		/// <summary>
		/// Retrieves user details from the database
		/// </summary>
		/// <param name="id">The database id of the user</param>
		/// <returns>A customer's full details</returns>
		public BankCustomer GetCustomerDetails(int id)
		{
			BankCustomer customer = new BankCustomer();
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();

					string sql = $"SELECT * FROM customers WHERE customer_id = {id};";
					SqlCommand cmd = new SqlCommand(sql, conn);

					SqlDataReader reader = cmd.ExecuteReader();
					if (reader.Read())
					{
						customer = TranslateReaderToBankCustomer(reader);
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}

			return customer;
		}

		/// <summary>
		/// Check to see if a username exists in the database already
		/// </summary>
		/// <param name="username">The username to check</param>
		/// <returns>True if the username is available; false if it's already taken</returns>
		public bool IsAvailableUsername(string username)
		{
			bool isAvailable = true;
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					string sql = "SELECT * FROM customers WHERE username = @username;";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@username", username);
					SqlDataReader reader = cmd.ExecuteReader();
					if (reader.Read())
					{
						isAvailable = false;
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return isAvailable;
		}

		/// <summary>
		/// Updates the user's person info in the database
		/// </summary>
		/// <param name="customer">The customer details to update</param>
		/// <param name="id">The customer's id in the database</param>
		public void UpdateUserInfo(BankCustomer customer, int id)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					string sql = $"UPDATE customers SET first_name = @fname, last_name = @lname, address1 = @address1, address2 = @address2, city = @city, state = @state, zipcode = @zip WHERE customer_id = {id};";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@fname", customer.FirstName);
					cmd.Parameters.AddWithValue("@lname", customer.LastName);
					cmd.Parameters.AddWithValue("@address1", customer.Address.Address1);
					cmd.Parameters.AddWithValue("@address2", customer.Address.Address2);
					cmd.Parameters.AddWithValue("@city", customer.Address.City);
					cmd.Parameters.AddWithValue("@state", customer.Address.State);
					cmd.Parameters.AddWithValue("@zip", customer.Address.Zip);
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Translates a SQLDataReader line to a BankCustomer Object
		/// </summary>
		/// <param name="reader">The reader to get the data from</param>
		/// <returns>A BankCustomer populated by the database</returns>
		private BankCustomer TranslateReaderToBankCustomer(SqlDataReader reader)
		{
			return new BankCustomer()
			{
				FirstName = Convert.ToString(reader["first_name"]),
				LastName = Convert.ToString(reader["last_name"]),
				Address = new Address()
				{
					Address1 = Convert.ToString(reader["address1"]),
					Address2 = Convert.ToString(reader["address2"]),
					City = Convert.ToString(reader["city"]),
					State = Convert.ToString(reader["state"]),
					Zip = Convert.ToString(reader["zipcode"])
				}
			};
		}
	}
}
