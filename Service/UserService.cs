using Blomsterbinderiet.Models;

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

        public async Task AddUserAsync(User user)
        {
            Users.Add(user);
            await DbService.AddObjectAsync(user);
        }


    }
}
