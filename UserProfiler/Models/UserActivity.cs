using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfiler.Models
{
    public class UserActivity
    {
        public int Id { get; set; }

        [Required]
        public string Kind { get; set; }

        public string Location { get; set; }

        public DateTimeOffset Date { get; set; }

        public ContentPage ContentPage { get; set; }

        public AnonymousUser AnonymousUser { get; set; }

        public Guid AnonymousUserId { get; set; }

        public int ContentPageId { get; set; }
    }
}
