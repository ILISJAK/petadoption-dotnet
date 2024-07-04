namespace PetAdoption.Models
{
    public class Shelter
    {
        public int ShelterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public List<Pet> Pets { get; set; } = new List<Pet>();
    }
}
