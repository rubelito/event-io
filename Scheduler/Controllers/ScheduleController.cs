﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Authorization;
using Scheduler.Entity;
using Scheduler.Models;
using Scheduler.Services;
using Scheduler.SharedCode;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class ScheduleController : Controller
    {
        private AppointmentRepository _appointmentRepository;
        private UserRepository _userRepository;

        public ScheduleController()
        {
            _appointmentRepository = new AppointmentRepository();
            _userRepository = new UserRepository();
        }

        [Route("[action]", Name = "GetMeetings")]
        [HttpGet]
        [CustomAuthorize]
        public IActionResult GetMeetings(string yearMonth)
        {
            List<EventModel> results = new List<EventModel>();
            try
            {
                var currentUser = HttpContext.Items["User"] as UserIdentity;
                var ownedEvents = _appointmentRepository.GetAppointments(currentUser.Username, yearMonth);

                var events = _appointmentRepository.GetMeetings(currentUser.Id, yearMonth);
                var groupInvites = _appointmentRepository.GetMeetingsThatYoureInvited(currentUser.Id, yearMonth);
                events = events.UnionBy(ownedEvents, e => e.Id).ToList(); //Combine and remove duplicates;
                events = events.UnionBy(groupInvites, e => e.Id).ToList();

                var ownedEventsWithRepeats = _appointmentRepository.GetAppointmentsWithRepeats(currentUser.Username);

                var eventsWithRepeats = _appointmentRepository.GetMeetingsWithRepeat(currentUser.Id);
                var groupInvitesWithRepeats = _appointmentRepository.GetMeetingsThatYoureInvitedWithRepeats(currentUser.Id);
                eventsWithRepeats = eventsWithRepeats.UnionBy(ownedEventsWithRepeats, e => e.Id).ToList(); //Combine and remove duplicates;
                eventsWithRepeats = eventsWithRepeats.UnionBy(groupInvitesWithRepeats, e => e.Id).ToList();

                eventsWithRepeats = RemoveFutureRepeats(eventsWithRepeats, yearMonth);

                ScheduleRepeater repeatGenerator = new ScheduleRepeater();
                var repeats = repeatGenerator.HandleRepeats(eventsWithRepeats, yearMonth);

                ApplyEditRepeats(repeats);
                events.AddRange(repeats);
                results = ClassConverter.ConvertToEventModel(events);
                results = results.Where(r => !r.IsDeleted).ToList();

                foreach (var r in results)
                {
                    r.IsOwner = r.CreatedBy == currentUser.Username;
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok(results);
        }

        [HttpPost]
        [Route("[action]", Name = "GetNumberOfRepeats")]
        [Consumes("application/json")]
        [CustomAuthorize]
        public IActionResult GetNumberOfRepeats([FromBody] EventModel model)
        {
            int numberOfRepeats = 0;

            Appointment ap = ClassConverter.ConvertToAppointment(model);
            var originalEvent = _appointmentRepository.GetAppointmentById(model.Id);
            ap.Date = originalEvent.Date;

            ScheduleRepeater repeaGenerator = new ScheduleRepeater();
            var repeatDates = repeaGenerator.GetDateRangeForRepeats(ap, ap.YearMonth);
            numberOfRepeats = repeatDates.Count;

            return Ok(numberOfRepeats);   
        }


        private List<Appointment> RemoveFutureRepeats(List<Appointment> apps, string yearMonth)
        {
            DateTime dateOfMonth = DateTime.ParseExact(yearMonth, "M/yyyy", CultureInfo.InvariantCulture);
            dateOfMonth = new DateTime(dateOfMonth.Year, dateOfMonth.Month, DateTime.DaysInMonth(dateOfMonth.Year, dateOfMonth.Month));
            return apps.Where(e => e.Date <= dateOfMonth).ToList();
        }

        private void ApplyEditRepeats(List<Appointment> apps)
        {
            AppointmentRepository _appointmentRepository = new AppointmentRepository();
            var repeatEdits = new List<RepeatEdit>();

            var withRepeats = apps.Where(a => a.isRepeat).Select(a => a.Id).Distinct().ToList();

            foreach (var r in withRepeats)
            {
                var editRepeats = _appointmentRepository.GetRepeatEdits(r);
                foreach (var er in editRepeats)
                {
                    var ap = apps.FirstOrDefault(wr => wr.Id == er.AppointmentId && wr.Date == er.OriginalDate);

                    if (ap != null)
                    {
                        ap.RepeatEdit = er;
                    }
                }
            }

            var hello = repeatEdits;
        }


        [HttpPost]
        [Route("[action]", Name = "CreateEvent")]
        [Consumes("application/json")]
        [CustomAuthorize]
        public async Task<IActionResult> CreateEvent([FromBody] AddEditModel ev)
        {
            try
            {
                var currentUser = HttpContext.Items["User"] as UserIdentity;

                var appointment = ClassConverter.ConvertToAppointment(ev.Appointment);

                var newlyCreatedAppointment = _appointmentRepository.AddAppointment(currentUser.Username, appointment);
                _appointmentRepository.AssignUserToAppointments(ev.MemberIds, newlyCreatedAppointment.Id);
                _appointmentRepository.AssignGroupToAppointments(ev.GroupIds, newlyCreatedAppointment.Id);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok("Success");
        }

        [HttpPost]
        [Route("[action]", Name = "EditEvent")]
        [Consumes("application/json")]
        [CustomAuthorize]
        public async Task<IActionResult> EditEvent([FromBody] AddEditModel ev)
        {
            try
            {
                var appointment = ClassConverter.ConvertToAppointment(ev.Appointment);

                _appointmentRepository.EditAppointment(appointment);
                _appointmentRepository.DeleteRepeats(appointment.Id);

                _appointmentRepository.DeleteAllUserAsignedToAppointment(ev.Appointment.Id);
                _appointmentRepository.AssignUserToAppointments(ev.MemberIds, ev.Appointment.Id);

                _appointmentRepository.DeleteAllGroupAsignedToAppointment(appointment.Id);
                _appointmentRepository.AssignGroupToAppointments(ev.GroupIds, appointment.Id);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok("Success");
        }

        [HttpPost]
        [Route("[action]", Name = "EditRepeat")]
        [Consumes("application/json")]
        [CustomAuthorize]
        public async Task<IActionResult> EditRepeat([FromBody] AddEditModel ev)
        {
            try
            {
                bool hasDifference = hasDifferenceOnRepeatSettings(ev.Appointment);
                if (hasDifference)
                {
                    _appointmentRepository.EditAppointmentRepeatSettings(ev.Appointment);
                }

                _appointmentRepository.EditAllRepeatAppointment(ev.Appointment);

                _appointmentRepository.DeleteAllUserAsignedToAppointment(ev.Appointment.Id);
                _appointmentRepository.AssignUserToAppointments(ev.MemberIds, ev.Appointment.Id);

                _appointmentRepository.DeleteAllGroupAsignedToAppointment(ev.Appointment.Id);
                _appointmentRepository.AssignGroupToAppointments(ev.GroupIds, ev.Appointment.Id);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok("Success");
        }

        private bool hasDifferenceOnRepeatSettings(EventModel model)
        {
            var originalEvent = _appointmentRepository.GetAppointmentById(model.Id);

            bool hasDifference = false;

            if ((originalEvent.RepeatSelection != model.RepeatSelection) ||
                (originalEvent.RepeatEnd != model.RepeatEnd) ||
                (originalEvent.After != model.After) ||
                 originalEvent.OnDate.ToString("MM/dd/yyyy") != model.OnDate)
            {
                hasDifference = true;
            }

            return hasDifference;

        }

        [HttpPost]
        [Route("[action]", Name = "ChangeScheduleDate")]
        [Consumes("application/json")]
        [CustomAuthorize]
        public async Task<IActionResult> ChangeScheduleDate([FromBody] DateModel ev)
        {
            try
            {
                DateTime parsedDate = DateTime.ParseExact(ev.StrDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _appointmentRepository.ChangeScheduleDate(ev.Id, parsedDate);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok("Success");
        }

        [HttpPost]
        [Route("[action]", Name = "DeleteAppointment")]
        [Consumes("application/json")]
        [CustomAuthorize]
        public async Task<IActionResult> DeleteAppointment([FromBody] int id)
        {
            try
            {
                var currentUser = HttpContext.Items["User"] as UserIdentity;
                _appointmentRepository.DeleteAppointment(currentUser.Username, id);
                _appointmentRepository.DeleteRepeats(id);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok("Success");
        }

        [HttpGet]
        [Route("[action]", Name = "DeleteAppointmentRepeat")]
        [CustomAuthorize]
        public async Task<IActionResult> DeleteAppointmentRepeat(int appointmentId, string originalDateStr)
        {
            try
            {
                var currentUser = HttpContext.Items["User"] as UserIdentity;
                DateTime originalDate = DateTime.ParseExact(originalDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _appointmentRepository.MarkRepeatAsDeleted(appointmentId, originalDate);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok("Success");
        }

        [CustomAuthorize]
        [HttpGet]
        [Route("[action]", Name = "GetAllAttendees")]
        public async Task<IActionResult> GetAllAttendees(int appointmentId)
        {
            List<UserBasic> results = new List<UserBasic>();
            try
            {
                var attendees = _appointmentRepository.GetAllAttendeesByAppointment(appointmentId);
                results = ClassConverter.ConvertToUserBasic(attendees);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok(results);
        }

        [HttpGet]
        [CustomAuthorize]
        [Route("[action]", Name = "GetAllGroupAttendees")]
        public async Task<IActionResult> GetAllGroupAttendees(int appointmentId)
        {
            List<GroupBasic> results = new List<GroupBasic>();
            try
            {
                var groups = _appointmentRepository.GetAllGroupAttendessByAppointment(appointmentId);
                results = ClassConverter.ConvertToGroupBasic(groups);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            finally
            {
                Dispose();
            }

            return Ok(results);
        }

        private void Dispose()
        {
            _userRepository.Dispose();
            _appointmentRepository.Dispose();
        }
    }
}

