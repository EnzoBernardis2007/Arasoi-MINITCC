using Arasoi_MINITCC.Tabs.Athlete;
using Arasoi_MINITCC.Tabs.Tournament;
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
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddTournament addTournament = new AddTournament();
            addTournament.Show();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyTabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Campeonatos":
                        this.DataContext = new TournamentViewModel(); 
                        break;
                    case "Atletas":
                        this.DataContext = new AthleteViewModel();
                        break;
                }
            }
        }
    }
}
