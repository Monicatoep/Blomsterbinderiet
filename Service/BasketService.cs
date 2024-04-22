using Blomsterbinderiet.Models;
namespace Blomsterbinderiet.Service
{
    public class BasketService
    {
        private DbGenericService<Product> DbService { get; set; }
        public BasketService(DbGenericService<Product> dbService)
        {
            DbService = dbService;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return DbService.GetObjectByIdAsync(id).Result;
        }
    }
}
