using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Authorization;
using Scheduler.Entity;
using Scheduler.Models;
using Scheduler.Services;
using Scheduler.SharedCode;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private UserService _userService;
    private UserRepository _userRepository;
    private GroupRepository _groupRepository;

    public UserController()
    {
        _userService = new UserService();
        _userRepository = new UserRepository();
        _groupRepository = new GroupRepository();
    }

    // GET: api/values
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("[action]", Name = "Login")]
    [Consumes("application/json")]
    public ActionResult Login([FromBody] UserCredential userCredential)
    {
        var user = _userService.Authenticate(userCredential.Username, userCredential.Password);

        if (user.IsAuthenticated)
        {
            string credential = userCredential.Username + ":" + userCredential.Password;

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credential);
            user.Credential = Convert.ToBase64String(plainTextBytes);
        }

        return Ok(user);
    }

    [CustomAuthorize]
    [HttpGet]
    [Route("[action]", Name = "GetAllActiveUser")]
    public async Task<IActionResult> GetAllActiveUser()
    {
        List<UserBasic> results;

        try
        {
            // var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;

            var users = _userRepository.GetAllUser();
            results = ClassConverter.ConvertToUserBasic(users);
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
            dispose();
        }

        return Ok(results);
    }

    [CustomAuthorize]
    [Consumes("application/json")]
    [Route("[action]", Name = "GetCurrentLogUser")]
    public async Task<ActionResult> GetCurrentLogUser()
    {
        User userEntity = new User();

        try
        {
            var currentUser = HttpContext.Items["User"] as UserIdentity;
            userEntity = _userRepository.GetUserById(currentUser.Id);
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
            dispose();
        }


        return Ok(userEntity);
    }

    [AllowAnonymous]
    [Consumes("application/json")]
    [Route("[action]", Name = "AddUser")]
    public async Task<ActionResult> Register([FromBody] UserBasic user)
    {
        ResultModel result = new ResultModel();
        result.Success = false;

        try
        {
            if (_userRepository.IsExist(user.UserName))
            {
                result.Message = "User already exist";
                return Ok(result);

            }
            if (_userRepository.IsExistByEmail(user.Email))
            {
                result.Message = "Email already exist";
                return Ok(result);
            }

            var userToadd = ClassConverter.ConvertModelToUserEntity(user);
            _userRepository.AddUser(userToadd);
            result.Success = true;
            result.Message = "Success registering user";
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
            dispose();
        }

        return Ok(result);
    }

    [HttpPost]
    [CustomAuthorize]
    [Consumes("application/json")]
    [Route("[action]", Name = "EditCurrentLogUser")]
    public async Task<ActionResult> EditCurrentLogUser([FromBody] UserBasic model)
    {
        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            var userToEdit = _userRepository.GetUserByUsername(currentlyLogUser.Username);

            if (userToEdit != null)
            {
                User user = new User();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.MiddleName = model.MiddleName;
                user.Id = userToEdit.Id;
                _userRepository.EditUser(user);
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
            dispose();
        }

        return Ok(model);
    }

    [AllowAnonymous]
    [Route("[action]", Name = "IsUserExist")]
    public async Task<ActionResult> IsUserExist(string userName)
    {
        bool isExist = false;

        try
        {
            isExist = _userRepository.IsExist(userName);
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
            dispose();
        }

        return Ok(isExist);
    }

    [AllowAnonymous]
    [Route("[action]", Name = "IsEmailExist")]
    public async Task<ActionResult> IsEmailExist(string email)
    {
        bool isExist = false;

        try
        {
            isExist = _userRepository.IsExistByEmail(email);
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
            dispose();
        }

        return Ok(isExist);
    }

    [CustomAuthorize]
    [Route("[action]", Name = "GetContacts")]
    public async Task<ActionResult> GetContacts()
    {
        List<UserBasic> results = new List<UserBasic>();

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;

            var users = _userRepository.GetContacts(currentlyLogUser.Id);

            results = ClassConverter.ConvertToUserBasic(users);
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
            dispose();
        }

        return Ok(results);
    }

    [CustomAuthorize]
    [Route("[action]", Name = "AddContact")]
    public async Task<ActionResult> AddContact(string email)
    {
        ResultModel result = new ResultModel();

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            var contact = _userRepository.GetUserByEmail(email);

            if (contact != null)
            {
                _userRepository.AddContact(currentlyLogUser.Id, contact.Id);
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.Message = "User does not exist";
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
            dispose();
        }

        return Ok(result);
    }

    [CustomAuthorize]
    [Route("[action]", Name = "RemoveContact")]
    public async Task<ActionResult> RemoveContact(int contactId)
    {
        ResultModel result = new ResultModel();

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
             _userRepository.RemoveContact(currentlyLogUser.Id, contactId);
            result.Success = true;
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
            dispose();
        }

        return Ok(result);
    }

    [CustomAuthorize]
    [Route("[action]", Name = "GetAllUserExcludingYou")]
    public async Task<ActionResult> GetAllUserExcludingYou()
    {
        List<UserBasic> results;

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;

            var users = _userRepository.GetAllUser();
            var userToExclude = users.FirstOrDefault(u => u.Id == currentlyLogUser.Id);
            users.Remove(userToExclude);

            results = ClassConverter.ConvertToUserBasic(users);
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
            dispose();
        }

        return Ok(results);
    }

    [CustomAuthorize]
    [Consumes("application/json")]
    [Route("[action]", Name = "ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] PasswordModel model)
    {
        var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
        ChangePasswordResult result = new ChangePasswordResult();

        try
        {
            result = _userRepository.ChangePassword(currentlyLogUser.Id, model.OldPassword, model.NewPassword);
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
            dispose();
        }

        return Ok(result);
    }

    [HttpGet]
    [CustomAuthorize]
    [Route("[action]", Name = "GetGroupListWithMembers")]
    public async Task<ActionResult> GetGroupListWithMembers()
    {
        var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
        var results = new List<GroupResult>();

        try
        {
            var user = _userRepository.GetUserById(currentlyLogUser.Id);
            results = _groupRepository.GetGroupListWithMembers();
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
            dispose();
        }

        return Ok(results);
    }

    [HttpGet]
    [CustomAuthorize]
    [Route("[action]", Name = "GetYourGroupListWithMembers")]
    public async Task<ActionResult> GetYourGroupListWithMembers()
    {
        var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
        var results = new List<GroupResult>();

        try
        {
            results = _groupRepository.GetYourGroupListWithMembers(currentlyLogUser.Id);
            
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
            dispose();
        }

        return Ok(results);
    }


    [HttpPost]
    [CustomAuthorize]
    [Consumes("application/json")]
    [Route("[action]", Name = "CreateGroup")]
    public async Task<ActionResult> CreateGroup([FromBody] CreateEditGroupModel model)
    {
        var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
        var user = _userRepository.GetUserById(currentlyLogUser.Id);
        ResultModel result = new ResultModel();

        var newGroup = new Group();

        newGroup.GroupName = model.GroupName;
        newGroup.Description = model.Description;
        newGroup.Active = true;
        newGroup.OwnerId = user.Id;
        
        try
        {
            _groupRepository.CreateGroup(newGroup);
            result.Success = true;
            result.Message = "Success";
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
            dispose();
        }

        return Ok(result);
    }

    [HttpPost]
    [CustomAuthorize]
    [Consumes("application/json")]
    [Route("[action]", Name = "EditGroup")]
    public async Task<ActionResult> EditGroup([FromBody] CreateEditGroupModel model)
    {
        var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
        ResultModel result = new ResultModel();
        
        var gToEdit = new Group();

        gToEdit.Id = model.Id;
        gToEdit.GroupName = model.GroupName;
        gToEdit.Description = model.Description;

        try
        {
            _groupRepository.EditGroup(gToEdit);
            result.Success = true;
            result.Message = "Success";
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
            dispose();
        }

        return Ok(result);
    }

    [HttpGet]
    [CustomAuthorize]
    [Consumes("application/json")]
    [Route("[action]", Name = "GetUsersInGroup")]
    public async Task<ActionResult> GetUsersInGroup(int groupId)
    {
        List<UserBasic> results = new List<UserBasic>();

        try
        {
            var users = _groupRepository.GetUsersInGroup(groupId);
            results = ClassConverter.ConvertToUserBasic(users);
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
            dispose();
        }

        return Ok(results);
    }

    [HttpPost]
    [CustomAuthorize]
    [Consumes("application/json")]
    [Route("[action]", Name = "AddMembersToGroup")]
    public async Task<ActionResult> AddMembersToGroup([FromBody] AddMembersToGroupModel model)
    {
        ResultModel result = new ResultModel();

        try
        {
            result = _groupRepository.AddMembersToGroup(model.GroupId, model.Members);
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
            dispose();
        }

        return Ok(result);
    }

    [HttpPost]
    [CustomAuthorize]
    [Consumes("application/json")]
    [Route("[action]", Name = "RemoveMembersToGroup")]
    public async Task<ActionResult> RemoveMembersToGroup([FromBody] AddMembersToGroupModel model)
    {
        ResultModel result = new ResultModel();

        try
        {
            result = _groupRepository.RemoveMembersToGroup(model.GroupId, model.Members);
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
            dispose();
        }

        return Ok(result);
    }

    private void dispose()
    {
        _userRepository.Dispose();
        _groupRepository.Dispose();
    }
}


