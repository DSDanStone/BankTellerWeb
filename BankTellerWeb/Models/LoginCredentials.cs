﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTellerWeb.Models
{
    public class LoginCredentials
    {
		[Required]
		[MinLength(6)]
		public string UserName { get; set; }

		[Required]
		[MinLength(6)]
		public string Password { get; set; }
    }
}
