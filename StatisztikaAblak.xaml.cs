using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace EdzesKovetoWpf
{
    public partial class StatisztikaAblak : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:51991/") };

        public StatisztikaAblak(int felhasznaloId)
        {
            InitializeComponent();
            StatisztikaBetoltese(felhasznaloId);
        }

        private async void StatisztikaBetoltese(int felhasznaloId)
        {
            try
            {
                var res = await _httpClient.GetFromJsonAsync<StatisztikaDto>($"api/Felhasznalok/{felhasznaloId}/statisztika");
                if (res != null)
                {
                    OsszesPipaText.Text = $"Összes feladat: {res.OsszesFeladat}";
                    ElvegezveText.Text = $"Elvégzett feladatok: {res.Elvegezve}";
                    TeljesitmenyBar.Value = res.TeljesitmenySzazalek;
                    SzazalekText.Text = $"{res.TeljesitmenySzazalek}%";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a statisztika letöltésekor: {ex.Message}");
            }
        }
    }
}
