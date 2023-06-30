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
            id = generateUniqueID(),
            username = newUser.username,
            email = newUser.email,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.password),  // hash password using BCrypt
            measurements = new List<Measurement>()  // empty list of measurements at first
        };

        allUsers.Add(user);
        return RegistrationResult.Success;
    }

    public AccountDeletionResult DeleteUser(User user)
    {
        foreach (var u in allUsers)
        {
            // if the user is found
            if (u.username == user.username)
            {
                // if the user is logged in (has a JWT token)
                if (user.token != "")
                {
                    allUsers.Remove(user);
                    return AccountDeletionResult.Success;
                }
                return AccountDeletionResult.UserNotLoggedIn;
            }
        }
        return AccountDeletionResult.UserNotFound;
    }
    
    public enum RegistrationResult
    {
        Success,
        UsernameExists,
        InvalidEmail,
        PasswordTooShort
    }

    public enum AccountDeletionResult
    {
        Success,
        UserNotFound,
        UserNotLoggedIn
    }
}