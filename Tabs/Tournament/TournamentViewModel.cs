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

namespace Arasoi_MINITCC.Tabs.Tournament
{
    internal class TournamentViewModel : INotifyPropertyChanged
    {
        // this class takes care of viewing the tournaments tab in the MainWindow

        private ObservableCollection<TournamentCard> _tournamentCards;
        public ObservableCollection<TournamentCard> TournamentCards
        {
            get { return _tournamentCards; }
            set
            {
                if (_tournamentCards != value)
                {
                    _tournamentCards = value;
                    OnPropertyChanged();
                }
            }
        }

        public TournamentViewModel()
        {
            LoadView();
        }

        // Get data from the database and add that data to a collection of 'TournamentCard'
        public void LoadView()
        {
            TournamentCards = new ObservableCollection<TournamentCard>();

            using (MySqlConnection connection = ConnectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    string command = "SELECT * FROM campeonato";
                    
                    using (MySqlCommand commandSELECT = new MySqlCommand(command, connection))
                    {
                        using (MySqlDataReader reader = commandSELECT.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string id = reader["cod_campeonato"].ToString();
                                string name = reader["nome_campeonato"].ToString();
                                string filiation = reader["filiacao"].ToString();
                                DateTime dateStart = Convert.ToDateTime(reader["data_inicio"]);
                                DateTime dateEnd = Convert.ToDateTime(reader["data_fim"]);

                                TournamentCards.Add(new TournamentCard(id, name, filiation, dateStart, dateEnd));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar os campeonatos: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class TournamentCard
    {
        // this class serves to serve as a base of information for the WPF, in the tournaments tab
        public string Id { get; set; }
        public string Name { get; set; }
        public string Filiation { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public TournamentCard(string id, string name, string filiation, DateTime dateStart, DateTime dateEnd)
        {
            Id = id;
            Name = name;
            Filiation = filiation;
            DateStart = dateStart;
            DateEnd = dateEnd;

            DeleteCommand = new RelayCommand(Delete);
            EditCommand = new RelayCommand(Edit);
        }

        // deletes a record in the database that has the same Id
        public void Delete()
        {
            using (MySqlConnection connection = ConnectionFactory.GetConnection()) 
            {
                connection.Open();
                string command = "DELETE FROM campeonato WHERE cod_campeonato = @cod_campeonato";
                MySqlCommand commandDELETE = new MySqlCommand(command, connection);

                commandDELETE.Parameters.AddWithValue("@cod_campeonato", Id);

                commandDELETE.ExecuteNonQuery();
            }
        }

        // Opens a window to edit the information for the respective record
        public void Edit()
        {
            EditTournament editTournament = new EditTournament(Id, Name, Filiation, DateStart, DateEnd);
            editTournament.Show();
        }
    }
}