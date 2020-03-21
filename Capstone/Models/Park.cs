using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {
        public int ParkID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateEst { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }

    }
}
