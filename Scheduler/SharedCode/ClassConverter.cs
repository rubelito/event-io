using System;
using System.Globalization;
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
                m.Color = a.Color;
                m.Details = a.Details;
                m.ActualYearMonth = a.YearMonth;
                m.YearMonth = a.YearMonth;
                m.OriginalDate = a.Date.ToString("MM/dd/yyyy");
                m.Date = a.Date.ToString("MM/dd/yyyy");
                m.Time = a.Time;
                m.IsRepeat = a.isRepeat;
                m.RepeatSelection = a.RepeatSelection;
                m.RepeatEnd = a.RepeatEnd;
                m.After = a.After;
                m.OnDate = a.OnDate.ToString("MM/dd/yyyy");
                m.CreatorId = a.Creator.Id;
                m.CreatedBy = a.Creator.UserName;
                m.Type = a.Type;
                m.IsDone = a.IsDone;
                m.IsClone = a.IsClone;
                m.NumberOfRepeats = a.NumberOfRepeats;

                if (a.isRepeat && a.RepeatEdit != null)
                {
                    m.HasEdit = true;
                    m.OriginalDate = a.RepeatEdit.OriginalDate.ToString("MM/dd/yyyy");
                    m.Date = a.RepeatEdit.EditedDate.ToString("MM/dd/yyyy"); ;

                    m.Title = a.RepeatEdit.Title;
                    m.Location = a.RepeatEdit.Location;
                    m.Color = a.RepeatEdit.Color;
                    m.Details = a.RepeatEdit.Details;
                    m.Time = a.RepeatEdit.Time;
                    m.IsDone = a.RepeatEdit.IsDone;
                    m.IsDeleted = a.RepeatEdit.IsDeleted;
                }

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
            a.Color = model.Color;
            a.Details = model.Details;
            a.YearMonth = model.YearMonth;
            a.Date = DateTime.ParseExact(model.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            a.Time = model.Time;
            a.CreatorId = model.CreatorId;
            a.isRepeat = model.IsRepeat;
            a.RepeatSelection = model.RepeatSelection;
            a.RepeatEnd = model.RepeatEnd;
            a.Type = model.Type;
            a.IsDone = model.IsDone;
            a.After = model.After;
            a.OnDate = DateTime.ParseExact(model.OnDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            a.IsClone = model.IsClone;
            a.IsDeleted = model.IsDeleted;

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

