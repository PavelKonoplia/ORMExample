using System.Configuration;
using ORMExample.DbLogic;
using ORMExample.Models;

namespace ORMExample
{
    public class Program
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static void TestUser()
        {
            Repository<User> usersContext = new Repository<User>(connectionString);
            User user = new User(33, "Mate", 31);
            User user2 = new User(13, "Rom", 22);
            usersContext.GetItemsList();
            usersContext.GetItem(112);
            usersContext.Create(user);
            usersContext.Update(user2);
            usersContext.Delete(6);
            usersContext.GetItemsList();
        }
        
        public static void TestProduct()
        {
            Repository<Product> productsContext = new Repository<Product>(connectionString);
            Product prod = new Product(13, "Rom-cola", 224);
            Product prod2 = new Product(13, "Rom", 22);
            productsContext.GetItemsList();
            productsContext.GetItem(12);
            productsContext.Create(prod);
            productsContext.Update(prod2);
            productsContext.Delete(6);
            productsContext.GetItemsList();
        }

        public static void Main(string[] args)
        {
            // TestUser();
            TestProduct();
        }      
    }
}
