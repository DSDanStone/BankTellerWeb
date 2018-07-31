using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTellerWeb.Models
{
	public abstract class BankAccount
	{
		/// <summary>
		/// Represents the Account Number
		/// </summary>
		public string AccountNumber { get; set; }

		/// <summary>
		/// Represents a title for the account (e.g. Dan's Checking Account)
		/// </summary>
		public string AccountName { get; set; }

		/// <summary>
		/// Represents the Account Balance
		/// </summary>
		public decimal Balance { get; private set; }

		/// <summary>
		/// Creates a new empty bank account
		/// </summary>
		public BankAccount()
		{
			this.Balance = 0;
		}

		/// <summary>
		/// Makes a deposit to the account
		/// </summary>
		/// <param name="amountToDeposit">The ammount to deposit</param>
		/// <returns>The new accout balance</returns>
		public virtual decimal Deposit(decimal amountToDeposit)
		{
			this.Balance += amountToDeposit;
			return this.Balance;
		}

		/// <summary>
		/// Makes a withdrawl from the account
		/// </summary>
		/// <param name="amountToWithdraw">The ammount to withdraw</param>
		/// <returns>The new account balance</returns>
		public virtual decimal Withdraw(decimal amountToWithdraw)
		{
			this.Balance -= amountToWithdraw;
			return this.Balance;
		}

		/// <summary>
		/// Transfers funds from this account to another
		/// </summary>
		/// <param name="destinationAccount">The account to transfer to</param>
		/// <param name="transferAmount">The amount to transfer</param>
		public void Transfer(BankAccount destinationAccount, decimal transferAmount)
		{
			if (!this.Equals(destinationAccount))
			{
				decimal initialBalance = this.Balance;
				decimal finalBalance = this.Withdraw(transferAmount);

				if (initialBalance != finalBalance)
				{
					destinationAccount.Deposit(transferAmount);
				}
			}
		}
	}
}
