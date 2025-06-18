using System;
using System.Collections.Generic;

namespace Peggy.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
        public int OwnerUserId { get; set; }

        // Navigation property to link Project to Patronage
        public ICollection<Patronage> Patronages { get; set; }
    }
} 