using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Entities
{
	public class Apartment
	{
		public int Apartment_ID { get; set; }
		public int Apartment_Number { get; set; }
		public int House_ID { get; set; }

		public List<Phone> Phones { get; set; } = new List<Phone>();
	}
}
