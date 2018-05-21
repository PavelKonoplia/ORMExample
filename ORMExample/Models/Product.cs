﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMExample.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Product()
        {
        }

        public Product(int id, string name, int price)
        {
            ProductID = id;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return "ProductID: " + ProductID + " Name: " + Name + " Price " + Price;
        }
    }
}
