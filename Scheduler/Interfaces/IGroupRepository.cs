using Scheduler.Entity;
using Scheduler.Models;

namespace Scheduler.Interfaces
{
    public interface IGroupRepository
    {
        ResultModel AddMembersToGroup(int groupId, List<int> membersId);
        void AddUser(int groupId, int userId);
        void CreateGroup(Group group);
        void DeleteGroup(int groupId);
        void Dispose();
        void EditGroup(Group group);
        List<GroupResult> GetGroupListWithMembers();
        List<User> GetUsersInGroup(int groupId);
        List<GroupResult> GetYourGroupListWithMembers(int userId);
        ResultModel RemoveMembersToGroup(int groupId, List<int> membersId);
        void RemoveUser(int groupId, int userId);
    }
}