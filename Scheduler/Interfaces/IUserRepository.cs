using Scheduler.Entity;
using Scheduler.Models;

namespace Scheduler.Interfaces
{
    public interface IUserRepository
    {
        void AddContact(int userId, int contactId);
        void AddUser(User user);
        ChangePasswordResult ChangePassword(int id, string oldPassword, string newPassword);
        void DeleteProfilePicture(int userId);
        void Dispose();
        void EditUser(User user);
        List<User> GetAllUser();
        List<User> GetContacts(int userId);
        byte[] GetEmptyProfilePicture();
        User GetUserByEmail(string email);
        User GetUserById(int id);
        User GetUserByUsername(string userName);
        UserPicture GetUserPicture(int userId);
        void InsertUserPicture(UserPicture picture);
        bool IsExist(int Id);
        bool IsExist(string userName);
        bool IsExistByEmail(string email);
        void RemoveContact(int userId, int contactId);
    }
}