using System.Collections.Generic;

namespace ORMExample
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetItemsList(); 

        T GetItem(int id);  

        void Create(T item); 

        void Update(T item); 

        void Delete(int id); 
    }
}
