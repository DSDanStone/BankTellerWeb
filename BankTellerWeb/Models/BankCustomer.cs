using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankTellerWeb.Models
{
	public class BankCustomer
	{
		/// <summary>
		/// Represents the customer's First Name
		/// </summary>
		[Required]
		public string FirstName { get; set; }

		/// <summary>
		/// Represents the customer's Last Name
		/// </summary>
		[Required]
		public string LastName { get; set; }

		/// <summary>
		/// Represents the customer's address
		/// </summary>
		[Required]
		public Address Address { get; set; }

		/// <summary>
		/// Represents the customer's phone number
		/// </summary>
		[Required]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Holds the customers accounts
		/// </summary>
		public IList<BankAccount> Accounts { get; set; }

		/// <summary>
		/// Holds the user's login credentials
		/// </summary>
		[Required]
		public LoginCredentials Login { get; set; }

		/// <summary>
		/// Represents whether the customer is a VIP
		/// </summary>
		public bool IsVIP
		{
			get
			{
				decimal totalWorth = 0;
				foreach (BankAccount account in Accounts)
				{
					totalWorth += account.Balance;
				}
				return (totalWorth >= 25000M);
			}
		}
	}
}
