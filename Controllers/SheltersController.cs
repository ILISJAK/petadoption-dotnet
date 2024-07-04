using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetAdoption.Data;
using PetAdoption.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetAdoption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheltersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SheltersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Shelters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shelter>>> GetShelters()
        {
            return await _context.Shelters.Include(s => s.Pets).ToListAsync();
        }

        // GET: api/Shelters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shelter>> GetShelter(int id)
        {
            var shelter = await _context.Shelters.Include(s => s.Pets).FirstOrDefaultAsync(s => s.ShelterId == id);

            if (shelter == null)
            {
                return NotFound();
            }

            return shelter;
        }

        // PUT: api/Shelters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShelter(int id, Shelter shelter)
        {
            if (id != shelter.ShelterId)
            {
                return BadRequest();
            }

            _context.Entry(shelter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShelterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Shelters
        [HttpPost]
        public async Task<ActionResult<Shelter>> PostShelter(Shelter shelter)
        {
            _context.Shelters.Add(shelter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShelter), new { id = shelter.ShelterId }, shelter);
        }

        // DELETE: api/Shelters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShelter(int id)
        {
            var shelter = await _context.Shelters.FindAsync(id);
            if (shelter == null)
            {
                return NotFound();
            }

            _context.Shelters.Remove(shelter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShelterExists(int id)
        {
            return _context.Shelters.Any(e => e.ShelterId == id);
        }
    }
}
