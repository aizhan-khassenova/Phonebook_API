using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Entities
{
	public class Street
	{
		public int Street_ID { get; set; }
		public string Street_Name { get; set; }

		public List<House> Houses { get; set; } = new List<House>();

	}
}
