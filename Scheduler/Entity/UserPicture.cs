using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Entity
{
    public class UserPicture
    {
        public UserPicture()
        {
        }

        public int UserId { get; set; }
        public byte[] Picture { get; set; }

        public virtual User User { get; set; }
    }
}

