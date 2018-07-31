using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTellerWeb.Models
{
	public class CheckingAccount : BankAccount
	{
		/// <summary>
		/// Withdraws from the account, while accounting for overdraft and fees
		/// </summary>
		/// <param name="amountToWithdraw">The amount to withdraw</param>
		/// <returns>The new account balance</returns>
		public override decimal Withdraw(decimal amountToWithdraw)
		{
			if (this.Balance - amountToWithdraw <= -100)
			{
				return this.Balance;
			}
			if (base.Withdraw(amountToWithdraw) < 0)
			{
				return base.Withdraw(10);
			}
			return this.Balance;
		}
	}
}
