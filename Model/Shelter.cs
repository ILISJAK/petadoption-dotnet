namespace PetAdoption.Models
{
    public class Shelter
    {
        public int ShelterId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ContactInfo { get; set; }
        public List<Pet> Pets { get; set; }
    }
}
