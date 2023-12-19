//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using Azure.Core;
//using ClientServerLinkingPart;

//namespace ClientWPF
//{
//    public class RequestController
//    {
//        #region ClientServerInteraction
//        private readonly TcpClient tcpClient;
//        private readonly NetworkStream stream;

//        public RequestController(string ip, int port)
//        {
//            tcpClient = new TcpClient();
//            tcpClient.Connect(ip, port);
//            stream = tcpClient.GetStream();
//        }
//        ~RequestController()
//        {
//            stream.Dispose();
//            tcpClient.Close();
//        }

//        public Connection SendRequest(Connection connection)
//        {
//            List<byte> data = new List<byte>();
//            stream.Write(Encoding.Unicode.GetBytes(connection.GetJson()));
//            do
//            {
//                data.Add((byte)stream.ReadByte());
//            }
//            while (stream.DataAvailable);
//            string json = Encoding.Unicode.GetString(data.ToArray());
//            return Connection.GetRequest(json);
//        }
//        #endregion

//        public bool SetAndCheckPath(string path)
//        {
//            Connection con = SendRequest(new Connection("SetAndCheckPath", path));
//            return (con.Content[0] == "True");
//        }
//        public void AddRecord(string newRecord)
//        {
//            SendRequest(new Connection("AddRecord", newRecord));
//        }

//        public bool DeleteRecord(int number)
//        {
//            Connection con = SendRequest(new Connection("DeleteRecord", number.ToString()));
//            return (con.Content[0] == "True");
//        }

//        public bool SetModelType(string modeltype)
//        {
//            Connection answer = SendRequest(new Connection("SetModelType", modeltype));
//            return (answer.Content[0] == "True");
//        }
//        public void SaveNewData(List<string> cinemaList, int pos)
//        {
//            cinemaList.Add(pos.ToString());
//            SendRequest(new Connection("SaveNewData", cinemaList));
//        }

//        //public ObservableCollection<Cinema> GetAllRecords()
//        //{
//        //    Connection con = SendRequest(new Connection("GetAllRecords", ""));
//        //    return con.Content;
//        //}

//        public List<string> GetSepRecord(int number)
//        {
//            Connection con = SendRequest(new Connection("GetSepRecord", number.ToString()));
//            return con.Content;
//        }

//        public void ShutDown()
//        {
//            SendRequest(new Connection("ShutDown", ""));
//        }
//    }
//}
