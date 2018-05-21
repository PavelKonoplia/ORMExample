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
            TestUser();
        }

        public static void TestUser()
        {
            User user = new User(33, "Mate", 31);
            User user2 = new User(13, "Rom", 22);
            UserContext userContext = new UserContext();
            userContext.GetItemsList();
            userContext.GetItem(2);
            userContext.Create(user);
            userContext.Update(user2);
            userContext.Delete(6);
            userContext.GetItemsList();
        }
    }
}
