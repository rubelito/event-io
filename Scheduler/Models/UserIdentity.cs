using System;
namespace Scheduler.Models
{
    public class UserIdentity
    {
        public UserIdentity()
        {
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Credential { get; set; }
        public string LoginStatus { get; set; }
        public string Role { get; set; }
    }
}