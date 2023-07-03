using System.Reflection;
using Scheduler.Entity;
using Scheduler.Models;
using Scheduler.Interfaces;

namespace Scheduler.Services
{
    public class UserRepository : IUserRepository
    {
        private SchedulerDbContext _dbContext;

        public UserRepository()
        {
            _dbContext = new SchedulerDbContext();
        }

        public UserRepository(SchedulerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUserById(int id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByUsername(string userName)
        {
            return _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public void AddUser(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsExist(int Id)
        {
            return _dbContext.Users.Any(u => u.Id == Id);
        }

        public bool IsExist(string userName)
        {
            return _dbContext.Users.Any(u => u.UserName == userName);
        }

        public bool IsExistByEmail(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }

        public void EditUser(User user)
        {
            _dbContext.Database.BeginTransaction();
            try
            {
                var uToEdit = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
                if (uToEdit == null)
                {
                    throw new ArgumentException("Cannot find existing user Id=" + user.Id);
                }

                uToEdit.FirstName = user.FirstName;
                uToEdit.MiddleName = user.MiddleName;
                uToEdit.LastName = user.LastName;

                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                throw ex;

            }
        }

        public ChangePasswordResult ChangePassword(int id, string oldPassword, string newPassword)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            var result = new ChangePasswordResult();

            try
            {
                if (user != null)
                {
                    if (user.Password == oldPassword)
                    {
                        if (!string.IsNullOrWhiteSpace(newPassword) && newPassword.Length >= 8)
                        {
                            if (user.Password != newPassword)
                            {
                                user.Password = newPassword;
                                _dbContext.SaveChanges();

                                result.IsChanged = true;
                                result.Message = "Success Changed";
                            }
                            else
                            {
                                result.IsChanged = false;
                                result.Message = "New password should not match the old password";
                            }
                        }
                        else
                        {
                            result.IsChanged = false;
                            result.Message = "New password should be 8 characters long";
                        }

                    }
                    else
                    {
                        result.IsChanged = false;
                        result.Message = "Old password does not match";
                    }
                }
                else
                {
                    throw new ArgumentException("User does not exist");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<User> GetAllUser()
        {
            return _dbContext.Users.ToList();
        }

        public List<User> GetContacts(int userId)
        {
            return _dbContext.Users.Where(u => u.ContactsOf.Any(c => c.UserId == userId)).ToList();
        }

        public void AddContact(int userId, int contactId)
        {
            bool isUserExist = _dbContext.Users.Any(u => u.Id == userId);
            bool isContactExist = _dbContext.Users.Any(u => u.Id == contactId);

            try
            {
                if (isUserExist && isContactExist)
                {
                    bool isEntryExist = _dbContext.Contacts.Any(c => c.UserId == userId && c.ContactId == contactId);

                    if (!isEntryExist)
                    {
                        UserContact newContact = new UserContact();

                        newContact.UserId = userId;
                        newContact.ContactId = contactId;

                        _dbContext.Contacts.Add(newContact);
                        _dbContext.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveContact(int userId, int contactId)
        {
            try
            {
                var contactToRemove = _dbContext.Contacts.FirstOrDefault(c => c.UserId == userId && c.ContactId == contactId);

                if (contactToRemove != null)
                {
                    _dbContext.Contacts.Remove(contactToRemove);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public UserPicture GetUserPicture(int userId)
        {
            return _dbContext.UserPictures.FirstOrDefault(u => u.UserId == userId);
        }

        public void InsertUserPicture(UserPicture picture)
        {
            if (_dbContext.UserPictures.Any(u => u.UserId == picture.UserId))
            {
                var userPictureToRemove = _dbContext.UserPictures.FirstOrDefault(u => u.UserId == picture.UserId);
                _dbContext.UserPictures.Remove(userPictureToRemove);
                _dbContext.SaveChanges();

                _dbContext.UserPictures.Add(picture);
                _dbContext.SaveChanges();


            }
            else
            {
                _dbContext.UserPictures.Add(picture);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteProfilePicture(int userId)
        {
            var pictureToRemove = _dbContext.UserPictures.FirstOrDefault(u => u.UserId == userId);

            if (pictureToRemove != null)
            {
                _dbContext.UserPictures.Remove(pictureToRemove);
                _dbContext.SaveChanges();
            }
        }

        public byte[] GetEmptyProfilePicture()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string picturePath = path + @"/wwwroot/empty-profileIcon.png";

            bool exist = System.IO.File.Exists(picturePath);

            byte[] emptyPicture = File.ReadAllBytes(picturePath);

            return emptyPicture;
        }

        public void Dispose()
        {
        }
    }
}