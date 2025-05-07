using MySql.Data.MySqlClient;
using System;
using MySql.Data.MySqlClient;
using ConsoleApp10;
using System.Data.Common;

using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        MySqlConnection conn = DBUtils.GetDBConnection();

        try
        {
            conn.Open();
            Console.WriteLine("З'єднання встановлено успішно!\n");

            while (true)
            {
                Console.WriteLine("=== Меню ===");
                Console.WriteLine("1. Переглянути абонентів");
                Console.WriteLine("2. Додати абонента");
                Console.WriteLine("3. Оновити абонента");
                Console.WriteLine("4. Видалити абонента");
                Console.WriteLine("5. Вийти");
                Console.Write("Оберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        QuerySubscribers(conn);
                        break;
                    case "2":
                        AddSubscriber(conn);
                        break;
                    case "3":
                        UpdateSubscriber(conn);
                        break;
                    case "4":
                        DeleteSubscriber(conn);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Невірна опція.");
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Помилка: " + e.Message);
        }
        finally
        {
            conn.Close();
        }
    }
   

    private static void QuerySubscribers(MySqlConnection conn)
    {
        string sql = @"SELECT phone_number, last_name, first_name, middle_name, Tariffs_tariff_code, 
                              unpaid_local, unpaid_long_distance, unpaid_international 
                       FROM subscribers";

        MySqlCommand cmd = new MySqlCommand(sql, conn);

        using (DbDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                Console.WriteLine("\nСписок абонентів:\n");
                while (reader.Read())
                {
                    Console.WriteLine($"Телефон: {reader["phone_number"]}, Ім’я: {reader["last_name"]} {reader["first_name"]} {reader["middle_name"]}");
                    Console.WriteLine($"Тариф: {reader["Tariffs_tariff_code"]}, Місцеві: {reader["unpaid_local"]}, Міжміські: {reader["unpaid_long_distance"]}, Міжнар.: {reader["unpaid_international"]}");
                    Console.WriteLine("----------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("Абоненти не знайдені.");
            }
        }
    }
    private static void ShowTariffs(MySqlConnection conn)
    {
        string sql = "SELECT tariff_code, privilege_category FROM tariffs";

        MySqlCommand cmd = new MySqlCommand(sql, conn);

        using (DbDataReader reader = cmd.ExecuteReader())
        {
            Console.WriteLine("\n=== Доступні тарифи ===");
            while (reader.Read())
            {
                Console.WriteLine($"Код: {reader["tariff_code"]} | Назва: {reader["privilege_category"]}");
            }
            Console.WriteLine("=======================\n");

        }
    }

    private static void AddSubscriber(MySqlConnection conn)
    {
        Console.WriteLine("=== Додавання абонента ===");
        Console.Write("Телефон: ");
        string phone = Console.ReadLine();
        Console.Write("Прізвище: ");
        string last = Console.ReadLine();
        Console.Write("Ім’я: ");
        string first = Console.ReadLine();
        Console.Write("По батькові: ");
        string middle = Console.ReadLine();
        ShowTariffs(conn);
        Console.Write("Оберіть код тарифу зі списку: ");
        string tariff = Console.ReadLine();
        double unpaid_local = GetDoubleInput("Борг за місцеві дзвінки: ");
        double unpaid_long = GetDoubleInput("Борг за міжміські дзвінки: ");
        double unpaid_Intl = GetDoubleInput("Борг за міжнародні дзвінки: ");

        string sql = @"INSERT INTO subscribers (phone_number, last_name, first_name, middle_name, Tariffs_tariff_code, unpaid_local, unpaid_long_distance, unpaid_international)
                       VALUES (@phone, @last, @first, @middle, @tariff, @local, @long, @intl)";

        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@phone", phone);
        cmd.Parameters.AddWithValue("@last", last);
        cmd.Parameters.AddWithValue("@first", first);
        cmd.Parameters.AddWithValue("@middle", middle);
        cmd.Parameters.AddWithValue("@tariff", tariff);
        cmd.Parameters.AddWithValue("@local", unpaid_local);
        cmd.Parameters.AddWithValue("@long", unpaid_long);
        cmd.Parameters.AddWithValue("@intl", unpaid_Intl);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Абонент доданий!" : "Не вдалося додати абонента.");
    }

    private static void UpdateSubscriber(MySqlConnection conn)
    {
        Console.WriteLine("=== Оновлення абонента ===");
        Console.Write("Введіть номер телефону абонента для оновлення: ");
        string phone = Console.ReadLine();

        Console.Write("Нове прізвище: ");
        string last = Console.ReadLine();
        Console.Write("Нове ім’я: ");
        string first = Console.ReadLine();
        Console.Write("Нове по батькові: ");
        string middle = Console.ReadLine();
        ShowTariffs(conn);
        Console.Write("Новий код тарифу: ");
        string tariff = Console.ReadLine();

        double unpaidLocal = GetDoubleInput("Новий борг за місцеві дзвінки: ");
        double unpaidLong = GetDoubleInput("Новий борг за міжміські дзвінки: ");
        double unpaidIntl = GetDoubleInput("Новий борг за міжнародні дзвінки: ");

        string sql = @"UPDATE subscribers 
                       SET last_name = @last, first_name = @first, middle_name = @middle,
                           Tariffs_tariff_code = @tariff,
                           unpaid_local = @local,
                           unpaid_long_distance = @long,
                           unpaid_international = @intl
                       WHERE phone_number = @phone";

        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@last", last);
        cmd.Parameters.AddWithValue("@first", first);
        cmd.Parameters.AddWithValue("@middle", middle);
        cmd.Parameters.AddWithValue("@tariff", tariff);
        cmd.Parameters.AddWithValue("@local", unpaidLocal);
        cmd.Parameters.AddWithValue("@long", unpaidLong);
        cmd.Parameters.AddWithValue("@intl", unpaidIntl);
        cmd.Parameters.AddWithValue("@phone", phone);


        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Абонент оновлений!" : "Абонента не знайдено.");
    }

    private static void DeleteSubscriber(MySqlConnection conn)
    {
        Console.WriteLine("=== Видалення абонента ===");
        Console.Write("Введіть номер телефону абонента для видалення: ");
        string phone = Console.ReadLine();

        string sql = "DELETE FROM subscribers WHERE phone_number = @phone";

        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@phone", phone);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Абонент видалений!" : "Абонента не знайдено.");
    }
    private static double GetDoubleInput(string message)
    {
        double result;
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            if (double.TryParse(input, out result) && result >= 0)
            {
                return result;
            }
            Console.WriteLine(" Неправильний ввід. Введіть додатне число (наприклад: 0, 12.5):");
        }
    }
}
