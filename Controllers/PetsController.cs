using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetAdoption.Data;
using PetAdoption.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetAdoption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PetsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            return await _context.Pets.Include(p => p.Shelter).ToListAsync();
        }

        // GET: api/Pets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var pet = await _context.Pets.Include(p => p.Shelter).FirstOrDefaultAsync(p => p.PetId == id);

            if (pet == null)
            {
                return NotFound();
            }

            return pet;
        }

        // PUT: api/Pets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPet(int id, Pet pet)
        {
            if (id != pet.PetId)
            {
                return BadRequest();
            }

            _context.Entry(pet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
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

        // POST: api/Pets
        [HttpPost]
        public async Task<ActionResult<Pet>> PostPet(Pet pet)
        {
            // Log the incoming pet data
            Console.WriteLine($"Attempting to create pet: {JsonSerializer.Serialize(pet)}");

            // Validate the shelter exists
            var shelter = await _context.Shelters.FindAsync(pet.ShelterId);
            if (shelter == null)
            {
                Console.WriteLine("Shelter not found.");
                return BadRequest(new { Error = "Shelter not found." });
            }

            // Log shelter information
            Console.WriteLine($"Found shelter: {JsonSerializer.Serialize(shelter)}");

            try
            {
                // Manually assign the shelter to avoid EF validation issues
                pet.Shelter = shelter;

                // Log the modified pet data
                Console.WriteLine($"Modified pet for saving: {JsonSerializer.Serialize(pet)}");

                // Add and save the new pet
                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving pet: {ex.Message}");
                return StatusCode(500, new { Error = ex.Message });
            }

            // Log successful creation
            Console.WriteLine($"Pet created successfully: {pet.Name}, ID: {pet.PetId}");

            // Custom serialization settings to handle cycles
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            var result = new JsonResult(pet)
            {
                SerializerSettings = options
            };

            return result;
        }

        // DELETE: api/Pets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetId == id);
        }
    }
}
