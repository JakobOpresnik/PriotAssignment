using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using InterviewAssignement.Models;

namespace InterviewAssignment.Services;

public class RegisterService
{
    public static List<User> allUsers = new List<User>();

    private string generateUniqueID()
    {
        int randomNum = new Random().Next();
        Guid guid = Guid.NewGuid();
        string uniqueID = $"{randomNum}-{guid}";
        return uniqueID;
    }

    private bool isValidEmail(string email)
    {
        // regex E-mail pattern
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        bool isValid = Regex.IsMatch(email, pattern);
        return isValid;
    }

    public RegistrationResult RegisterUser(UserDto newUser)
    {
        //string newID = generateUniqueID();
        //newUser.id = newID;

        foreach (var u in allUsers)
        {
            if (u.username == newUser.username)
            {
                return RegistrationResult.UsernameExists;
            }
        }

        if (!isValidEmail(newUser.email)) return RegistrationResult.InvalidEmail;
        if (newUser.password.Length < 5) return RegistrationResult.PasswordTooShort;

        var user = new User
        {
            username = newUser.username,
            email = newUser.email,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.password)  // hash password using BCrypt
        };

        allUsers.Add(user);
        return RegistrationResult.Success;
    }
    
    public enum RegistrationResult
    {
        Success,
        UsernameExists,
        InvalidEmail,
        PasswordTooShort
    }
}