using Arasoi_MINITCC.Prefab;
using Arasoi_MINITCC.Tabs.Athlete;
using Arasoi_MINITCC.Tabs.Tournament;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arasoi_MINITCC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string filePath = "C:\\Users\\berna\\source\\repos\\Arasoi-MINITCC\\JSON\\Credentials.json";
            Credential credentials = JSONmanagement.LoadCredentials(filePath);

            if (credentials.Entered == false)
            {
                LockScreen lockScreen = new LockScreen(this);
                lockScreen.Show();
                this.Hide();
            }
        }

        private void AddAthlete_Click(object sender, RoutedEventArgs e)
        {
            AddAthlete addAthlete = new AddAthlete();
            addAthlete.Show();
        }

        private void AddTournament_Click(object sender, RoutedEventArgs e)
        {
            AddTournament addTournament = new AddTournament();
            addTournament.Show();
        }

        private TournamentViewModel tournamentViewModel = new TournamentViewModel();
        private AthleteViewModel athleteViewModel = new AthleteViewModel();

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyTabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Name)
                {
                    case "TournamentTab":
                        this.DataContext = tournamentViewModel;
                        break;
                    case "AthleteTab":
                        this.DataContext = athleteViewModel;
                        break;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string filePath = "C:\\Users\\berna\\source\\repos\\Arasoi-MINITCC\\JSON\\Credentials.json";
            string jsonContent = File.ReadAllText(filePath);
            JObject jsonObject = JObject.Parse(jsonContent);
            jsonObject["Entered"] = false;
            string updatedJsonContent = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            File.WriteAllText(filePath, updatedJsonContent);
        }
    }
}
