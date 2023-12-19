using ClientWPFApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using ClientServerLinkingPart;
using System.Security.Policy;
using System.Net.Sockets;

namespace ClientWPF
{
    class ClientWPFController : INotifyPropertyChanged
    {
        #region определение PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region ClientServerInteraction
        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;

        private CinemaCon cinema;
        public ClientWPFController(string ip, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
            stream = tcpClient.GetStream();

            selectedCinemaindex = -1;
            selectedCinema = new();
            allRecords = new();
            cinema = new();

            name = new("");
            address = new("");
            halls = new("");
            capacity = new("");
            has3d = false;
        }
        ~ClientWPFController()
        {
            stream.Dispose();
            tcpClient.Close();
        }

        public Connection SendRequest(Connection connection)
        {
            List<byte> data = new List<byte>();
            stream.Write(Encoding.Unicode.GetBytes(connection.GetJson()));
            do
            {
                data.Add((byte)stream.ReadByte());
            }
            while (stream.DataAvailable);
            string json = Encoding.Unicode.GetString(data.ToArray());
            return Connection.GetRequest(json);
        }
        #endregion

        #region CsvFileHandler
        public bool SetAndCheckPath(string path)
        {
            Connection con = SendRequest(new Connection("SetAndCheckPath", path));
            return (con.Content[0] == "True");
        }
        public void AddRecord(string newRecord)
        {
            SendRequest(new Connection("AddRecord", newRecord));
        }

        public bool DeleteRecord(int number)
        {
            Connection con = SendRequest(new Connection("DeleteRecord", number.ToString()));
            return (con.Content[0] == "True");
        }

        public List<string> GetAllRecords()
        {
            Connection con = SendRequest(new Connection("GetAllRecords", ""));
            return con.Content;
        }

        public List<string> GetSepRecord(int number)
        {
            Connection con = SendRequest(new Connection("GetSepRecord", number.ToString()));
            return con.Content;
        }

        public void ShutDown()
        {
            SendRequest(new Connection("ShutDown", ""));
        }
        #endregion

        private int selectedCinemaindex;

        public int SelectedCinemaIndex
        {
            get
            {
                return selectedCinemaindex;
            }
            set
            {
                selectedCinemaindex = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CinemaCon> allRecords;
        public ObservableCollection<CinemaCon> AllRecords
        {
            get
            {
                return allRecords;
            }
            set
            {
                allRecords = value;
                OnPropertyChanged();
            }
        }

        private Command getAllRecordsDB;
        public Command GetAllRecordsDB
        {
            get
            {
                return getAllRecordsDB ??= new Command(obj =>
                {
                    AllRecords.Clear();
                    Connection con = SendRequest(new Connection("DBGetAllRecords", ""));
                    List<string> allrec = con.Content;
                    for (int i = 0; i < allrec.Count; i++)
                    {
                        List<string> cinem = allrec[i].Split("|").Select(x => x.Trim()).ToList();
                        AllRecords.Add(cinema.ReturnCinema(cinem));
                    }
                });
            }
        }
        private Command deleteRecord;
        public Command DeleteRecordDB
        {
            get
            {
                return deleteRecord ??= new Command(obj =>
                {
                    if (SelectedCinemaIndex != -1)
                    {
                        Connection con = SendRequest(new Connection("DBDeleteRecord", SelectedCinemaIndex.ToString()));
                        GetAllRecordsDB.Execute(null);
                    }
                });
            }
        }
        #region Данные для добавления кинотеатра
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private string address;

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }
        private string halls;

        public string Halls
        {
            get
            {
                return halls;
            }
            set
            {
                halls = value;
                OnPropertyChanged();
            }
        }
        private string capacity;

        public string Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                capacity = value;
                OnPropertyChanged();
            }
        }
  
        private bool has3d;

        public int Has3d
        {
            get
            {
                return Convert.ToInt32(has3d);
            }
            set
            {
                has3d = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }
        #endregion

        private Command addRecord;
        public Command AddRecordDB
        {
            get
            {
                return addRecord ??= new Command(obj =>
                {
                    try
                    {
                        string newRecord =
                          Name + ";"
                        + Address + ";"
                        + Halls.ToString() + ";"
                        + Capacity.ToString() + ";"
                        + has3d.ToString();
                        SendRequest(new Connection("DBAddRecord", newRecord));
                        GetAllRecordsDB.Execute(null);
                    }
                    catch
                    {
                        MessageBox.Show("Правила добавления записи (пример):\n" +
                            "Гудвин\n" +
                            "Вершинина 46\n" +
                            "6\n" +
                            "350\n" +
                            "(Выбрать наличие 3Д)");
                    }
                });
            }
        }

        private CinemaCon selectedCinema;

        public CinemaCon SelectedCinema
        {
            get
            {
                return selectedCinema;
            }
            set
            {
                selectedCinema = value;
                OnPropertyChanged();
            }
        }

        private bool selectedHas3d;

        public int SelectedHas3d
        {
            get
            {
                return Convert.ToInt32(selectedHas3d);
            }
            set
            {
                selectedHas3d = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        private Command editRecord;
        public Command EditRecordDB
        {
            get
            {
                return editRecord ??= new Command(obj =>
                {
                    if (SelectedCinema is not null)
                    {
                        List<string> cinemadata = new()
                        {
                            SelectedCinemaIndex.ToString(),
                            SelectedCinema.Name,
                            SelectedCinema.Address,
                            SelectedCinema.Halls.ToString(),
                            SelectedCinema.Capacity.ToString(),
                            SelectedCinema.Has3d.ToString()
                        };
                        Connection con = SendRequest(new Connection("DBEditRecord", cinemadata));
                        GetAllRecordsDB.Execute(null);
                    }
                    else MessageBox.Show("Сначала выберите кинотеатр");
                });
            }
        }
    }

}