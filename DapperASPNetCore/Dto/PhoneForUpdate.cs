using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Dto
{
	public class PhoneForUpdate
	{
		public string Phone_Number { get; set; }
		public string Owner_Name { get; set; }
	}
}
