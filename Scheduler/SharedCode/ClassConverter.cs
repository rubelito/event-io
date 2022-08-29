using System;
using Scheduler.Entity;
using Scheduler.Models;

namespace Scheduler.SharedCode
{
    public class ClassConverter
    {
        public static List<UserBasic> ConvertToUserBasic(List<User> users)
        {
            List<UserBasic> results = new List<UserBasic>();

            foreach (var u in users)
            {
                var ub = new UserBasic();
                ub.Id = u.Id;
                ub.FirstName = u.FirstName;
                ub.MiddleName = u.MiddleName;
                ub.LastName = u.LastName;
                ub.UserName = u.UserName;
                ub.Email = u.Email;
                ub.Enabled = u.Active;

                results.Add(ub);
            }

            return results;
        }

        public static List<GroupBasic> ConvertToGroupBasic(List<Group> groups)
        {
            List<GroupBasic> results = new List<GroupBasic>();

            foreach (var g in groups)
            {
                var gb = new GroupBasic();
                gb.Id = g.Id;
                gb.Name = g.GroupName;

                results.Add(gb);
            }

            return results;
        }

        public static List<EventModel> ConvertToEventModel(List<Appointment> appointments)
        {
            List<EventModel> results = new List<EventModel>();

            foreach (var a in appointments)
            {
                var m = new EventModel();
                m.Id = a.Id;
                m.Location = a.Location;
                m.Title = a.Title;
                m.Details = a.Details;
                m.YearMonth = a.YearMonth;
                m.Date = a.Date;
                m.Time = a.Time;
                m.CreatorId = a.Creator.Id;
                m.CreatedBy = a.Creator.UserName;

                results.Add(m);
            }

            return results;
        }

        public static Appointment ConvertToAppointment(EventModel model)
        {
            Appointment a = new Appointment();

            a.Id = model.Id;
            a.Location = model.Location;
            a.Title = model.Title;
            a.Details = model.Details;
            a.YearMonth = model.YearMonth;
            a.Date = model.Date;
            a.Time = model.Time;
            a.CreatorId = model.CreatorId;

            return a;
        }

        public static User ConvertModelToUserEntity(UserBasic model)
        {
            User u = new User();
            u.UserName = model.UserName;
            u.LastName = model.LastName;
            u.FirstName = model.FirstName;
            u.MiddleName = model.MiddleName;
            u.Email = model.Email;
            u.Password = model.Password;
            u.Active = true;

            return u;
        }
    }
}

