using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Authorization;
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

                results = ClassConverter.ConvertToEventModel(events);

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
        [Route("[action]", Name = "ChangeScheduleDate")]
        [Consumes("application/json")]
        [CustomAuthorize]
        public async Task<IActionResult> ChangeScheduleDate([FromBody] DateModel ev)
        {
            try
            {
                _appointmentRepository.ChangeScheduleDate(ev.Id, ev.StrDate);
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

