using Microsoft.EntityFrameworkCore;
using PetAdoption.Models;

namespace PetAdoption.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
    }
}
