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
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    object instance = Activator.CreateInstance(typeT);
                    for (int j = 0; j < ds.Tables[0].Rows[i].ItemArray.Length; j++)
                    {
                        PropertyInfo prop = typeT.GetProperty(ds.Tables[0].Columns[j].ToString());
                        prop.SetValue(instance, ds.Tables[0].Rows[i].ItemArray[j], null);
                    }
                    items.Add((T)instance);
                }
                adapter.Dispose();
                ds.Dispose();
            }

            Console.WriteLine(sqlExpression+"\n");
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
            string lowerTypeName = typeT.Name.ToLower(), condition = "";
            PropertyInfo[] propsInfo = typeT.GetProperties();

            for (int i = 0; i < propsInfo.Length; i++)
            {
                if (propsInfo[i].Name.ToString().ToLower().Contains(lowerTypeName + "id"))
                {
                    condition += propsInfo[i].Name + "=" + Id;
                    break;
                }
            }

            string sqlExpression = $"SELECT * FROM { tableName } WHERE {condition}";
            T item = new T();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                DataSet ds = new DataSet();
                try
                {
                    adapter.Fill(ds);
                    object instance = Activator.CreateInstance(typeT);
                    for (int j = 0; j < ds.Tables[0].Rows[0].ItemArray.Length; j++)
                    {
                        PropertyInfo prop = typeT.GetProperty(ds.Tables[0].Columns[j].ToString());
                        prop.SetValue(instance, ds.Tables[0].Rows[0].ItemArray[j], null);
                    }
                    item = (T)instance;
                    ShowResult(sqlExpression, item.ToString());
                }
                catch (Exception)
                {
                    Console.WriteLine("Item with this id was not found! \n");                   
                }              
                adapter.Dispose();
                ds.Dispose();
            }
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
                props += i == propsInfo.Length - 1 ? tempProp : tempProp + ",";
            }
            string sqlExpression = $"SET IDENTITY_INSERT {tableName} ON \n INSERT INTO {tableName} ({columns}) VALUES ({props}) \n SET IDENTITY_INSERT {tableName} OFF";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                try
                {
                    command.ExecuteNonQuery();
                    ShowResult(sqlExpression, item.ToString());
                }
                catch (Exception)
                {
                    Console.WriteLine("Error when creating Item, check your model. \n");
                }
            }            
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

                try
                {
                    command.ExecuteNonQuery();
                    ShowResult(sqlExpression, item.ToString());
                }
                catch (Exception)
                {
                    Console.WriteLine("Error on updating Item, check your model. \n");
                }
            }
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

            ShowResult(sqlExpression, $"Deleted item with id = {id}");
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public void ShowResult(string sql,string res)
        {
            Console.WriteLine(sql);
            Console.WriteLine();
            Console.WriteLine(res);
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
