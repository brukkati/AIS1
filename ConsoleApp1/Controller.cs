using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Controller
    {
        public string Path { get; set; }

        public bool SetAndCheckPath(string path) // Проверка пути и расширения
        {
            if (File.Exists(path) && System.IO.Path.GetExtension(path) == ".csv")
            {
                Path = path;
                return true;
            }
            else return false;
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
            var data = LoadAll(Path);
            var outputRecords = new List<string>();
            int num = 1;
            foreach (var cinema in data)
            {
                outputRecords.Add(GetString(cinema.GetStringList(), num));
                num++;
            }
            return outputRecords;
        }

        public List<string> GetSepRecord(int number)
        {
            var data = SeparatelyLoad(Path, number - 1);
            List<string> outputRecord = new();
            outputRecord.Add(GetString(data.GetStringList(), number));
            return outputRecord;
        }

        public void DeleteRecord(int number)
        {
            CinemaDelete(Path, number);
        }

        public void AddRecord(string str)
        {
            var record = ConvertTextToCinema(str);
            CinemaAdd(Path, record);
        }

        #region Обработка данных 

        string delim = ";";

       
        private string ConvertCinemaToText(Cinema cinema)
        {
            string str = cinema.Name + delim + cinema.Address + delim
                + cinema.Halls.ToString() + delim + cinema.Capacity.ToString()
                + delim + cinema.Has3D.ToString();
            return str;
        }
 
        private Cinema ConvertTextToCinema(string txtLine)
        {
            string[] line = txtLine.Split(delim);
            return new Cinema(
                line[0], line[1], int.Parse(line[2]), int.Parse(line[3]), bool.Parse(line[4]));
        }


        private List<Cinema> LoadAll(string path)
        {
            List<Cinema> allCinema = new();
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                allCinema.Add(ConvertTextToCinema(line));
            }
            return allCinema;
        }

        private Cinema SeparatelyLoad(string path, int number)
        {
            string[] lines = File.ReadAllLines(path);
            return ConvertTextToCinema(lines[number]);
        }

        private void CinemaDelete(string path, int recordNumber)
        {
            File.WriteAllLines(path,
                File.ReadLines(path).Where((line, index) => index != recordNumber - 1).ToList());
        }

        private void CinemaAdd(string path, Cinema cinema)
        {
            File.AppendAllText(path, ConvertCinemaToText(cinema));
        }
        #endregion
    }
}

