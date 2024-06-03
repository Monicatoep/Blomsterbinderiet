using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;

namespace Blomsterbinderiet.test
{
    [TestClass]
    public class UserServiceTest
    {
        DbGenericService<User> dbGeneric = new DbGenericService<User>();
        UserService service = new UserService(new DbGenericService<User>());
        User User;

        [TestInitialize]
        public void Initialize()
        {
            User = new User("TestUser", "123", "Customer", "unittest@gmail.com", "12345678", "Test 1, 1000 testby");
            service.AddUserAsync(User);
        }

        [TestMethod]
        public async Task DisableUserTest()
        {
            // Arrange
            List<User> testusers = service.GetAllUsersAsync().Result.Where(user => user.Email == User.Email).ToList();
            User testUser = testusers.Single();
            
            // Act
            await service.DeactivateUserAsync(testUser.ID);

            // Assert
            User user = await service.GetUserByIdAsync(testUser.ID);
            Assert.AreEqual(user.State, "Deaktiveret");
            Assert.AreEqual(testUser.Name, user.Name);
            Assert.AreEqual(testUser.Email, user.Email);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbGeneric.DeleteObjectAsync(User);
        }
    }
}