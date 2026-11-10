using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EdzesKovetoWpf
{
    public class GyakorlatWpfModell : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int FelhasznaloId { get; set; }
        
        private string nev = string.Empty;
        public string Nev
        {
            get => nev;
            set { nev = value; OnPropertyChanged(); }
        }

        public string Kategoria { get; set; } = "Altalanos";

        public bool Hetfo { get; set; }
        public bool Kedd { get; set; }
        public bool Szerda { get; set; }
        public bool Csutortok { get; set; }
        public bool Pentek { get; set; }
        public bool Szombat { get; set; }
        public bool Vasarnap { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FelhasznaloDto
    {
        public int Id { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class StatisztikaDto
    {
        public int FelhasznaloId { get; set; }
        public int OsszesFeladat { get; set; }
        public int Elvegezve { get; set; }
        public double TeljesitmenySzazalek { get; set; }
    }
}
