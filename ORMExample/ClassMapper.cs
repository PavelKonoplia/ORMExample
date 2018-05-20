using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORMExample
{
    public class ClassMapper<T> where T : class, new()
    {
        private Type typeT = typeof(T);

        public void GetAll(string connectionString, string sqlExpression)
        {
            Collection<T> items = new Collection<T>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) 
                {
                    DataTable dataTable = reader.GetSchemaTable();
                    while (reader.Read())
                    {
                        object instance = Activator.CreateInstance(typeT);
                        
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            PropertyInfo prop = typeT.GetProperty(dataTable.Rows[i].ItemArray[9].ToString());
                           // Console.WriteLine(prop.Name);
                           // Console.WriteLine(reader.GetValue(i));
                            prop.SetValue(instance, reader.GetValue(i), null);

                            Console.WriteLine(prop);
                        }
                        items.Add((T)instance);
                    }
                }

                reader.Close();
            }
            Console.WriteLine(items.Count);
        }
        /*
        public T MapToAdd(N n) {
            Type typeT = typeof(T);
            Type typeN = typeof(N);
            // create an instance of that type
            object instance = Activator.CreateInstance(typeT);
            // Get a property on the type that is stored in the 
            // property string
            PropertyInfo prop = type.GetProperty(property);
            // Set the value of the given property on the given instance
            prop.SetValue(instance, value, null);
            // at this stage instance.Bar will equal to the value


            return t;
        }*/
    }
}
