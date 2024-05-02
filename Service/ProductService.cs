using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
namespace Blomsterbinderiet.Service
{
    public class ProductService : ServiceGeneric<Product>
    {
        public List<Product> Products { get; set; }
        public ProductService(DbGenericService<Product> dbService) : base(dbService)
        {
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

        public async Task UpdateProductAsync(Product product)
        {
            await DbService.UpdateObjectAsync(product);
        }

        public async Task UpdateProductAsync(Product product, IEnumerable<string> updatedProperties)
        {
            await DbService.UpdateObjectAsync(product, updatedProperties);
        }
        public async Task AddProductAsync(Product product)
        {
            Products.Add(product);
            await DbService.AddObjectAsync(product);
        }

        public async Task DisableProductAsync(int id)
        {
            Product productToBeDisabled = DbService.GetObjectByIdAsync(id).Result;
            productToBeDisabled.Disabled = true;
            await DbService.UpdateObjectAsync(productToBeDisabled);
            Products = (await GetProductsAsync()).ToList();
        }

		public async Task ReenableProductAsync(int id)
		{
			Product productToBeReenabled = DbService.GetObjectByIdAsync(id).Result;
			productToBeReenabled.Disabled = false;
			await DbService.UpdateObjectAsync(productToBeReenabled);
            Products = (await GetProductsAsync()).ToList();
        }
    }
}
