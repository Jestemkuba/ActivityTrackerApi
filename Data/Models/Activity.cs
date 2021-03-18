using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Data.Models
{
    public class Activity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
