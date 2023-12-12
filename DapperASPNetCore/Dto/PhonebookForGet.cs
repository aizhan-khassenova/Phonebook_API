using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Repository
{
    public class PhonebookForGet
    {
        public int City_ID { get; set; }
        public string City_Name { get; set; }

        public int Street_ID { get; set; }
        public string Street_Name { get; set; }

        public int House_ID { get; set; }
        public int House_Number { get; set; }

        public int Apartment_ID { get; set; }
        public int Apartment_Number { get; set; }

        public int Phone_ID { get; set; }
        public string Phone_Number { get; set; }
        public string Owner_Name { get; set; }
    }
}