using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EdzesKovetoApi.Data;
using EdzesKovetoApi.Models;

namespace EdzesKovetoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FelhasznalokController : ControllerBase
    {
        private readonly EdzesDbContext _context;

        public FelhasznalokController(EdzesDbContext context)
        {
            _context = context;
        }

        // CRUD: READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Felhasznalo>>> GetFelhasznalok()
        {
            return await _context.Felhasznalok.ToListAsync();
        }

        // CRUD: READ BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Felhasznalo>> GetFelhasznalo(int id)
        {
            var felhasznalo = await _context.Felhasznalok.FindAsync(id);
            if (felhasznalo == null) return NotFound();
            return felhasznalo;
        }

        // CRUD: CREATE
        [HttpPost]
        public async Task<ActionResult<Felhasznalo>> PostFelhasznalo(Felhasznalo felhasznalo)
        {
            _context.Felhasznalok.Add(felhasznalo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFelhasznalo), new { id = felhasznalo.Id }, felhasznalo);
        }

        // CRUD: UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFelhasznalo(int id, Felhasznalo felhasznalo)
        {
            if (id != felhasznalo.Id) return BadRequest();
            _context.Entry(felhasznalo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // CRUD: DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFelhasznalo(int id)
        {
            var felhasznalo = await _context.Felhasznalok.FindAsync(id);
            if (felhasznalo == null) return NotFound();
            _context.Felhasznalok.Remove(felhasznalo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // SAJAT FUGGVENY 1: Teljesitmeny statisztika kiszamitasa (%)
        [HttpGet("{id}/statisztika")]
        public async Task<ActionResult<object>> GetStatisztika(int id)
        {
            var gyakorlatok = await _context.Gyakorlatok
                .Include(g => g.Naplok)
                .Where(g => g.FelhasznaloId == id)
                .ToListAsync();

            int osszesPipa = 0;
            int elvegzettPipa = 0;

            foreach (var gy in gyakorlatok)
            {
                if (gy.Naplok != null)
                {
                    osszesPipa += gy.Naplok.Count;
                    elvegzettPipa += gy.Naplok.Count(n => n.Elvegezve);
                }
            }

            double szazalek = osszesPipa == 0 ? 0 : Math.Round((double)elvegzettPipa / osszesPipa * 100, 2);

            return Ok(new { FelhasznaloId = id, OsszesFeladat = osszesPipa, Elvegezve = elvegzettPipa, TeljesitmenySzazalek = szazalek });
        }

        // SAJAT FUGGVENY 2: Inaktiv felhasznalok keresese
        [HttpGet("inaktivak")]
        public async Task<ActionResult<IEnumerable<Felhasznalo>>> GetInaktivFelhasznalok()
        {
            var inaktivak = await _context.Felhasznalok
                .Where(f => !_context.EdzesNaplok.Any(n => n.Gyakorlat!.FelhasznaloId == f.Id && n.Elvegezve))
                .ToListAsync();

            return Ok(inaktivak);
        }
    }
}
