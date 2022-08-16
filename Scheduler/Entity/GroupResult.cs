using System;
namespace Scheduler.Entity
{
    public class GroupResult
    {
        public GroupResult()
        {
            Members = new List<Member>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string OwnerEmail { get; set; }
        public bool IsOwner { get; set; }
        public bool Active { get; set; }
        public List<Member> Members { get; set; }

        public class Member
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
        }
    }
}

