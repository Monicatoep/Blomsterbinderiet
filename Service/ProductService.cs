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
    /// This class is responsible for most if not all things related to <see cref="Product"/>
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// A reference to the DbGenericService object handling CRUD methods to the database for the Product objects 
        /// </summary>
        private DbGenericService<Models.Product> DbService { get; set; }
        public List<Product> Products { get; set; }

        /// <summary>
        /// Constructor for ProductService it must be dependency injected an object of type DbGenericService<Product></Product>
        /// </summary>
        /// <param name="dbService">An object</param>
        public ProductService(DbGenericService<Product> dbService)
        {
            DbService = dbService;
        }

        /// <summary>
        /// Method <c>GetProductByIdAsync</c> calls the <c>GetObjectByIdAsync</c> in the DbGenericService class.
        /// <seealso cref="DbGenericService{T}.GetObjectByIdAsync"/>
        /// </summary>
        /// <param name="id">The primary key of a tuple</param>
        /// <returns>A Task object containing a Product object, it is null if no tuple in the database had a matching primary key</returns>
        /// <remarks>
        /// If no tuple, with the given input value of the "id" parameter as a primarykey,
        /// exists in the database then a null object is returned
        /// </remarks>
        /// 
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return DbService.GetObjectByIdAsync(id).Result;
        }

        /// <summary>
        /// Retrieves all tuples in the Products table on the database
        /// </summary>
        /// <returns>A task object containing an IEnumerable containing Product objects</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await DbService.GetObjectsAsync();
        }

        /// <summary>
        /// Updates the tuple, with a primary key which matches the one <br />
        /// on the supplied Product object, in the database 
        /// </summary>
        /// <param name="product">A product object whose properties get sent to the database</param>
        /// <returns>A Task object which can be used to tell when a operation is done</returns>
        public async Task UpdateProductAsync(Product product)
        {
            await DbService.UpdateObjectAsync(product);
        }

        /// <summary>
        /// Adds the given Product object to the database
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns a task object representing the Add operation</returns>
        public async Task AddProductAsync(Product product)
        {
            await DbService.AddObjectAsync(product);
        }

        /// <summary>
        /// Sets the <c>Disabled</c> property to <c>true</c>, on a product object specified by the primary key
        /// Test <see cref="DbGenericService{T}"/>
        /// </summary>
        /// <param name="id">Primary key of a product tuple/object</param>
        /// <returns>A task object</returns>
        public async Task DisableProductAsync(int id)
        {
            Product productToBeDisabled = DbService.GetObjectByIdAsync(id).Result;
            productToBeDisabled.Disabled = true;
            await DbService.UpdateObjectAsync(productToBeDisabled);
            Products = (await GetAllProductsAsync()).ToList();
        }

        /// <summary>
        /// Sets the <c>Disabled</c> property to <c>false</c>, on a product object specified by the primary key
        /// </summary>
        /// <param name="id">Primary key of a product tuple/object</param>
        /// <returns>A task object</returns>
		public async Task ReenableProductAsync(int id)
		{
			Product productToBeReenabled = DbService.GetObjectByIdAsync(id).Result;
			productToBeReenabled.Disabled = false;
			await DbService.UpdateObjectAsync(productToBeReenabled);
            Products = (await GetAllProductsAsync()).ToList();
        }

        /// <summary>
        /// This method orders the input IEnumerable by a name-specified property
        /// </summary>
        /// <param name="dataToBeSorted">The IEnumerable of Products to be sorted</param>
        /// <param name="property">Name of the Property which the IEnumerable should be ordered by</param>
        /// <param name="largeToSmall">Optional parameter, true if the ordering of the data is reversed</param>
        /// <returns>The input IEnumerable but ordered by the name-specified property</returns>
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
        /// A method like the <see cref="GetAllProductsAsync"/> but this method also includes the navigation property <see cref="Product.Keywords"/>
        /// </summary>
        /// <returns>A IEnumerable of Product objects whose <c>Keyword</c> navigation property has been included</returns>
        public async Task<IEnumerable<Models.Product>> GetAllProductsIncludeKeywordsAsync()
        {
            return await DbService.GetObjectsAsync(nameof(Models.Product.Keywords));
        }

        /// <summary>
        /// This method returns a IEnumerable of Product objects where Keywords have been included in the navigation property <see cref="Product.Keywords"/>.
        /// <para>Before the IEnumerable is returned its content gets filtered based on the parameters</para>
        /// <para>If a parameter is null then its filter is skipped</para>
        /// <para>Except for the price filter which is only skipped when both price1 and price2 is null</para>
        /// </summary>
        /// <param name="colour">Colour which the <see cref="Product.Colour"/> property must be</param>
        /// <param name="price1"></param>
        /// <param name="price2"></param>
        /// <param name="keywordNameSearch"></param>
        /// <param name="showDisabled"></param>
        /// <param name="sortProperty"></param>
        /// <param name="largeToSmall"></param>
        /// <returns>
        /// An IEnumerable of Products including the Keyword navigation property 
        /// which has been filtered and sorted according to the given parameters.
        /// </returns>
        /// <remarks>
        /// If either price1 or price2 is not null then Products only get included 
        /// if they are equal to either or between price1 and price2 if price1 is 
        /// null but price2 isn't then price1 will act as 0.
        /// </remarks>
        public async Task<IEnumerable<Models.Product>> GetAllProductsFilteredAndSorted(string colour, double? price1, double? price2, string keywordNameSearch, bool showDisabled, string sortProperty, bool largeToSmall)
        {
            IEnumerable<Models.Product> Products = await GetAllProductsIncludeKeywordsAsync();

            if (colour != null)
            {
                Products = Products.Where(p => p.Colour.ToLower().Contains(colour.ToLower()));
            }

            if (price1 != null || price2 != null)
            {
                double min = Math.Min(Convert.ToDouble(price1), Convert.ToDouble(price2));
                double maks = Math.Max(Convert.ToDouble(price1), Convert.ToDouble(price2));
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
        /// When Filter properties are not accessible then call this method. <br />
        /// See <see cref="GetAllProductsFilteredAndSorted"/>
        /// </summary>
        /// <returns>
        /// An IEnumerable of Product objects where <c>Disabled == false</c> is not 
        /// included and it is ordered by the <c>Name</c> property
        /// </returns>
        public async Task<IEnumerable<Models.Product>> GetAllProductsStandardFilterAndSort()
        {
            return (await GetAllProductsAsync()).Where(p => p.Disabled == false).OrderBy(p => p.Name);
        }
    }
}
