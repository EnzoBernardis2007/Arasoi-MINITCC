using Arasoi_MINITCC.DatabaseManagement;
using Arasoi_MINITCC.Prefab;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CpfLibrary;

namespace Arasoi_MINITCC.Tabs.Athlete
{
    internal class AthleteViewModel : INotifyPropertyChanged
    {
        // this class takes care of viewing the tournaments tab in the MainWindow
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<AthleteCard> _athleteCards;
        public ObservableCollection<AthleteCard> AthleteCards
        {
            get { return _athleteCards; }
            set
            {
                if (_athleteCards != value)
                {
                    _athleteCards = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AthleteCards)));
                }
            }
        }

        public AthleteViewModel()
        {
            LoadView();
        }

        // Get data from the database and add that data to a collection of 'TournamentCard'
        public void LoadView()
        {
            AthleteCards = new ObservableCollection<AthleteCard>();

            using (MySqlConnection connection = ConnectionFactory.GetConnection())
            {
                try
                {
                    string command = "SELECT * FROM atleta";

                    using (MySqlCommand commandSELECT = new MySqlCommand(command, connection))
                    {
                        using (MySqlDataReader reader = commandSELECT.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string cpf = Cpf.Format(reader["CPF"].ToString());
                                string name = reader["nome_atleta"].ToString();
                                double height = Convert.ToDouble(reader["altura"]);
                                int age = Convert.ToInt32(reader["idade"]);
                                int kyu = Convert.ToInt32(reader["kyu"]);
                                int dan = Convert.ToInt32(reader["dan"]);
                                string dojo = reader["dojo"].ToString();
                                string city = reader["cidade"].ToString();
                                int IdTournament = Convert.ToInt32(reader["cod_campeonato"]);

                                AthleteCards.Add(new AthleteCard(cpf, name, height, age, kyu, dan, dojo, city, IdTournament));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar os atletas: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class AthleteCard
    {
        // this class serves to serve as a base of information for the WPF, in the tournaments tab
        public string CPF { get; set; }
        public string AthleteName { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public int Kyu { get; set; }
        public int Dan { get; set; }
        public string Dojo { get; set; }
        public string City { get; set; }
        public int IdTournament { get; set; }

        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public AthleteCard(string cpf, string athleteName, double height, int age, int kyu, int dan, string dojo, string city, int idTournament)
        {
            CPF = cpf;
            AthleteName = athleteName;
            Height = height;
            Age = age;
            Kyu = kyu;
            Dan = dan;
            Dojo = dojo;
            City = city;
            IdTournament = idTournament;

            DeleteCommand = new RelayCommand(Delete);
            EditCommand = new RelayCommand(Edit);
        }

        // deletes a record in the database that has the same Id
        public void Delete()
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Tem certeza que quer deletar?", "Confirmação", MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.No) return;

                using (MySqlConnection connection = ConnectionFactory.GetConnection())
                {
                    string command = "DELETE FROM atleta WHERE CPF = @CPF";
                    MySqlCommand commandDELETE = new MySqlCommand(command, connection);

                    commandDELETE.Parameters.AddWithValue("@CPF", CPF);

                    commandDELETE.ExecuteNonQuery();
                }
                var viewModel = (AthleteViewModel)Application.Current.MainWindow.DataContext;
                viewModel.LoadView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao deletar um atleta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Opens a window to edit the information for the respective record
        public void Edit()
        {
            EditAthlete editAthlete = new EditAthlete(CPF, AthleteName, Height, Age, Kyu, Dan, Dojo, City);
            editAthlete.Show();
        }
    }
}