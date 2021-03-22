using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ActivityTrackerApi.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public List<Activity> Activities { get; set; }
    }
}
