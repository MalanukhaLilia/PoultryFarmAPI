namespace PoultryFarmApi.Models
{
    public class Coop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxCapacity { get; set; }

        public List<Bird> Birds { get; set; } = new List<Bird>();
    }
}