using System.Text.Json.Serialization;

namespace PoultryFarmApi.Models
{
    public class Bird
    {
        public int Id { get; set; }
        public string Species { get; set; }
        public double Weight { get; set; }
        public int AgeMonths { get; set; }

        public int CoopId { get; set; }
        [JsonIgnore]
        public Coop Coop { get; set; }
    }
}