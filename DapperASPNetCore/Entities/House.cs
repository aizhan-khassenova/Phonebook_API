using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Entities
{
	public class House
	{
		public int House_ID { get; set; }
		public int House_Number { get; set; }
		public int Street_ID { get; set; }

		public List<Apartment> Apartments { get; set; } = new List<Apartment>();
	}
}
