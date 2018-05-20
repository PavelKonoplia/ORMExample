using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMExample.Models
{
    public class Product
    {
        public int ProductID { get; }
        public string Name { get; set; }
        public double Cost { get; set; }

        public Product(int id, string name, double cost)
        {
            ProductID = id;
            Name = name;
            Cost = cost;
        }
    }
}
