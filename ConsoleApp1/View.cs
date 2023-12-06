using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1;
using NLog;
using ServerClientLinkingPart;

namespace lab1
{
    class View
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //static void SetPath(Controller controller)
        //{
        //    Console.WriteLine("Введите путь к файлу:\n");
        //    string path = Console.ReadLine();
        //    if (!controller.SetAndCheckPath(path))
        //    {
        //        Console.WriteLine("Введите корректный путь к файлу!");
        //        SetPath(controller);
        //    }
        //}

        static void doClientRequest(ref Connection con)
        {
            switch (con.Act)
            {
                case "SetAndCheckPath":
                    string isSuccess = Controller.GetInstance().SetAndCheckPath(con.Content[0]).ToString();
                    con.Content = new List<string> { isSuccess };
                    break;
                case "AddRecord":
                    Controller.GetInstance().AddRecord(con.Content[0]);
                    logger.Info("Client succesfully added the record");
                    break;
                case "DeleteRecord":
                    try
                    {
                        Controller.GetInstance().DeleteRecord(int.Parse(con.Content[0]));
                        con.Content = new List<string> { "True" };
                        logger.Info("Client succesfully deleted the record");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        con.Content = new List<string> { "False" };
                    }
                    break;
                case "GetAllRecords":
                    List<string> list = Controller.GetInstance().GetAllRecords();
                    con.Content = list;
                    logger.Info("Client received all records");
                    break;
                case "GetSepRecord":
                    try
                    {
                        List<string> line = Controller.GetInstance().GetSepRecord(int.Parse(con.Content[0]));
                        con.Content = line;
                        logger.Info("Client received separate record");
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        con.Content = new List<string> { "Строка не найдена!\n" };
                    }
                    break;
                case "Shutdown":
                    logger.Info("Client disconnected");
                    break;
            }
        }
        public static async Task AnswerRequestAsync(NetworkStream stream)
        {
            try
            {
                while (true)
                {
                    List<byte> data = new List<byte>();
                    while (!stream.DataAvailable) ;//
                    while (stream.DataAvailable)
                    {
                        data.Add((byte)stream.ReadByte());
                    }
                    string json = Encoding.Unicode.GetString(data.ToArray());
                    Connection con = Connection.GetRequest(json);
                    doClientRequest(ref con);
                    string responseJson = con.GetJson();
                    await stream.WriteAsync(Encoding.Unicode.GetBytes(responseJson));
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }
        }

        static async Task Main(string[] args)
        {
            TcpListener server = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 8080);
            try
            {
                server.Start();
                Console.WriteLine("Server is running");
                while (true)
                {
                    using var tcpClient = await server.AcceptTcpClientAsync();
                    Console.WriteLine("A connection with the client has been established");
                    logger.Info("A connection with the client has been established");
                    NetworkStream stream = tcpClient.GetStream();
                    await AnswerRequestAsync(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Info(ex.Message);
            }
            finally
            {
                server.Stop();
            }
        }
    }
}