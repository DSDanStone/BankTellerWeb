using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BankTellerWeb.Models;
using BankTellerWeb.DAL;
using BankTellerWeb.Extensions;

namespace BankTellerWeb.Controllers
{
	public class AccountController : Controller
	{
		private static readonly string ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=BankDB;Integrated Security=True";

		private ICustomerDAL customerDAL = new CustomerDAL(ConnectionString);
		private IAccountDAL accountDAL = new AccountDAL(ConnectionString);
		private ILogDAL logDAL = new LogDAL(ConnectionString);

		private const string Session_Key = "User_ID";
		private const string Failed_Login = "Failed_Login";

		//public AccountController(ICustomerDAL customerDAL, IAccountDAL accountDAL, ILogDAL logDAL)
		//{
		//	this.customerDAL = customerDAL;
		//	this.accountDAL = accountDAL;
		//	this.logDAL = logDAL;
		//}

		public IActionResult Index()
		{
			HttpContext.Session.Set(Session_Key, 1);
			if (HttpContext.Session.Get(Session_Key) != null)
			{
				return RedirectToAction("loggedinhome");
			}
			else
			{
				return View();
			}
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult VerifyLogin(LoginCredentials login)
		{
			int id = customerDAL.ConfirmLogin(login);
			if (id == -1)
			{
				TempData[Failed_Login] = true;
				return RedirectToAction("login");
			}
			else
			{
				HttpContext.Session.Set(Session_Key, id);
				return RedirectToAction("index", true);
			}
		}

		public IActionResult LogOut()
		{
			HttpContext.Session.Clear();
			return View();
		}

		public IActionResult LoggedInHome()
		{
			int id = HttpContext.Session.Get<int>(Session_Key);
			BankCustomer customer = customerDAL.GetCustomerDetails(id);
			customer.Accounts = accountDAL.GetUsersAccounts(id);
			return View(customer);
		}

		[HttpGet]
		public IActionResult AddAccount()
		{
			return View(new AccountCreatorModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddAccount(AccountCreatorModel account)
		{
			int userId = HttpContext.Session.Get<int>(Session_Key);
			int accountId = accountDAL.CreateAccount(account);
			accountDAL.AddAccountToUser(accountId, userId);
			return RedirectToAction("index");
		}

		public IActionResult Deposit()
		{
			return View();
		}

		[HttpGet]
		public IActionResult NewUser()
		{
			BankCustomer customer = new BankCustomer() { Address = new Address(), Login = new LoginCredentials() };
			return View(customer);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult NewUser(BankCustomer customer)
		{
			if (customerDAL.IsAvailableUsername(customer.Login.UserName))
			{
				int id = customerDAL.CreateNewUser(customer);
				HttpContext.Session.Set(Session_Key, id);
				return RedirectToAction("index");
			}
			customer.Login.UserName = "Not An Available Username";
			return View(customer);
		}

		public IActionResult ShowBalance()
		{
			return View();
		}

		public IActionResult ShowTransaction()
		{
			return View();
		}

		public IActionResult Transfer()
		{
			return View();
		}

		public IActionResult UpdateLogin()
		{
			return View();
		}

		[HttpGet]
		public IActionResult UpdatePersonalInfo()
		{
			int id = HttpContext.Session.Get<int>(Session_Key);
			BankCustomer customer = customerDAL.GetCustomerDetails(id);
			return View(customer);
		}

		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public IActionResult UpdatePersonalInfo(BankCustomer customer)
		{
			int id = HttpContext.Session.Get<int>(Session_Key);
			customerDAL.UpdateUserInfo(customer, id);
			return RedirectToAction("index");
		}

		public IActionResult Withdraw()
		{
			return View();
		}
	}
}