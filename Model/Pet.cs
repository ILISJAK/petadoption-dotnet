namespace PetAdoption.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public string? Name { get; set; }
        public string? Species { get; set; }
        public int Age { get; set; }
        public int ShelterId { get; set; }
        public Shelter? Shelter { get; set; } // Make Shelter nullable
    }
}
