using System.Text.Json;
using InterviewAssignement.Models;
using InterviewAssignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewAssignment.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AuthController :  ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly RegisterService _registerService;
    private readonly LoginService _loginService;

    public AuthController(ILogger<AuthController> logger, RegisterService registerService, LoginService loginService)
    {
        _logger = logger;
        _registerService = registerService;
        _loginService = loginService;
    }


    public IActionResult PostRegister([FromBody] string req)
    {
        var user = JsonSerializer.Deserialize<UserDto>(req);

        var registrationResult = _registerService.RegisterUser(user);
        return registrationResult switch
        {
            RegisterService.RegistrationResult.Success => Ok(user),
            RegisterService.RegistrationResult.UsernameExists => BadRequest("This username already exists!"),
            RegisterService.RegistrationResult.InvalidEmail => BadRequest("This E-mail address is invalid!"),
            RegisterService.RegistrationResult.PasswordTooShort => BadRequest("Your password is too short (min. 5 characters)!"),
            _ => Ok()   // default case
        };
    }

    public IActionResult PostLogin([FromBody] string req)
    {
        var user = JsonSerializer.Deserialize<UserDto>(req);

        var allUsers = RegisterService.allUsers;
        
        var loginResult = _loginService.LoginUser(user, allUsers);
        if (loginResult == "Incorrect password!" || loginResult == "User doesn't exist!")
        {
            return BadRequest(loginResult);
        }
        // JWT token is returned
        return Ok(loginResult);
    }

    public IActionResult PostLogout([FromBody] string req)
    {
        var user = JsonSerializer.Deserialize<User>(req);

        var allUsers = RegisterService.allUsers;

        var logoutResult = _loginService.LogoutUser(user, allUsers);
        return logoutResult switch
        {
            LoginService.LogoutResult.Success => Ok(user),
            LoginService.LogoutResult.UserNotLoggedIn => BadRequest("User was not logged in!"),
            LoginService.LogoutResult.UserNotFound => BadRequest("This user does not exist!"),
            _ => Ok()
        };
    }
    
    public IActionResult DeleteUser([FromBody] string req)
    {
        var user = JsonSerializer.Deserialize<User>(req);

        var deletionResult = _registerService.DeleteUser(user);
        return deletionResult switch
        {
            RegisterService.AccountDeletionResult.Success => Ok(user),
            RegisterService.AccountDeletionResult.UserNotLoggedIn => BadRequest("User was not logged in!"),
            RegisterService.AccountDeletionResult.UserNotFound => BadRequest("This user does not exist!"),
            _ => Ok()
        };
    }
}