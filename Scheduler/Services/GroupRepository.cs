using System;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Scheduler.Entity;
using Scheduler.Models;
using Scheduler.SharedCode;

namespace Scheduler.Services
{
    public class GroupRepository
    {
        private MySqlConnection _conn;
        private SchedulerDbContext _dbContext;

        public GroupRepository()
        {
            //_conn = new MySqlConnection(StaticConfig.ConStr);
            //_dbContext = new SchedulerDbContext(_conn, false);
            //_dbContext.Database.CreateIfNotExists();
            //_conn.Open();

            _dbContext = new SchedulerDbContext();
        }

        public GroupRepository(SchedulerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateGroup(Group group)
        {
            try
            {
                _dbContext.Groups.Add(group);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditGroup(Group group)
        {
            try
            {
                var gToEdit = _dbContext.Groups.FirstOrDefault(g => g.Id == group.Id);

                gToEdit.GroupName = group.GroupName;
                gToEdit.Description = group.Description;

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetUsersInGroup(int groupId)
        {
            var users = _dbContext.UsersGroups.Where(g => g.GroupId == groupId).Select(g => g.User).ToList();
            return users;
        }

        public void AddUser(int groupId, int userId)
        {
            try
            {
                UserGroup ug = new UserGroup();
                ug.GroupId = groupId;
                ug.UserId = userId;

                _dbContext.UsersGroups.Add(ug);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveUser(int groupId, int userId)
        {
            try
            {
                var ugToRemove = _dbContext.UsersGroups.FirstOrDefault(f => f.GroupId == groupId && f.UserId == userId);

                if (ugToRemove != null) {
                    _dbContext.UsersGroups.Remove(ugToRemove);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteGroup(int groupId)
        {
            try
            {
                var gToDelete = _dbContext.Groups.FirstOrDefault(g => g.Id == groupId);

                _dbContext.Groups.Remove(gToDelete);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GroupResult> GetGroupListWithMembers()
        {
            List<GroupResult> results = new List<GroupResult>();

            List <Group> groups = _dbContext.Groups
                .Include("Owner")
                .Include("Members")
                .Include("Members.User")
                .ToList();

            foreach (var g in groups)
            {
                var r = new GroupResult();
                r.Id = g.Id;
                r.Name = g.GroupName;
                r.Owner = g.Owner.LastName + " ," + g.Owner.FirstName + " " + g.Owner.MiddleName;
                r.OwnerEmail = g.Owner.Email;
                r.Active = g.Active;

                foreach (var m in g.Members)
                {
                    var member = new GroupResult.Member();
                    member.LastName = m.User.LastName;
                    member.FirstName = m.User.FirstName;
                    member.MiddleName = m.User.MiddleName;

                    r.Members.Add(member);
                }

                results.Add(r);
            }

            return results;
        }

        public List<GroupResult> GetYourGroupListWithMembers(int userId)
        {
            List<GroupResult> results = new List<GroupResult>();

            List<Group> groups = _dbContext.Groups
                .Include("Owner")
                .Include("Members")
                .Include("Members.User")
                .Where(g => g.OwnerId == userId || g.Members.Any(m => m.UserId == userId))
                .ToList();

            foreach (var g in groups)
            {
                var r = new GroupResult();
                r.Id = g.Id;
                r.Name = g.GroupName;
                r.Owner = g.Owner.LastName + " ," + g.Owner.FirstName + " " + g.Owner.MiddleName;
                r.OwnerEmail = g.Owner.Email;
                r.IsOwner = g.OwnerId == userId;
                r.Active = g.Active;

                foreach (var m in g.Members)
                {
                    var member = new GroupResult.Member();
                    member.LastName = m.User.LastName;
                    member.FirstName = m.User.FirstName;
                    member.MiddleName = m.User.MiddleName;

                    r.Members.Add(member);
                }

                results.Add(r);
            }

            return results;
        }

        public ResultModel AddMembersToGroup(int groupId, List<int> membersId)
        {
            ResultModel result = new ResultModel();

            try {
                List<UserGroup> uGroups = new List<UserGroup>();

                foreach (var id in membersId)
                {
                    var ug = new UserGroup();
                    ug.GroupId = groupId;
                    ug.UserId = id;

                    uGroups.Add(ug);
                }

                _dbContext.UsersGroups.AddRange(uGroups);

                _dbContext.SaveChanges();
                result.Success = true;
                result.Message = "Success AddMembersToGroup";

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ResultModel RemoveMembersToGroup(int groupId, List<int> membersId)
        {
            ResultModel result = new ResultModel();

            try
            {
                var currentUg = _dbContext.UsersGroups.Where(u => u.GroupId == groupId &&  membersId.Any(id => id == u.UserId)).ToList();

                _dbContext.UsersGroups.RemoveRange(currentUg);

                _dbContext.SaveChanges();
                result.Success = true;
                result.Message = "Success RemoveMembersToGroup";

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public void Dispose()
        {
            //_dbContext.Database.Connection.Close();
        }
    }
}

