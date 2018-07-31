using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTellerWeb.Models
{
	public class SavingsAccount : BankAccount
	{
		/// <summary>
		/// Withdraws from the account, while accounting for overdraft and fees
		/// </summary>
		/// <param name="amountToWithdraw">The amount to withdraw</param>
		/// <returns>The new account balance</returns>
		public override decimal Withdraw(decimal amountToWithdraw)
		{
			if (this.Balance < amountToWithdraw)
			{
				return this.Balance;
			}
			if (this.Balance < 150)
			{
				base.Withdraw(2);
			}
			return base.Withdraw(amountToWithdraw);
		}
	}
}
