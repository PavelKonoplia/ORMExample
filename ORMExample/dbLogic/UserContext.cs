using ORMExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMExample
{
    public class UserContext: IRepository<User>
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private CommandRunner<User> commandRunner;

        public UserContext() {
            commandRunner = new CommandRunner<User>(connectionString);
        }
        public IEnumerable<User> GetItemsList()
        {
            return commandRunner.GetItemList();
        }
        public User GetItem(int userId)
        {
            return commandRunner.GetItem(userId);
        }
        public void Create(User u)
        {
            commandRunner.Create(u);
        }
        public void Update(User u)
        {
            commandRunner.Update(u);
        }
        public void Delete(int userId )
        {
            commandRunner.Delete(userId);
        }

    }
}
