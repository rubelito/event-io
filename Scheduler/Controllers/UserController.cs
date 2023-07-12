using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Authorization;
using Scheduler.Entity;
using Scheduler.Models;
using Scheduler.Interfaces;
using Scheduler.Services;
using Scheduler.SharedCode;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserService _userService;
    private IUserRepository _userRepository;
    private IGroupRepository _groupRepository;
    private IActivityLoggerSql _activityLoggSql;
    private TelemetryClient _telemetry;

    public UserController(IGroupRepository groupRepository, IUserRepository userRepository, IUserService userService, IActivityLoggerSql activityLoggerSql)
    {
        _userService = userService;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _activityLoggSql = activityLoggerSql;
        _telemetry = new TelemetryClient();
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
        UserIdentity user;

        try
        {
            user = _userService.Authenticate(userCredential.Username, userCredential.Password);

            if (user.IsAuthenticated)
            {
                string credential = userCredential.Username + ":" + userCredential.Password;

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credential);
                user.Credential = Convert.ToBase64String(plainTextBytes);
            }
        }
        catch (Exception ex)
        {
            LogError(userCredential.Username, "UserController/Login", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
        }

        return Ok(user);
    }

    [CustomAuthorize]
    [HttpGet]
    [Route("[action]", Name = "GetAllActiveUser")]
    public async Task<IActionResult> GetAllActiveUser()
    {
        List<UserBasic> results;
        string userName = "";

        try
        {
            var currentUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentUser.Username;
            var users = _userRepository.GetAllUser();
            results = ClassConverter.ConvertToUserBasic(users);
        }
        catch (Exception ex)
        {
            
            LogError(userName, "UserController/GetAllActiveUser", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";

        try
        {
            var currentUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentUser.Username;
            userEntity = _userRepository.GetUserById(currentUser.Id);
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/GetAllActiveUser", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
            LogError("N/A", "UserController/Register", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
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
            LogError(userName, "UserController/EditCurrentLogUser", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
            LogError("N/A", "UserController/IsUserExist", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
            LogError("N/A", "UserController/IsEmailExist", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
            currentlyLogUser = null;

            var users = _userRepository.GetContacts(currentlyLogUser.Id);

            results = ClassConverter.ConvertToUserBasic(users);
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/GetContacts", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
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
            LogError(userName, "UserController/AddContact", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
             _userRepository.RemoveContact(currentlyLogUser.Id, contactId);
            result.Success = true;
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/RemoveContact", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;

            var users = _userRepository.GetAllUser();
            var userToExclude = users.FirstOrDefault(u => u.Id == currentlyLogUser.Id);
            users.Remove(userToExclude);

            results = ClassConverter.ConvertToUserBasic(users);
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/GetAllUserExcludingYou", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";
        ChangePasswordResult result = new ChangePasswordResult();

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
            result = _userRepository.ChangePassword(currentlyLogUser.Id, model.OldPassword, model.NewPassword);
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/ChangePassword", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";        
        var results = new List<GroupResult>();

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
            var user = _userRepository.GetUserById(currentlyLogUser.Id);
            results = _groupRepository.GetGroupListWithMembers();
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/GetGroupListWithMembers", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";
        var results = new List<GroupResult>();

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
            results = _groupRepository.GetYourGroupListWithMembers(currentlyLogUser.Id);
            
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/GetYourGroupListWithMembers", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";
        ResultModel result = new ResultModel();

        var newGroup = new Group();

        newGroup.GroupName = model.GroupName;
        newGroup.Description = model.Description;
        newGroup.Active = true;
        
        
        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
            var user = _userRepository.GetUserById(currentlyLogUser.Id);

            newGroup.OwnerId = user.Id;
            _groupRepository.CreateGroup(newGroup);
            result.Success = true;
            result.Message = "Success";
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/CreateGroup", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";
        ResultModel result = new ResultModel();
        
        var gToEdit = new Group();

        gToEdit.Id = model.Id;
        gToEdit.GroupName = model.GroupName;
        gToEdit.Description = model.Description;

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;

            _groupRepository.EditGroup(gToEdit);
            result.Success = true;
            result.Message = "Success";
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/EditGroup", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";
        List<UserBasic> results = new List<UserBasic>();

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;

            var users = _groupRepository.GetUsersInGroup(groupId);
            results = ClassConverter.ConvertToUserBasic(users);
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/GetUsersInGroup", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
            result = _groupRepository.AddMembersToGroup(model.GroupId, model.Members);
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/AddMembersToGroup", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
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
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
            result = _groupRepository.RemoveMembersToGroup(model.GroupId, model.Members);
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/RemoveMembersToGroup", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
        }
        finally
        {
            dispose();
        }

        return Ok(result);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [CustomAuthorize]
    [Route("[action]", Name = "UploadProfilePicture")]
    public async Task<ActionResult> UploadProfilePicture(IFormFile file)
    {
        ResultModel result = new ResultModel();
        string userName = "";
        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;

            var memoryStream = new MemoryStream();
            file.CopyToAsync(memoryStream);
            byte[] bytePicture = memoryStream.ToArray();

            byte[] reducedImage = ReducePictureSize(bytePicture);

            if (bytePicture.Length != 0)
            {
                UserPicture picture = new UserPicture();
                picture.UserId = currentlyLogUser.Id;
                picture.Picture = reducedImage;

                _userRepository.InsertUserPicture(picture);
            }

            result.Success = true;
        }
        catch(Exception ex)
        {
            LogError(userName, "UserController/UploadProfilePicture", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
        }

        return Ok(result);
    }

    [HttpPost]
    [Consumes("application/json")]
    [CustomAuthorize]
    [Route("[action]", Name = "GetAvatar")]
    public async Task<ActionResult> GetAvatar([FromBody] int userId)
    {
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;
            byte[] blobPicture;

            var picture = _userRepository.GetUserPicture(userId);
            if (picture != null)
            {
                blobPicture = picture.Picture;

                return File(blobPicture, "image/png");
            }

            blobPicture = _userRepository.GetEmptyProfilePicture();
            return File(blobPicture, "image/png");
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/GetAvatar", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
        }
        finally {
            dispose();
        } 
    }

    private byte[] ReducePictureSize(byte[] bytes)
    {
        using var memoryStream = new MemoryStream(bytes);
        using var image = Image.Load(memoryStream);
        image.Mutate(x => x.Resize(150, 150));
        using var outputStream = new MemoryStream();
        image.Save(outputStream, new PngEncoder() /*or another encoder*/);
        return outputStream.ToArray();
    }

    [HttpGet]
    [CustomAuthorize]
    [Route("[action]", Name = "RemoveProfilePicture")]
    public async Task<ActionResult> RemoveProfilePicture(int userId)
    {
        ResultModel result = new ResultModel();
        string userName = "";

        try
        {
            var currentlyLogUser = HttpContext.Items["User"] as UserIdentity;
            userName = currentlyLogUser.Username;

            _userRepository.DeleteProfilePicture(userId);
            result.Success = true;
        }
        catch (Exception ex)
        {
            LogError(userName, "UserController/RemoveProfilePicture", ex);
            _telemetry.TrackException(ex);
            return StatusCode(500);
        }
        finally
        {
            dispose();
        }

        return Ok(result);
    }

    private void LogError(string userName, string action, Exception ex)
    {
        var errorLog = LogMaker.MakeLog(userName, action, ex);
        _activityLoggSql.LogErrorToDb(errorLog);
    }

    private void dispose()
    {
        _userRepository.Dispose();
        _groupRepository.Dispose();
    }
}
