using ORMExample.Models;
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
    public class CommandRunner<T> where T : class, new()
    {
        private static Type typeT = typeof(T);
        private string tableName = $"[dbo].[{typeT.Name}] ";
        private string _connectionString;

        public CommandRunner(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<T> GetItemList()
        {
            string sqlExpression = $"SELECT * FROM {tableName}";
            Collection<T> items = new Collection<T>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                            prop.SetValue(instance, reader.GetValue(i), null);
                        }
                        items.Add((T)instance);
                    }
                }
                reader.Close();
            }

            Console.WriteLine(sqlExpression);
            Console.WriteLine();

            foreach (T item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
            Console.ReadKey();

            return items;
        }

        public T GetItem(int Id)
        {
            string condition = "";
            string lowerTypeName = typeT.Name.ToLower();

            PropertyInfo[] propsInfo = typeT.GetProperties();

            for (int i = 0; i < propsInfo.Length; i++)
            {
                if (propsInfo[i].Name.ToString().ToLower().Contains(lowerTypeName + "id"))
                {
                    condition += propsInfo[i].Name + "="+  Id;
                    break;
                }
            }

            string sqlExpression = $"SELECT * FROM { tableName } WHERE {condition}";

            T item = new T();

            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                            prop.SetValue(instance, reader.GetValue(i), null);
                        }
                        item = (T)instance;
                    }
                }
                reader.Close();
            }

            Console.WriteLine(sqlExpression);
            Console.WriteLine();
            Console.WriteLine(item.ToString());
            Console.WriteLine();
            Console.ReadKey();

            return item;
        }

        public void Create(T item)
        {
            string lowerTypeName = typeT.Name.ToLower();
            string columns = "", props = "", tempProp;
            PropertyInfo[] propsInfo = typeT.GetProperties();

            for (int i = 0; i < propsInfo.Length; i++)
            {
                columns += i == propsInfo.Length - 1 ? propsInfo[i].Name : propsInfo[i].Name + ",";
                tempProp = propsInfo[i].PropertyType.ToString().Contains("String")
                    ? "\'" + GetPropValue(item, propsInfo[i].Name) + "\'"
                    : GetPropValue(item, propsInfo[i].Name).ToString();
                props += i == propsInfo.Length - 1 ? tempProp  : tempProp + ",";
            }
            string sqlExpression = $"SET IDENTITY_INSERT {tableName} ON \n INSERT INTO {tableName} ({columns}) VALUES ({props}) \n SET IDENTITY_INSERT {tableName} OFF";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine(sqlExpression);
                    Console.WriteLine();
                    Console.WriteLine(item.ToString());
                    Console.WriteLine();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Item with same Id already exist \n");
                }
                
            }            

            Console.ReadKey();
        }

        public void Update(T item)
        {
            string lowerTypeName = typeT.Name.ToLower();
            string props = "", condition = "", tempProp;
            PropertyInfo[] propsInfo = typeT.GetProperties();

            for (int i = 0; i < propsInfo.Length; i++)
            {
                if (propsInfo[i].Name.ToString().ToLower().Contains(lowerTypeName + "id"))
                {
                    condition = propsInfo[i].Name + "=" + GetPropValue(item, propsInfo[i].Name).ToString();
                    continue;
                }
                tempProp = propsInfo[i].PropertyType.ToString().Contains("String")
                     ? "\'" + GetPropValue(item, propsInfo[i].Name) + "\'"
                     : GetPropValue(item, propsInfo[i].Name).ToString();
                props += i == propsInfo.Length - 1 ? propsInfo[i].Name + "=" + tempProp : propsInfo[i].Name + "=" + tempProp + ",";
            }

            string sqlExpression = $"UPDATE {tableName} SET {props} WHERE {condition}";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }

            Console.WriteLine(sqlExpression);
            Console.WriteLine();
            Console.WriteLine(item.ToString());
            Console.WriteLine();
            Console.ReadKey();
        }

        public void Delete(int id)
        {
            string lowerTypeName = typeT.Name.ToLower();
            string condition = "";
            PropertyInfo[] propsInfo = typeT.GetProperties();

            for (int i = 0; i < propsInfo.Length; i++)
            {
                if (propsInfo[i].Name.ToString().ToLower().Contains(lowerTypeName + "id"))
                {
                    condition = propsInfo[i].Name + "=" + id;
                    break;
                }
            }

            string sqlExpression = $"DELETE FROM {tableName} WHERE {condition}";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }

            Console.WriteLine(sqlExpression);
            Console.WriteLine();
            Console.WriteLine("Deleted item with id = {0}", id);
            Console.WriteLine();
            Console.ReadKey();
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
