using InterviewAssignement.Models;
using InterviewAssignment.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace UnitTestProject;

public class AuthTests
{
    private RegisterService _registerService;
    private LoginService _loginService;
    private IConfiguration _configuration;

    [SetUp]
    public void Setup()
    {
        // initialize services
        _configuration = new ConfigurationManager();
        _registerService = new RegisterService();
        _loginService = new LoginService(_configuration);
    }

    [Test]
    public void RegisterUserPasswordTooShort()
    {
        // arrange
        var json = @"
            {
                'username': 'jakob',
                'email': 'j.opresnik@gmail.com',
                'password': '123'
            }";

        var user = JsonConvert.DeserializeObject<UserDto>(json);
        
        // act
        var actualResult = _registerService.RegisterUser(user);
        RegisterService.RegistrationResult expectedResult = RegisterService.RegistrationResult.PasswordTooShort;
        
        // assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void RegisterUserInvalidEmail()
    {
        // arrange
        var json = @"
            {
                'username': 'jakob',
                'email': 'j.opresnikgmail.com',
                'password': '123'
            }";

        var user = JsonConvert.DeserializeObject<UserDto>(json);
        
        // act
        var actualResult = _registerService.RegisterUser(user);
        RegisterService.RegistrationResult expectedResult = RegisterService.RegistrationResult.InvalidEmail;
        
        // assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void LoginUserDoesntExist()
    {
        // arrange
        var json_1 = @"
            {
                'username': 'jakob1',
                'email': 'j.opresnik@gmail.com',
                'password': '123'
            }";
        var json_2 = @"
            {
                'username': 'jakob2',
                'email': 'j.opresnik@gmail.com',
                'password': '123'
            }";

        var user_1 = JsonConvert.DeserializeObject<UserDto>(json_1);
        var user_2 = JsonConvert.DeserializeObject<UserDto>(json_2);
        
        // act
        var allUsers = new List<User>();
        var userHashed = new User()
        {
            username = user_1.username,
            email = user_1.email,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(user_1.password)
        };
        allUsers.Add(userHashed);
        
        var actualResult = _loginService.LoginUser(user_2, allUsers);
        var expectedResult = "User doesn't exist!";
        
        // assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void LogoutUserNotLoggedIn()
    {
        // arrange
        var json = @"
            {
                'username': 'jakob3',
                'email': 'j.opresnik@gmail.com',
                'password': '12345',
                'token': ''
            }";

        var user = JsonConvert.DeserializeObject<User>(json);
        
        
        // act
        var allUsers = new List<User>();
        allUsers.Add(user);
        
        var actualResult = _loginService.LogoutUser(user, allUsers);
        LoginService.LogoutResult expectedResult = LoginService.LogoutResult.UserNotLoggedIn;
        
        // assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void LogoutUserNotFound()
    {
        // arrange
        var json = @"
            {
                'username': 'jakob4',
                'email': 'j.opresnik@gmail.com',
                'password': '12345',
                'token': '123'
            }";

        var user = JsonConvert.DeserializeObject<User>(json);
        
        
        // act
        var allUsers = new List<User>();
        
        var actualResult = _loginService.LogoutUser(user, allUsers);
        LoginService.LogoutResult expectedResult = LoginService.LogoutResult.UserNotFound;
        
        // assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void DeleteUserNotLoggedIn()
    {
        // arrange
        var json = @"
            {
                'username': 'jakob5',
                'email': 'j.opresnik@gmail.com',
                'password': '12345'
            }";

        var user = JsonConvert.DeserializeObject<UserDto>(json);
        if (_registerService.RegisterUser(user) == RegisterService.RegistrationResult.Success)
        {
            User u = new User()
            {
                username = user.username,
                email = user.email,
                passwordHash = BCrypt.Net.BCrypt.HashPassword(user.password),
                token = ""  // not logged in
            };

            // act
            var actualResult = _registerService.DeleteUser(u);
            RegisterService.AccountDeletionResult expectedResult = RegisterService.AccountDeletionResult.UserNotLoggedIn;
        
            // assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
    
    [Test]
    public void DeleteUserNotFound()
    {
        // arrange
        var json = @"
            {
                'username': 'jakob6',
                'email': 'j.opresnik@gmail.com',
                'password': '12345'
            }";

        var user = JsonConvert.DeserializeObject<UserDto>(json);
        User u = new User()
        {
            username = user.username,
            email = user.email,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(user.password),
            token = ""
        };

        // act
        var actualResult = _registerService.DeleteUser(u);
        RegisterService.AccountDeletionResult expectedResult = RegisterService.AccountDeletionResult.UserNotFound;
    
        // assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
}