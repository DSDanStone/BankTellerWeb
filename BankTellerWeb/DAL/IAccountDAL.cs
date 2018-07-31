using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTellerWeb.Models;

namespace BankTellerWeb.DAL
{
    public interface IAccountDAL
    {
		IList<BankAccount> GetUsersAccounts(int id);
		BankAccount GetAccount(int id);
		int CreateAccount(AccountCreatorModel account);
		void AddAccountToUser(int accountId, int userId);
	}
}
