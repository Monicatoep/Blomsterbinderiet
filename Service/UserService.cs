using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using System.Data;
using System.Net;
using System.Numerics;

namespace Blomsterbinderiet.Service
{
    public class UserService
    {
        public List<User> Users { get; set; }

        private DbGenericService<User> DbService { get; set; }
        public UserService(DbGenericService<User> dbService)
        {
            DbService =dbService;
        ;
            Users = dbService.GetObjectsAsync().Result.ToList();
        }

        public List<User> GetAllUsers()
        {
            Users = DbService.GetObjectsAsync().Result.ToList();
            return Users;
        }

        public User GetUserByIdAsync(int id)
        {
            User user = DbService.GetObjectByIdAsync(id).Result;
            return user;
        }


        public User GetUserByEmail(string email)
        {
            foreach (User u in Users)
            {
                if (u.Email.Equals(email))
                { return u; }
            }
            return null;
        }

        public void UpdateUser(User user)
        {
            DbService.UpdateObjectAsync(user);
        }

        public void UpdateUser(User user, IEnumerable<string> updatedProperties)
        {
            DbService.UpdateObjectAsync(user, updatedProperties);
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


    }
}
