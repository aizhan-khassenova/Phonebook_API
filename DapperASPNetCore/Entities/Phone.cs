using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Entities
{
	public class Phone
	{
		public int Phone_ID { get; set; }
		public string Phone_Number { get; set; }
		public string Owner_Name { get; set; }
		public int Apartment_ID { get; set; }
	}
}
