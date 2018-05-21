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
        static void Main(string[] args)
        {
            //TestUser();
            TestProduct();
        }

        public static void TestUser()
        {
            User user = new User(33, "Mate", 31);
            User user2 = new User(13, "Rom", 22);
            UserContext userContext = new UserContext();
            userContext.UsersContext.GetItemsList();
            userContext.UsersContext.GetItem(112);
            userContext.UsersContext.Create(user);
            userContext.UsersContext.Update(user2);
            userContext.UsersContext.Delete(6);
            userContext.UsersContext.GetItemsList();
        }
        public static void TestProduct()
        {
            Product prod = new Product(13, "Rom-cola", 224);
            Product prod2 = new Product(13, "Rom", 22);
            ProductContext userContext = new ProductContext();
            userContext.ProductsContext.GetItemsList();
            userContext.ProductsContext.GetItem(12);
            userContext.ProductsContext.Create(prod);
            userContext.ProductsContext.Update(prod2);
            userContext.ProductsContext.Delete(6);
            userContext.ProductsContext.GetItemsList();
        }
    }
}
