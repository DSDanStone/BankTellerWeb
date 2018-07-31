using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankTellerWeb.Models
{
    public class Address
    {
		/// <summary>
		/// Represents the street address
		/// </summary>
		[Required]
		public string Address1 { get; set; }

		/// <summary>
		/// Represents auxilary address info
		/// </summary>
		public string Address2 { get; set; }

		/// <summary>
		/// Represents the city
		/// </summary>
		[Required]
		public string City { get; set; }

		/// <summary>
		/// Represents the state
		/// </summary>
		[Required]
		public string State { get; set; }

		/// <summary>
		/// Represents the zip code
		/// </summary>
		[Required]
		public string Zip { get; set; }
	}
}
