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
            return await DbService.GetObjectsAsync();
        }

        public void UpdateProduct(Product product)
        {
            DbService.UpdateObjectAsync(product);
        }

        public void UpdateProduct(Product product, IEnumerable<string> updatedProperties)
        {
            DbService.UpdateObjectAsync(product, updatedProperties);
        }

        public async Task DisableProducAsync(int id)
        {
            Product productToBeDisabled = DbService.GetObjectByIdAsync(id).Result;
            productToBeDisabled.Disabled = true;
            await DbService.UpdateObjectAsync(productToBeDisabled);
        }
    }
}
