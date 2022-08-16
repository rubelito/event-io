using System;
using Scheduler.Entity;

namespace Scheduler.Models
{
    public class PrivilegeModel
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}

