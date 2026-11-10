using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EdzesKovetoApi.Data;
using EdzesKovetoApi.Models;

namespace EdzesKovetoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EdzesNaploController : ControllerBase
    {
        private readonly EdzesDbContext _context;

        public EdzesNaploController(EdzesDbContext context)
        {
            _context = context;
        }

        // CRUD: READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EdzesNaplo>>> GetNaplok()
        {
            return await _context.EdzesNaplok.ToListAsync();
        }

        // CRUD: READ BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EdzesNaplo>> GetNaplo(int id)
        {
            var naplo = await _context.EdzesNaplok.FindAsync(id);
            if (naplo == null) return NotFound();
            return naplo;
        }

        // CRUD: CREATE
        [HttpPost]
        public async Task<ActionResult<EdzesNaplo>> PostNaplo(EdzesNaplo naplo)
        {
            _context.EdzesNaplok.Add(naplo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNaplo), new { id = naplo.Id }, naplo);
        }

        // CRUD: UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNaplo(int id, EdzesNaplo naplo)
        {
            if (id != naplo.Id) return BadRequest();
            _context.Entry(naplo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // CRUD: DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNaplo(int id)
        {
            var naplo = await _context.EdzesNaplok.FindAsync(id);
            if (naplo == null) return NotFound();
            _context.EdzesNaplok.Remove(naplo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // SAJAT FUGGVENY: Pipa allapotanak gyors atvaltasa (Toggle)
        [HttpPatch("toggle/{gyakorlatId}/{napNeve}")]
        public async Task<IActionResult> TogglePipa(int gyakorlatId, string napNeve)
        {
            var naplo = await _context.EdzesNaplok
                .FirstOrDefaultAsync(n => n.GyakorlatId == gyakorlatId && n.NapNeve == napNeve);

            if (naplo == null) return NotFound();

            naplo.Elvegezve = !naplo.Elvegezve;
            await _context.SaveChangesAsync();

            return Ok(naplo);
        }
    }
}
