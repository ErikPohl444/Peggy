using System;

namespace Peggy.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }
    }
} 