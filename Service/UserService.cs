using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using System.Data;
using System.Net;
using System.Numerics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Blomsterbinderiet.Service
{
    public class UserService
    {
        private DbGenericService<User> DbService { get; set; }
        public List<User> Users { get; set; }

        public UserService(DbGenericService<User> dbService)
        {
            DbService = dbService;
            Users = dbService.GetObjectsAsync().Result.ToList();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            Users = (await DbService.GetObjectsAsync()).ToList();
            return Users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User user = await DbService.GetObjectByIdAsync(id);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await DbService.GetObjectByIdAsync(Convert.ToInt32(id));
        }

        public User? GetUserByEmail(string email)
        {
            foreach (User u in Users)
            {
                if (u.Email.Equals(email))
                { return u; }
            }
            return null;
        }

        public async Task UpdateUserAsync(User user)
        {
            await DbService.UpdateObjectAsync(user);
        }

        public async Task UpdateUserAsync(User user, IEnumerable<string> updatedProperties)
        {
            await DbService.UpdateObjectAsync(user, updatedProperties);
        }

        public async Task AddUserAsync(User user)
        {
            Users.Add(user);
            await DbService.AddObjectAsync(user);
        }

        public IEnumerable<User> SortByName()
        {
            return from user in Users
                   orderby user.Name
                   select user;
        }

        public IEnumerable<User> SortByNameDescending()
        {
            return from user in Users
                   orderby user.Name descending
                   select user;
        }

        public IEnumerable<User> SortByRole()
        {
            return from user in Users
                   orderby user.Role
                   select user;
        }

        public IEnumerable<User> SortByRoleDescending()
        {
            return from user in Users
                   orderby user.Role descending
                   select user;
        }

        public async Task<User?> GetUserByHttpContextAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                string userId = context.User.FindFirst(ClaimTypes.Name)?.Value;
                if (userId != null)
                {
                    return await GetUserByIdAsync(userId);
                }
            }
            return null;
        }

        public async Task DeactivateUserAsync(int id)
        {
            User user = await GetUserByIdAsync(id);
            user.State = "Deaktiveret";
            await UpdateUserAsync(user);
        }
        public async Task<IEnumerable<User>> GetEmployeesAsync()
        {

            return from user in Users
                   where user.Role == "Employee"
                   select user;
        }
        public IEnumerable<Models.User> FilterByStatus(string status)
        {
            return from user in Users
                   where user.State == status
                   select user;
        }
        public IEnumerable<Models.User> FilterByRole(string role)
        {
            return from user in Users
                   where user.Role == role
                   select user;
        }
    }
}
