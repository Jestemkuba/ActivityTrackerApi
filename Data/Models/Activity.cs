

using Newtonsoft.Json;

namespace ActivityTrackerApi.Data.Models
{
    public class Activity
    { 
        public int Id { get; set; }
        public long StravaId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public double MovingTime { get; set; }
        public double ElapsedTime { get; set; }
        public string Type { get; set; }
        public double AverageSpeed { get; set; }
        public double MaxSpeed { get; set; }
    }
}
