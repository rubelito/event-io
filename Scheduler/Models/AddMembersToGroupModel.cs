using System;
namespace Scheduler.Models
{
    public class AddMembersToGroupModel
    {
        public int GroupId { get; set; }
        public List<int> Members { get; set; }
    }
}

