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
            //Tester.Test();

            // Tester.Test2();
            TestUser();
        }

        public static void TestUser()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlExpression = "SELECT * FROM [dbo].[User]";
            int UserId = 2;
            string sqlExpression2 = "SELECT * FROM [dbo].[User] WHERE UserId="+UserId;

            User u = new User(13, "Jacky", 44);
            ClassMapper<User> classMapper = new ClassMapper<User>();
            // classMapper.GetItemList(connectionString, sqlExpression);
            //classMapper.GetItem(connectionString, sqlExpression2);
            classMapper.Create(connectionString, u);

        }
    }
}
