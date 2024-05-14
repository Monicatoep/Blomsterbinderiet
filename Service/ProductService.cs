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
    /// This class is responsible for handling <see cref="Product"/> objects. It contains filter-, sort- and CRUD methods.
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// <c>DbService</c> represents a reference to the DbGenericService object handling CRUD calls to the database for the <see cref="Product"/> objects 
        /// </summary>
        private ProductDbService DbService { get; set; }
        public List<Product> Products { get; set; }

        /// <summary>
        /// Constructor for the <c>ProductService</c> class. 
        /// </summary>
        /// <remarks>
        /// The constructor must be dependency injected an 
        /// object of type <see cref="DbGenericService{T=Product}"/> where T is a type of Product
        /// </remarks>
        /// <param name="dbService">Reference to a <see cref="DbGenericService{T=Product}"/> object where T is Product</param>
        public ProductService(ProductDbService dbService)
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

        public async Task<Product?> GetProductIncludingKeywordsByID(int id)
        {
            return await DbService.GetProductIncludingKeywordsByID(id);
        }

        /// <summary>
        /// Retrieves all tuples/rows in the "Products" table on the database
        /// </summary>
        /// <returns>A task object containing an IEnumerable containing Product objects</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await DbService.GetObjectsAsync();
        }

        /// <summary>
        /// Updates the tuple, in the "Products" entity/table, where the primary key matches the primary key <br />
        /// on the supplied <see cref="Product"/> object in the database 
        /// </summary>
        /// <param name="product">A product object whose properties get sent to the database</param>
        /// <returns>A Task object which can be used to tell when a operation is done</returns>
        public async Task UpdateProductAsync(Product product)
        {
            await DbService.UpdateObjectAsync(product);
        }

        public async Task UpdateProductAsync(Product product, IEnumerable<int> idsOfKeywords)
        {
            await DbService.UpdateObjectAsync(product, idsOfKeywords);
        }

        /// <summary>
        /// Adds the passed <see cref="Product"/> object to the "Products" entity/table in the database
        /// </summary>
        /// <remarks>
        /// The Product object passed to this method must have its primary key, "ID", set to null.
        /// </remarks>
        /// <param name="product">An object of type Product</param>
        /// <returns>Returns a task object representing the Add operation</returns>
        public async Task AddProductAsync(Product product)
        {
            await DbService.AddObjectAsync(product);
        }

        public async Task AddProductAsync(Product product, int[] idsOfKeywords)
        {
            await DbService.AddProductAsync(product, idsOfKeywords);
        }

        /// <summary>
        /// Sets the <c>Disabled</c> property to <c>true</c>, on a product object specified by the primary key.
        /// And updates the "Products" entity/table in the database
        /// </summary>
        /// <param name="id">Primary key of a product tuple/object</param>
        /// <returns>A task object representing the DisableProduct operation</returns>
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
        /// <returns>A task object representing the re-enable operation</returns>
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
        /// <param name="largeToSmall">Optional parameter, true if the ordering of the data should reversed before it's returned</param>
        /// <returns>The input IEnumerable but ordered by the name-specified property</returns>
        public IEnumerable<Product> Sort(IEnumerable<Product> dataToBeSorted,string property, bool largeToSmall=false)
        {
            switch(property)
            {
                case nameof(Product.Name):
                    dataToBeSorted = dataToBeSorted.OrderBy(p => p.Name);
                    break;
                case nameof(Product.Price):
                    dataToBeSorted = dataToBeSorted.OrderBy(p => p.Price);
                    break;
                case nameof(Product.Colour):
                    dataToBeSorted = dataToBeSorted.OrderBy(p => p.Colour);
                    break;
                case nameof(Product.Description):
                    dataToBeSorted = dataToBeSorted.OrderBy(p => p.Description);
                    break;
            }
            if(largeToSmall)
            {
                dataToBeSorted = dataToBeSorted.Reverse();
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
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="keywordNameSearch"></param>
        /// <param name="showDisabled"></param>
        /// <returns>
        /// An IEnumerable of Products including the Keyword navigation property 
        /// which has been filtered and sorted according to the given parameters.
        /// </returns>
        /// <remarks>
        /// If either price1 or price2 is not null then Products only get included 
        /// if they are equal to either or between price1 and price2 if price1 is 
        /// null but price2 isn't then price1 will act as 0.
        /// </remarks>
        public async Task<IEnumerable<Models.Product>> GetAllProductsFiltered(string name, string colour, double? minPrice, double? maxPrice, string keywordNameSearch, bool showDisabled)
        {
            IEnumerable<Models.Product> Products = await GetAllProductsIncludeKeywordsAsync();

            if (name != null)
            {
                name = name.ToLower();
                Products = Products.Where(p => p.Name.ToLower().Contains(name) || name.Contains(p.Name.ToLower()));
            }

            if (colour != null)
            {
                Products = Products.Where(p => p.Colour.ToLower().Contains(colour.ToLower()));
            }

            if (minPrice != null && maxPrice != null)
            {
                Products = Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            } else if(minPrice != null) 
            {
                Products = Products.Where(p => p.Price >= minPrice);
            } else if(maxPrice != null) 
            {
                Products = Products.Where(p => p.Price <= maxPrice);
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
            return Products;
        }

        /// <summary>
        /// This method returns all tuples/rows in the "Products" entity excluding the disabled, or rather expired, products
        /// which should not be shown to the customer.<br />
        /// The returned IEnumerable containing the non-expired products gets sorted alphabetically by their name.
        /// </summary>
        /// <returns>
        /// An IEnumerable of Product objects containing non-disabled products which is ordered by the <c>Name</c> property
        /// </returns>
        public async Task<IEnumerable<Models.Product>> GetAllProductsStandardFilterAndSort()
        {
            return (await GetAllProductsAsync()).Where(p => p.Disabled == false).OrderBy(p => p.Name);
        }
    }
}
