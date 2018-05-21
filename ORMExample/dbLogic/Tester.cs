using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMExample
{
    public class Tester
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static void Test()
        {
            string sqlExpression = "SELECT * FROM [dbo].[User]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    DataTable dataTable = reader.GetSchemaTable();
                    while (reader.Read())
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            Console.WriteLine(dataTable.Rows[i].ItemArray[9] +"="+ reader.GetValue(i));
                        }
                    }                    
                }
                reader.Close();
            }
            Console.ReadKey();

        }

        public static void Test2()
        {
            string sqlExpression = "SELECT * FROM [dbo].[User]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[0].Rows[j].ItemArray.Length; j++)
                    {
                        Console.WriteLine(ds.Tables[0].Rows[i].ItemArray[j]);
                    }
                }
            }
            Console.ReadKey();

        }
    }
}
