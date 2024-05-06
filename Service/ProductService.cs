using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
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

        //public async Task UpdateProductAsync(Product product, IEnumerable<string> updatedProperties)
        //{
        //    await DbService.UpdateObjectAsync(product, updatedProperties);
        //}
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
                dataToBeSorted.Reverse();
            }
            return dataToBeSorted;
        }

        public async Task<IEnumerable<Models.Product>> GetAllProductsIncludeKeywordsAsync()
        {
            return await DbService.GetObjectsAsync(nameof(Models.Product.Keywords));
        }

        public async Task<IEnumerable<Models.Product>> GetAllProductsFilteredAndSorted(string colour, double? price1, double? price2, string keywordNameSearch, bool showDisabled, string sortProperty, bool largeToSmall)
        {

            IEnumerable<Models.Product> Products = await GetAllProductsIncludeKeywordsAsync();

            if (colour != null)
            {
                Products = Products.Where(p => p.Colour.ToLower().Contains(colour.ToLower()));
            }

            if (price1 != null || price2 != null)
            {
                int min = Math.Min(Convert.ToInt32(price1), Convert.ToInt32(price2));
                int maks = Math.Max(Convert.ToInt32(price1), Convert.ToInt32(price2));
                Products = Products.Where(p => p.Price >= min && p.Price <= maks);
            }

            if (keywordNameSearch != null)
            {
                Products = Products.Where(p => p.Keywords.Any(k => k.Name.ToLower().Contains(keywordNameSearch.ToLower())));
            }

            if (!showDisabled)
            {
                Products = from product in Products
                           where product.Disabled == false
                           select product;
            }

            Sort(Products, sortProperty, largeToSmall);
            Products.OrderByDescending(p => p.Disabled);
            return Products;
        }

        public async Task<IEnumerable<Models.Product>> GetAllProductsStandardFilterAndSort()
        {
            return (await GetAllProductsAsync()).Where(p => p.Disabled == false).OrderBy(p => p.Name);
        }
    }
}
