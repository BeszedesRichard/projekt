using Microsoft.EntityFrameworkCore;
using EdzesKovetoApi.Models;

namespace EdzesKovetoApi.Data
{
    public class EdzesDbContext : DbContext
    {
        public EdzesDbContext(DbContextOptions<EdzesDbContext> options) : base(options) { }

        public DbSet<Felhasznalo> Felhasznalok { get; set; }
        public DbSet<Gyakorlat> Gyakorlatok { get; set; }
        public DbSet<EdzesNaplo> EdzesNaplok { get; set; }
    }
}
