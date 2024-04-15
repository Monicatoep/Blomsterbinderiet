using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class UserService
    {
        public List<User> Users { get; set; }

        //private JsonFileService<User> JsonFileService { get; set; }
        private DbGenericService<User> DbService { get; set; }
        public UserService(DbGenericService<User> dbService)
        {
            DbService =dbService;
        ;
            Users = dbService.GetObjectsAsync().Result.ToList();
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

        public void AddUser(User user)
        {
            Users.Add(user);
            DbService.AddObjectAsync(user);
        }


    }
}
