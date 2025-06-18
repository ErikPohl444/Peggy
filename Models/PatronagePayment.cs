using System;

namespace Peggy.Models
{
    public class PatronagePayment
    {
        public int PaymentId { get; set; } // Primary key
        public int PatronageId { get; set; } // Foreign key
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; } // New property
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }

        // Navigation property to link PatronagePayment to Patronage
        public Patronage Patronage { get; set; }
    }
} 