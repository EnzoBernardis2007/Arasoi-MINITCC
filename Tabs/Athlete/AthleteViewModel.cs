using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasoi_MINITCC.Tabs.Athlete
{
    internal class AthleteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class AthleteCard
    {
        public string CPF {  get; set; }
        public string Name { get; set; } 
    }
}
