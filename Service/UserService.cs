using Blomsterbinderiet.Models;
using System.Data;
using System.Security.Claims;

namespace Blomsterbinderiet.Service
{
    public class UserService
    {
        private DbGenericService<User> UserDbService { get; set; }
        public List<User> Users { get; set; }

        public UserService(DbGenericService<User> dbService)
        {
            UserDbService = dbService;
            Users = dbService.GetObjectsAsync().Result.ToList();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            Users = (await UserDbService.GetObjectsAsync()).ToList();
            return Users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User user = await UserDbService.GetObjectByIdAsync(id);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await UserDbService.GetObjectByIdAsync(Convert.ToInt32(id));
        }

        public async Task UpdateUserAsync(User user)
        {
            await UserDbService.UpdateObjectAsync(user);
        }

        public async Task UpdateUserAsync(User user, IEnumerable<string> updatedProperties)
        {
            await UserDbService.UpdateObjectAsync(user, updatedProperties);
        }

        public async Task AddUserAsync(User user)
        {
            Users.Add(user);
            await UserDbService.AddObjectAsync(user);
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

        public IEnumerable<User> FilterByStatus(string status)
        {
            return from user in Users
                   where user.State == status
                   select user;
        }

        public IEnumerable<User> FilterByRole(string role)
        {
            return from user in Users
                   where user.Role == role
                   select user;
        }
    }
}
