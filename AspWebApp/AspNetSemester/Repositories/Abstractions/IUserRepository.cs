using AspNetSemester.Models;

namespace AspNetSemester.Repositories.Abstractions;

public interface IUserRepository
{
    bool ValidateUser(string username, string password);
    User GetUserByUsername(string username);
    void AddUser(User user);
}