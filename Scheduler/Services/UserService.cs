using Scheduler.Models;

namespace Scheduler.Services
{
    public interface IUserService
    {
        UserIdentity Authenticate(string firstName, string lastName);
    }

    public class UserService : IUserService
    {
        private UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public UserIdentity Authenticate(string username, string password)
        {
            UserIdentity u = new UserIdentity();
            var user = _userRepository.GetUserByUsername(username);
            _userRepository.Dispose();

            if (user != null)
            {
                if (user.UserName == username && user.Password == password)
                {
                    u.Id = user.Id;
                    u.Username = username;
                    u.IsAuthenticated = true;
                    u.LoginStatus = "Success";
                    return u;
                }
            }

            u.IsAuthenticated = false;
            u.LoginStatus = "Wrong username and password";

            return u;
        }
    }
}

