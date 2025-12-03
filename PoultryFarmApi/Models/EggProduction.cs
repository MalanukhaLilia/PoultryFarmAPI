using System.Text.Json.Serialization;

namespace PoultryFarmApi.Models
{
    public class EggProduction
    {
        public int Id { get; set; }
        public int EggCount { get; set; }
        public int BrokenCount { get; set; }
        public DateTime CollectionDate { get; set; }

        public int CoopId { get; set; }

        [JsonIgnore]
        public Coop? Coop { get; set; }
    }
}