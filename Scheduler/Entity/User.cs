using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Entity
{
    public class User
    {
        public User()
        {
            Contacts = new List<UserContact>();
            ContactsOf = new List<UserContact>();

            Appointments = new List<Appointment>();
            Meetings = new List<UserSchedule>();
            Groups = new List<UserGroup>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLoggin { get; set; }
        public bool Active { get; set; }

        public ICollection<UserContact> Contacts { get; set; }
        public ICollection<UserContact> ContactsOf { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<UserSchedule> Meetings { get; set; }
        public ICollection<UserGroup> Groups { get; set; }
    }
}