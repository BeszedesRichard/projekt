using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace EdzesKovetoWpf
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:51991/") };
        private readonly int _felhasznaloId;
        public ObservableCollection<GyakorlatWpfModell> FeladatokLista { get; set; } = new ObservableCollection<GyakorlatWpfModell>();

        public MainWindow(int felhasznaloId, string felhasznaloNev)
        {
            InitializeComponent();
            _felhasznaloId = felhasznaloId;
            StatuszText.Text = $"Bejelentkezve mint: {felhasznaloNev} (ID: {felhasznaloId})";
            EdzesDataGrid.ItemsSource = FeladatokLista;

            AdatokBetoltese();
        }

        private async void AdatokBetoltese()
        {
            try
            {
                var adatok = await _httpClient.GetFromJsonAsync<List<GyakorlatWpfModell>>($"api/Gyakorlatok/felhasznalo/{_felhasznaloId}");
                FeladatokLista.Clear();
                if (adatok != null)
                {
                    foreach (var elem in adatok)
                    {
                        FeladatokLista.Add(elem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az adatok letöltésekor: {ex.Message}");
            }
        }

        private async void UjFeladatHozzaadasa_Kattintas(object sender, RoutedEventArgs e)
        {
            string ujNev = UjFeladatNevInput.Text.Trim();
            if (string.IsNullOrEmpty(ujNev)) return;

            var ujGyakorlat = new GyakorlatWpfModell
            {
                FelhasznaloId = _felhasznaloId,
                Nev = ujNev,
                Kategoria = "Altalanos"
            };

            // Elküldjük az API-nak az új gyakorlatot
            var response = await _httpClient.PostAsJsonAsync("api/Gyakorlatok", ujGyakorlat);
            if (response.IsSuccessStatusCode)
            {
                UjFeladatNevInput.Clear();

                // ITT A LÉNYEG: Sikeres mentés után azonnal újratöltjük az adatokat az adatbázisból,
                // így a DataGrid táblázat azonnal frissülni fog a képernyőn!
                AdatokBetoltese();
            }
            else
            {
                MessageBox.Show("Nem sikerült elmenteni az új gyakorlatot.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void FeladatTorlese_Kattintas(object sender, RoutedEventArgs e)
        {
            if (sender is Button gomb && gomb.DataContext is GyakorlatWpfModell torlendo)
            {
                var response = await _httpClient.DeleteAsync($"api/Gyakorlatok/{torlendo.Id}");
                if (response.IsSuccessStatusCode)
                {
                    FeladatokLista.Remove(torlendo);
                }
            }
        }

        private void StatisztikaAblak_Megnyitas(object sender, RoutedEventArgs e)
        {
            StatisztikaAblak statAblak = new StatisztikaAblak(_felhasznaloId);
            statAblak.ShowDialog();
        }
    }
}
