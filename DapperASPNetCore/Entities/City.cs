using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Entities
{
	public class City
	{
		public int City_ID { get; set; }
		public string City_Name { get; set; }

		public List<Street> Streets { get; set; } = new List<Street>();
	}
}
