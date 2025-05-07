using MySql.Data.MySqlClient;
using System;
using MySql.Data.MySqlClient;
using ConsoleApp10;
using System.Data.Common;

using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Getting Connection ...");
        MySqlConnection conn = DBUtils.GetDBConnection();

        try
        {
            Console.WriteLine("Opening Connection ...");
            conn.Open();
            Console.WriteLine("Connection successful!");

            QuerySubscribers(conn); // викликаємо наш запит
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            Console.WriteLine(e.StackTrace);
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }

        Console.Read();
    }

    private static void QuerySubscribers(MySqlConnection conn)
    {
        string sql = @"SELECT phone_number,last_name, first_name, middle_name, Tariffs_tariff_code, 
                              unpaid_local, unpaid_long_distance, unpaid_international 
                       FROM subscribers";

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;

        using (DbDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                Console.WriteLine("List of subscribers:\n");
                while (reader.Read())
                {
                    string phone = reader["phone_number"].ToString();
                    string last = reader["last_name"].ToString();
                    string first = reader["first_name"].ToString();
                    string middle = reader["middle_name"].ToString();
                    string tariff = reader["Tariffs_tariff_code"].ToString();
                    string local = reader["unpaid_local"].ToString();
                    string longDist = reader["unpaid_long_distance"].ToString();
                    string intl = reader["unpaid_international"].ToString();

                    Console.WriteLine($"Phone: {phone}, Name: {last} {first} {middle}, Tariff: {tariff}");
                    Console.WriteLine($"Unpaid Minutes - Local: {local}, Long-distance: {longDist}, International: {intl}");
                    Console.WriteLine("----------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("No subscribers found.");
            }
        }
    }
}
