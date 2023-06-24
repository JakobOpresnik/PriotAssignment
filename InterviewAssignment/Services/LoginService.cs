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
                return token;
            }
            
            if (!BCrypt.Net.BCrypt.Verify(user.password, u.passwordHash))
            {
                return "Incorrect password!";
            }
        }
        return "User doesn't exist!";
    }
    
}