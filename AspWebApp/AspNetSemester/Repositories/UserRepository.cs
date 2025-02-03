using System.Security.Cryptography;
using AspNetSemester.Contexts;
using AspNetSemester.Models;
using AspNetSemester.Repositories.Abstractions;

namespace AspNetSemester.Repositories;
public class UserRepository : IUserRepository
{
    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? GetUserByUsername(string username)
    {
        return _context.Users.FirstOrDefault(u => u.Username == username);
    }
    public User? GetUserById(int id)
    {
        return _context.Users.Find(id);
    }
    public bool ValidateUser(string username, string password)
    {
        var user = GetUserByUsername(username);
        var hashPassword = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password)));
        if (user != null && user.Password == hashPassword)
        {
            return true;
        }
        return false;
    }

}