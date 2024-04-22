using Blomsterbinderiet.Models;
namespace Blomsterbinderiet.Service
{
    public class ProductService
    {
        private DbGenericService<Product> DbService { get; set; }
        public ProductService(DbGenericService<Product> dbService)
        {
            DbService = dbService;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return DbService.GetObjectByIdAsync(id).Result;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return DbService.GetObjectsAsync().Result;
        }
    }
}
