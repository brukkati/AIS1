using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

namespace Arch_Lab5
{
    public class myFriends
    {
        public int[] response { get; set; }
    }
    public class Rootobject
    {
        public Response response { get; set; }
    }

    public class Response
    {
        public int id { get; set; }
        //public string home_town { get; set; }
        public string status { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string bdate { get; set; }
        //public City city { get; set; }
        //public Country country { get; set; }
        public string phone { get; set; }
        public int sex { get; set; }
    }

    //public class City
    //{
    //    public int id { get; set; }
    //    public string title { get; set; }
    //}

    //public class Country
    //{
    //    public int id { get; set; }
    //    public string title { get; set; }
    //}

    public partial class UserSettingsWindow : Window
    {
        private MainWindow mw;
        private string f;

        public UserSettingsWindow(MainWindow mainwin)
        {
            InitializeComponent();
            mw = mainwin;
            f = "";
        }

        private void Button_Click_Data(object sender, RoutedEventArgs e)
        {
            string reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=5.154";
            string method = "account.getProfileInfo";
            
            f = mw.GET(reqStrTemplate, method, mw.Access_token);
            var user = JsonSerializer.Deserialize<Rootobject>(f).response;
            string[] list =
            {
                "id: " + user.id.ToString(),
                "status: " + user.status,
                "lastname: " + user.last_name,
                "firstname: " + user.first_name,
                "birth date: " + user.bdate,
                //"city: " + user.city.title,
                "phone number: " + user.phone,
                "sex: " + user.sex.ToString()
                //"country: " + user.country.title
            };
            UserInformationTextBox.Text = string.Join("\n", list);
        }

        private void Button_Click_Friends(object sender, RoutedEventArgs e)
        {
            string reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=5.154&user_id="+mw.UserID;
            string method = "users.getFollowers";
            //string method = "account.getAppPermissions";
            f = mw.GET(reqStrTemplate, method, mw.Access_token);
            var ArrayOfFriends = JsonSerializer.Deserialize<myFriends>(f).response;

            reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=5.154&user_ids=" + string.Join(",",ArrayOfFriends.Select(x => x.ToString()));
            method = "users.get";
            f = mw.GET(reqStrTemplate, method, mw.Access_token);
            var friends = JsonDocument.Parse(f).RootElement.GetProperty("response");
            var Users = JsonSerializer.Deserialize<User[]>(friends);
            UserInformationTextBox.Text = string.Join("\n", Users.Select(x=> x.last_name + " " + x.first_name));
        }
    }
    public class User
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}
