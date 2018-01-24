using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfiler.Models
{
    public class AnonymousUser
    {
        public Guid Id { get; set; }

        [Required]
        public string AppVersion { get; set; }

        [Required]
        public string UserAgent { get; set; }
        
        public DateTimeOffset CreationDate { get; set; }

        public ICollection<UserActivity> ActivityHistory { get; set; }
    }
}
