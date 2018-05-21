using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMExample.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public User()
        {
        }

        public User(int id, string name, int age)
        {
            UserID = id;
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return "UserID: "+UserID + " Name: " + Name + " Age " + Age;
        }
    }
}
