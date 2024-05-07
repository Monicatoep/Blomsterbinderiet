using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
namespace Blomsterbinderiet.Service
{
    /// <summary>
    /// The <c>ProductService</c> class is responsible for most if not all things related to <c>Product</c>
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// 
        /// </summary>
        private DbGenericService<Models.Product> DbService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbService"></param>
        public ProductService(DbGenericService<Product> dbService)
        {
            DbService = dbService;
            Products = dbService.GetObjectsAsync().Result.ToList();
        }

        /// <summary>
        /// Method <c>GetProductByIdAsync</c> calls the <c>GetObjectByIdAsync</c> in the DbGenericService class.
        /// </summary>
        /// <param name="id">The primary key of a tuple</param>
        /// <returns>A Task object containing a Product object, it is null if no tuple in the database had a matching primary key</returns>
        /// <remarks>
        /// If no tuple, with the given input value of the "id" parameter as a primarykey,
        /// exists in the database then a null object is returned
        /// </remarks>
        /// <seealso cref="GetObjectByIdAsync"/>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return DbService.GetObjectByIdAsync(id).Result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>A task object containing an IEnumerable containing Product objects</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await DbService.GetObjectsAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task UpdateProductAsync(Product product)
        {
            await DbService.UpdateObjectAsync(product);
        }

        public async Task AddProductAsync(Product product)
        {
            Products.Add(product);
            await DbService.AddObjectAsync(product);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DisableProductAsync(int id)
        {
            Product productToBeDisabled = DbService.GetObjectByIdAsync(id).Result;
            productToBeDisabled.Disabled = true;
            await DbService.UpdateObjectAsync(productToBeDisabled);
            Products = (await GetAllProductsAsync()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public async Task ReenableProductAsync(int id)
		{
			Product productToBeReenabled = DbService.GetObjectByIdAsync(id).Result;
			productToBeReenabled.Disabled = false;
			await DbService.UpdateObjectAsync(productToBeReenabled);
            Products = (await GetAllProductsAsync()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToBeSorted"></param>
        /// <param name="property"></param>
        /// <param name="largeToSmall"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Models.Product>> GetAllProductsIncludeKeywordsAsync()
        {
            return await DbService.GetObjectsAsync(nameof(Models.Product.Keywords));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="price1"></param>
        /// <param name="price2"></param>
        /// <param name="keywordNameSearch"></param>
        /// <param name="showDisabled"></param>
        /// <param name="sortProperty"></param>
        /// <param name="largeToSmall"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Models.Product>> GetAllProductsStandardFilterAndSort()
        {
            return (await GetAllProductsAsync()).Where(p => p.Disabled == false).OrderBy(p => p.Name);
        }
    }
}
