using System;
using System.Collections.Generic;

namespace EdzesKovetoApi.Models
{
    // 1. Entitas: Felhasznalok (User)
    public class Felhasznalo
    {
        public int Id { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisztracioDatuma { get; set; } = DateTime.Now;

        // Navigacios tulajdonsag
        public List<Gyakorlat>? Gyakorlatok { get; set; }
    }

    // 2. Entitas: Gyakorlatok / Feladatok (Exercise)
    public class Gyakorlat
    {
        public int Id { get; set; }
        public int FelhasznaloId { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string Kategoria { get; set; } = "Altalanos";

        // Navigacios tulajdonsagok
        public Felhasznalo? Felhasznalo { get; set; }
        public List<EdzesNaplo>? Naplok { get; set; }
    }

    // 3. Entitas: Edzes Naplo / Pipak (WorkoutLog)
    public class EdzesNaplo
    {
        public int Id { get; set; }
        public int GyakorlatId { get; set; }
        public string NapNeve { get; set; } = string.Empty; // Hetfo, Kedd, stb.
        public bool Elvegezve { get; set; } = false;

        // Navigacios tulajdonsag
        public Gyakorlat? Gyakorlat { get; set; }
    }
}
