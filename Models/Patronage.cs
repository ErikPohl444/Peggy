using System;

namespace Peggy.Models
{
    public class Patronage
    {
        public int PatronageId { get; set; } // Primary key
        public int PatronUserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }

        // Navigation property to link Patronage to User
        public User PatronUser { get; set; }

        // Navigation property to link Patronage to Project
        public Project Project { get; set; }
    }
} 