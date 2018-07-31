using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTellerWeb.Models;

namespace BankTellerWeb.DAL
{
    public interface ICustomerDAL
    {
		BankCustomer GetCustomerDetails(int id);
		int ConfirmLogin(LoginCredentials login);
		int CreateNewUser(BankCustomer customer);
		bool IsAvailableUsername(string userName);
		void UpdateUserInfo(BankCustomer customer, int id);
	}
}
