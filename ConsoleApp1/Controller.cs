using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Controller
    {

        private static Controller? instance;
        public static Controller GetInstance()
        {
            instance ??= new Controller();
            return instance;
        }

        public string Path { get; set; }

        public bool SetAndCheckPath(string path) // Проверка пути и расширения
        {
            if (File.Exists(path) && System.IO.Path.GetExtension(path) == ".csv")
            {
                Path = path;
                return true;
            }
            return false;
        }

        public string GetString(List<string> records, int position) // Вывод записи на экран
        {
            string result = position.ToString();
            foreach (string rec in records)
                result += string.Format("{0,15}", rec);
            return result;
        }

        public List<string> GetAllRecords() // вывод всех записей на экран
        {
            List<Cinema> allCinema = new();
            string[] lines = File.ReadAllLines(Path);
            foreach (string line in lines)
            {
                allCinema.Add(ConvertTextToCinema(line));
            }
            var outputRecords = new List<string>();
            int num = 1;
            foreach (var cinema in allCinema)
            {
                outputRecords.Add(GetString(cinema.GetStringList(), num));
                num++;
            }
            return outputRecords;
        }

        public List<string> GetSepRecord(int number)
        {
            string[] lines = File.ReadAllLines(Path);
            var data = ConvertTextToCinema(lines[number-1]);
            List<string> outputRecord = new();
            outputRecord.Add(GetString(data.GetStringList(), number));
            return outputRecord;
        }

        public void DeleteRecord(int number)
        {
            File.WriteAllLines(Path,
                File.ReadLines(Path).
                Where((line, index) => index != number - 1).ToList());
        }

        public void AddRecord(string str)
        {
            //var record = ConvertTextToCinema(str);
            File.AppendAllText(Path, str);//ConvertCinemaToText(record));
        }

        #region Обработка данных 

        string delim = ";";
        //private string ConvertCinemaToText(Cinema cinema)
        //{
        //    string str = cinema.Name + delim + cinema.Address + delim
        //        + cinema.Halls.ToString() + delim + cinema.Capacity.ToString()
        //        + delim + cinema.Has3D.ToString();
        //    return str;
        //}
 
        private Cinema ConvertTextToCinema(string txtLine)
        {
            string[] line = txtLine.Split(delim);
            return new Cinema(
                line[0], line[1], int.Parse(line[2]), int.Parse(line[3]), bool.Parse(line[4]));
        }
        #endregion
    }
}

