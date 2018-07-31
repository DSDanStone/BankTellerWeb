using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BankTellerWeb.Models;

namespace BankTellerWeb.DAL
{
	public class AccountDAL : IAccountDAL
	{
		/// <summary>
		/// Holds the connection string for the DAL
		/// </summary>
		private string ConnectionString;

		public AccountDAL(string connectionString)
		{
			this.ConnectionString = connectionString;
		}

		/// <summary>
		/// Retieves an account based on its unique ID
		/// </summary>
		/// <param name="id">the ID number of the bank account</param>
		/// <returns></returns>
		public BankAccount GetAccount(int id)
		{
			BankAccount account = null;
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();

					string sql = $"SELECT * FROM accounts WHERE account_id = {id};";
					SqlCommand cmd = new SqlCommand(sql, conn);

					SqlDataReader reader = cmd.ExecuteReader();
					if (reader.Read())
					{
						account = TranslateReaderToBankAccount(reader);
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}

			return account;
		}

		/// <summary>
		/// Adds a new account to the Database
		/// </summary>
		/// <param name="account">The account to be added</param>
		/// <returns>The ID number of the new account</returns>
		public int CreateAccount(AccountCreatorModel account)
		{
			int accountId = 0;
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					string sql = "INSERT INTO accounts (account_name, account_type, account_number) VALUES (@name, @type, @number);";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@name", account.Name);
					cmd.Parameters.AddWithValue("@type", account.Type);

					// Generate a random Account Number
					Random random = new Random();
					int newNumber = random.Next(1000000000);
					while (!IsAvailableNumber(newNumber))
					{
						newNumber = random.Next(1000000000);
					}
					string newAccountNumber = newNumber.ToString("000000000");
					cmd.Parameters.AddWithValue("@number", newAccountNumber);

					cmd.ExecuteNonQuery();

					sql = $"SELECT account_id FROM accounts WHERE account_number = {newNumber};";
					cmd = new SqlCommand(sql, conn);
					SqlDataReader reader = cmd.ExecuteReader();
					reader.Read();
					accountId = Convert.ToInt32(reader["account_id"]);
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			return accountId;
		}

		/// <summary>
		/// Assigns an existing account to a user
		/// </summary>
		/// <param name="accountId">The id number of the account</param>
		/// <param name="userId">The user's id number</param>
		public void AddAccountToUser(int accountId, int userId)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					string sql = $"INSERT INTO customer_account (customer_id, account_id) VALUES ({userId}, {accountId});";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.ExecuteNonQuery();
				}

			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Retrieves a list of User Accounts
		/// </summary>
		/// <param name="id">The user's Id number</param>
		/// <returns>A list of bank accoutns the user has</returns>
		public IList<BankAccount> GetUsersAccounts(int id)
		{
			List<BankAccount> accounts = new List<BankAccount>();
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();

					string sql = $"SELECT * FROM accounts INNER JOIN customer_account ON accounts.account_id = customer_account.account_id WHERE customer_account.customer_id = {id};";
					SqlCommand cmd = new SqlCommand(sql, conn);

					SqlDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						accounts.Add(TranslateReaderToBankAccount(reader));
					}
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine(ex.Message);
			}

			return accounts;
		}

		/// <summary>
		/// Converts a SQLDataReader line into a BankAccount Object
		/// </summary>
		/// <param name="reader">The reader to read from</param>
		/// <returns>A BankAccount object populated from the database</returns>
		private BankAccount TranslateReaderToBankAccount(SqlDataReader reader)
		{
			BankAccount account;
			if (Convert.ToString(reader["account_type"]) == "Savings")
			{
				account = new SavingsAccount();
			}
			else
			{
				account = new CheckingAccount();
			}
			account.AccountName = Convert.ToString(reader["account_name"]);
			account.AccountNumber = Convert.ToString(reader["account_number"]);
			account.Deposit(Convert.ToDecimal(reader["balance"]));
			return account;
		}

		/// <summary>
		/// Checks whether a given number exists as an account number in the database
		/// </summary>
		/// <param name="newNumber">A number to check</param>
		/// <returns>True if the number is available; false if it's already taken</returns>
		private bool IsAvailableNumber(int newNumber)
		{
			bool isAvailable = true;
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					string sql = "SELECT * FROM accounts WHERE account_number = @number;";
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.AddWithValue("@number", newNumber.ToString("000000000"));
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
	}
}
