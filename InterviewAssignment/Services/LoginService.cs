using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InterviewAssignement.Models;
using Microsoft.IdentityModel.Tokens;

namespace InterviewAssignment.Services;

public class LoginService
{
    private readonly IConfiguration _configuration;

    public LoginService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    private string generateJwt(UserDto user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.username)
        };

        string key = "priot-back-end-secret-key-with-sufficient-length";
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: credentials
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }   
    
    public string LoginUser(UserDto user, List<User> allUsers)
    {
        foreach (var u in allUsers)
        {
            if (u.username == user.username)
            {
                var token = generateJwt(user);
                u.token = token;
                return token;
            }
            
            if (!BCrypt.Net.BCrypt.Verify(user.password, u.passwordHash))
            {
                return "Incorrect password!";
            }
        }
        return "User doesn't exist!";
    }

    public LogoutResult LogoutUser(User user, List<User> allUsers)
    {
        foreach (var u in allUsers)
        {
            // if the user is found
            if (u.username == user.username)
            {
                // if the user is logged in (has a JWT token)
                if (u.token != "")
                {
                    // reset the user's JWT token
                    u.token = "";
                    return LogoutResult.Success;
                }
                return LogoutResult.UserNotLoggedIn;
            }
        }
        return LogoutResult.UserNotFound;
    }

    public enum LogoutResult
    {
        Success,
        UserNotFound,
        UserNotLoggedIn
    }
    
}