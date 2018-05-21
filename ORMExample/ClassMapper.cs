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
    public class ClassMapper<T> where T : class, new()
    {
        private Type typeT = typeof(T);

        public void GetItemList(string connectionString, string sqlExpression)
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
                            prop.SetValue(instance, reader.GetValue(i), null);
                        }
                        items.Add((T)instance);
                    }
                }
                reader.Close();
            }
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.ReadKey();
        }

        public void GetItem(string connectionString, string sqlExpression)
        {
            T item = new T();

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
                            prop.SetValue(instance, reader.GetValue(i), null);
                        }
                        item = (T)instance;
                    }
                }
                reader.Close();
            }
            Console.WriteLine(item.ToString());


            Console.ReadKey();
        }


        public void Create(string connectionString, T item)
        {
            string tableName = "[dbo].[" + typeT.Name + "] ";
            Type type = item.GetType();
            string columns = "(", props = "(", tempProp;
            int propsCount = type.GetProperties().Length;
            PropertyInfo[] propsInfo = type.GetProperties();
            for (int i = 0; i < propsCount; i++)
            {
                if (propsInfo[i].Name.ToString().Contains("ID"))
                {
                    continue;
                }
                columns += i == propsCount - 1 ? propsInfo[i].Name + ")" : propsInfo[i].Name + ",";
                tempProp = propsInfo[i].PropertyType.ToString().Contains("String") ? "\'" + GetPropValue(item, type.GetProperties()[i].Name) + "\'" : GetPropValue(item, type.GetProperties()[i].Name).ToString();
                props += i == propsCount - 1 ? tempProp + ")" : tempProp + ",";
            }
            string sqlExpression2 = "INSERT INTO " + tableName + columns + " VALUES " + props;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression2, connection);
                command.ExecuteNonQuery();
            }
            Console.ReadKey();
        }

        //TODO
        public void Update(string connectionString, T item)
        {
            string tableName = "[dbo].[" + typeT.Name + "] ";
            Type type = item.GetType();
            string columns = "(", props = "(", tempProp;
            int propsCount = type.GetProperties().Length;
            PropertyInfo[] propsInfo = type.GetProperties();
            for (int i = 0; i < propsCount; i++)
            {
                if (propsInfo[i].Name.ToString().Contains("ID"))
                {
                    continue;
                }
                columns += i == propsCount - 1 ? propsInfo[i].Name + ")" : propsInfo[i].Name + ",";
                tempProp = propsInfo[i].PropertyType.ToString().Contains("String") ? "\'" + GetPropValue(item, type.GetProperties()[i].Name) + "\'" : GetPropValue(item, type.GetProperties()[i].Name).ToString();
                props += i == propsCount - 1 ? tempProp + ")" : tempProp + ",";
            }
            string sqlExpression2 = "INSERT INTO " + tableName + columns + " VALUES " + props;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression2, connection);
                command.ExecuteNonQuery();
            }
            Console.ReadKey();
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
