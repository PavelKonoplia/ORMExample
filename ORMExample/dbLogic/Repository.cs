using System.Collections.Generic;
using System.Configuration;

namespace ORMExample.DbLogic
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private CommandRunner<T> commandRunner;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
            commandRunner = new CommandRunner<T>(_connectionString);
        }

        public IEnumerable<T> GetItemsList()
        {
            return commandRunner.GetItemList();
        }

        public T GetItem(int userId)
        {
            return commandRunner.GetItem(userId);
        }

        public void Create(T u)
        {
            commandRunner.Create(u);
        }

        public void Update(T u)
        {
            commandRunner.Update(u);
        }

        public void Delete(int userId)
        {
            commandRunner.Delete(userId);
        }
    }
}
