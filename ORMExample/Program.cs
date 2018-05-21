using ORMExample.dbLogic;
using ORMExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMExample
{
    class Program
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static void Main(string[] args)
        {
            //TestUser();
            TestProduct();
        }

        public static void TestUser()
        {
            Repository<User> UsersContext = new Repository<User>(connectionString);
            User user = new User(33, "Mate", 31);
            User user2 = new User(13, "Rom", 22);
            UsersContext.GetItemsList();
            UsersContext.GetItem(112);
            UsersContext.Create(user);
            UsersContext.Update(user2);
            UsersContext.Delete(6);
            UsersContext.GetItemsList();
        }
        public static void TestProduct()
        {
            Repository<Product> ProductsContext = new Repository<Product>(connectionString);
            Product prod = new Product(13, "Rom-cola", 224);
            Product prod2 = new Product(13, "Rom", 22);
            ProductsContext.GetItemsList();
            ProductsContext.GetItem(12);
            ProductsContext.Create(prod);
            ProductsContext.Update(prod2);
            ProductsContext.Delete(6);
            ProductsContext.GetItemsList();
        }
    }
}
