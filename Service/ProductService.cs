using Blomsterbinderiet.Models;
namespace Blomsterbinderiet.Service
{
    public class ProductService
    {
        public List<Product> Products { get; set; }
        private DbGenericService<Product> DbService { get; set; }
        public ProductService(DbGenericService<Product> dbService)
        {
            DbService = dbService;
            Products = dbService.GetObjectsAsync().Result.ToList();
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
