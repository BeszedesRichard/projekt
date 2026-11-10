using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace EdzesKovetoWpf
{
    public partial class BejelentkezesAblak : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:51991/") };

        public BejelentkezesAblak()
        {
            InitializeComponent();
        }

        private async void Bejelentkezes_Kattintas(object sender, RoutedEventArgs e)
        {
            string nev = FelhasznaloNevInput.Text.Trim();
            if (string.IsNullOrEmpty(nev))
            {
                MessageBox.Show("Adjon meg egy nevet!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var felhasznalok = await _httpClient.GetFromJsonAsync<List<FelhasznaloDto>>("api/Felhasznalok");
                var meglevo = felhasznalok?.Find(f => f.Nev.Equals(nev, StringComparison.OrdinalIgnoreCase));

                int felhasznaloId;

                if (meglevo != null)
                {
                    felhasznaloId = meglevo.Id;
                }
                else
                {
                    var ujFelhasznalo = new FelhasznaloDto { Nev = nev, Email = $"{nev.ToLower()}@edzes.hu" };
                    var response = await _httpClient.PostAsJsonAsync("api/Felhasznalok", ujFelhasznalo);
                    var kesz = await response.Content.ReadFromJsonAsync<FelhasznaloDto>();
                    felhasznaloId = kesz!.Id;
                }

                MainWindow foAblak = new MainWindow(felhasznaloId, nev);
                foAblak.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nem sikerült csatlakozni az API-hoz: {ex.Message}", "API Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
