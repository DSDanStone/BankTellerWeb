using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankTellerWeb.Models
{
	public class AccountCreatorModel
	{
		public string Name { get; set; }

		[Required]
		public string Type { get; set; }

		public static List<SelectListItem> AccountTypes = new List<SelectListItem>()
		{
			new SelectListItem(){Text = "(Pick a Type)", Value = "" },
			new SelectListItem(){Text = "Checking", Value = "Checking" },
			new SelectListItem(){Text = "Savings", Value = "Savings" }
		};
	}
}
