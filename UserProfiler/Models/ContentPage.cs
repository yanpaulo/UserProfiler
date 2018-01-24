using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfiler.Models
{
    public class ContentPage
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        public ICollection<UserActivity> ActivityHistory { get; set; }
    }
}
