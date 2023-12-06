using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientLab2
{
    internal class ClientView
    {

        static void MainMenu()
        {
            Console.WriteLine("\nВыберите действие с файлом\n" +
                "1. Выбрать файл для чтения и записи\n" +
                "2. Вывод всех записей\n" +
                "3. Вывод записи по номеру\n" +
                "4. Удаление записи из файла\n" +
                "5. Добавление записи в файл\n" +
                "ESC. Выйти из программы\n");
        }
        static void SetPath(ClientController controller)
        {
            //Console.WriteLine("Введите путь к файлу:\n");
            string path = "D:\\учеба 5 семестр\\ais\\ConsoleApp1\\Cinema.csv";//Console.ReadLine();
            if (!controller.SetAndCheckPath(path))
            {
                Console.WriteLine("Введите корректный путь к файлу!");
                SetPath(controller);
            }
        }

        static void doActionMainMenu(ClientController controller)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                    SetPath(controller);
                    break;
                case ConsoleKey.D2:

                    controller.GetAllRecords().ForEach(Console.WriteLine);
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("Введите номер искомой записи: ");
                    try
                    {
                        int a = int.Parse(Console.ReadLine());
                        controller.GetSepRecord(a).ForEach(Console.WriteLine);
                    }
                    catch
                    {
                        Console.WriteLine("Запись под этим номером не найдена или введена неверно!");
                    }
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("Введите номер записи, которую нужно удалить: ");
                    try
                    {
                        int a = int.Parse(Console.ReadLine());
                        controller.DeleteRecord(a);
                    }
                    catch
                    {
                        Console.WriteLine("Запись под этим номером не найдена или введена неверно!");
                    }
                    break;
                case ConsoleKey.D5:
                    string str = "\n";
                    Console.WriteLine("Введите название кинотеатра:\n");
                    str = Console.ReadLine() + ";";

                    Console.WriteLine("Введите адрес кинотеатра:\n");
                    str += Console.ReadLine() + ";";

                    Console.WriteLine("Введите количество залов кинотеатра (число):\n");
                    string b = Console.ReadLine();
                    try
                    {
                        int.Parse(b);

                        str += b + ";";
                    }
                    catch
                    {
                        Console.WriteLine("Неверно введены данные. Введите число");
                        break;
                    }

                    Console.WriteLine("Введите вместимость кинотеатра (число):\n");
                    b = Console.ReadLine();
                    try
                    {
                        int.Parse(b);
                        str += b + ";";
                    }
                    catch
                    {
                        Console.WriteLine("Неверно введены данные. Введите число");
                        break;
                    }

                    Console.WriteLine("Введите, поддерживает ли кинотеатр 3D-показ (True/False):\n");
                    b = Console.ReadLine();
                    try
                    {
                        bool.Parse(b);
                        str += b;
                    }
                    catch
                    {
                        Console.WriteLine("Неверно введены данные. Введите True/False");
                        break;
                    }
                    controller.AddRecord(str);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }

        static void Main(string[] args)
        {
            ClientController controller = new("127.0.0.1", 8080);
            SetPath(controller);
            while (true)
            {
                MainMenu();
                doActionMainMenu(controller);
            }
        }
    }
}