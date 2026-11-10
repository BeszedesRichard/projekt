using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EdzesKovetoApi.Data;
using EdzesKovetoApi.Models;

namespace EdzesKovetoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GyakorlatokController : ControllerBase
    {
        private readonly EdzesDbContext _context;

        public GyakorlatokController(EdzesDbContext context)
        {
            _context = context;
        }

        // CRUD: READ ALL (Felhasznalo alapjan)
        [HttpGet("felhasznalo/{felhasznaloId}")]
        public async Task<ActionResult<IEnumerable<Gyakorlat>>> GetGyakorlatokByFelhasznalo(int felhasznaloId)
        {
            return await _context.Gyakorlatok
                .Include(g => g.Naplok)
                .Where(g => g.FelhasznaloId == felhasznaloId)
                .ToListAsync();
        }

        // CRUD: READ BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Gyakorlat>> GetGyakorlat(int id)
        {
            var gyakorlat = await _context.Gyakorlatok.Include(g => g.Naplok).FirstOrDefaultAsync(g => g.Id == id);
            if (gyakorlat == null) return NotFound();
            return gyakorlat;
        }

        // CRUD: CREATE
        [HttpPost]
        public async Task<ActionResult<Gyakorlat>> PostGyakorlat(Gyakorlat gyakorlat)
        {
            _context.Gyakorlatok.Add(gyakorlat);
            await _context.SaveChangesAsync();

            // Automatkus 7 napos naplo alapertelmezett ertekekkel
            var napok = new string[] { "Hetfo", "Kedd", "Szerda", "Csutortok", "Pentek", "Szombat", "Vasarnap" };
            foreach (var nap in napok)
            {
                _context.EdzesNaplok.Add(new EdzesNaplo { GyakorlatId = gyakorlat.Id, NapNeve = nap, Elvegezve = false });
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGyakorlat), new { id = gyakorlat.Id }, gyakorlat);
        }

        // CRUD: UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGyakorlat(int id, Gyakorlat gyakorlat)
        {
            if (id != gyakorlat.Id) return BadRequest();
            _context.Entry(gyakorlat).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // CRUD: DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGyakorlat(int id)
        {
            var gyakorlat = await _context.Gyakorlatok.FindAsync(id);
            if (gyakorlat == null) return NotFound();
            _context.Gyakorlatok.Remove(gyakorlat);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // SAJAT FUGGVENY: Szuress kategoria alapjan
        [HttpGet("kategoria/{kategorianev}")]
        public async Task<ActionResult<IEnumerable<Gyakorlat>>> GetKategoriaAlapjan(string kategorianev)
        {
            return await _context.Gyakorlatok
                .Where(g => g.Kategoria.ToLower() == kategorianev.ToLower())
                .ToListAsync();
        }
    }
}
