using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
namespace Blomsterbinderiet.Service
{
    public class ProductService
    {
        public List<Product> Products { get; set; }
        private DbGenericService<Models.Product> DbService { get; set; }
        public ProductService(DbGenericService<Product> dbService)
        {
            DbService = dbService;
            Products = dbService.GetObjectsAsync().Result.ToList();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return DbService.GetObjectByIdAsync(id).Result;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
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
            Products = (await GetAllProductsAsync()).ToList();
        }

		public async Task ReenableProductAsync(int id)
		{
			Product productToBeReenabled = DbService.GetObjectByIdAsync(id).Result;
			productToBeReenabled.Disabled = false;
			await DbService.UpdateObjectAsync(productToBeReenabled);
            Products = (await GetAllProductsAsync()).ToList();
        }

        public IEnumerable<Product> Sort(IEnumerable<Product> dataToBeSorted,string property, bool largeToSmall=false)
        {
            switch(property)
            {
                case nameof(Product.Name):
                    dataToBeSorted.OrderBy(p => p.Name);
                    break;
                case nameof(Product.Price):
                    dataToBeSorted.OrderBy(p => p.Price);
                    break;
                case nameof(Product.Colour):
                    dataToBeSorted.OrderBy(p => p.Colour);
                    break;
                case nameof(Product.Description):
                    dataToBeSorted.OrderBy(p => p.Description);
                    break;
            }
            if(largeToSmall)
            {
                dataToBeSorted = dataToBeSorted.Reverse();
            }
            return dataToBeSorted;
        }

        public async Task<IEnumerable<Models.Product>> GetAllProductsIncludeKeywordsAsync()
        {
            return await DbService.GetObjectsAsync(nameof(Models.Product.Keywords));
        }
    }
}
