using System;
using Scheduler.Entity;

namespace Scheduler.Models
{
    public class UserBasic
    {
        public UserBasic()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
    }
}