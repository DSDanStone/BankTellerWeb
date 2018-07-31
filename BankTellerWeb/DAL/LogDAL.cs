using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTellerWeb.DAL
{
	public class LogDAL : ILogDAL
	{
		private readonly string ConnectionString;

		public LogDAL(string connectionString)
		{
			this.ConnectionString = connectionString;
		}
	}
}
