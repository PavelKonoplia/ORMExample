namespace ORMExample.Models
{
    public class User
    {
        public User() { }

        public User(int id, string name, int age)
        {
            UserID = id;
            Name = name;
            Age = age;
        }

        public int UserID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"UserID:{UserID} Name: {Name} Age: {Age}";
        }
    }
}
