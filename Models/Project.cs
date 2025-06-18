using System;
using System.Collections.Generic;

namespace Peggy.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ProjectParentId { get; set; }  // Nullable foreign key for parent project

        // Navigation properties
        public User Owner { get; set; }
        public ICollection<Patronage> Patronages { get; set; }
        public Project Parent { get; set; }  // Navigation property for parent project
        public ICollection<Project> ChildProjects { get; set; }  // Navigation property for child projects
    }
} 