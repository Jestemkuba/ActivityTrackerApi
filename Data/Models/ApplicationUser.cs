using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ActivityTrackerApi.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [JsonIgnore]
        public List<Activity> Activities { get; set; }
    }
}
