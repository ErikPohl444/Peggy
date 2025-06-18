using System;
using System.Collections.Generic;

namespace Peggy.Models
{
    public class ProjectCollection
    {
        public int CollectionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public int OwnerUserId { get; set; }

        // Navigation properties
        public User Owner { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
} 