using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    /// <summary>
    /// This class is responsible for handling <see cref="Product"/> objects.
    /// It contains filter-, sort- and CRUD methods.
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// <c>ProductDbService</c> represents a reference to the DbGenericService object handling CRUD calls to the database for the <see cref="Product"/> objects 
        /// </summary>
        private ProductDbService ProductDbService { get; set; }

        /// <summary>
        /// A list of product object references
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        /// Constructor for the <c>ProductService</c> class. 
        /// </summary>
        /// <remarks>
        /// The constructor must be dependency injected a 
        /// reference to an object of type <see cref="Service.ProductDbService"/>
        /// </remarks>
        /// <param name="dbService">A reference</param>
        public ProductService(ProductDbService dbService)
        {
            ProductDbService = dbService;
        }

        /// <summary>
        /// Retrieves a Product object from the database using the argument value of id
        /// </summary>
        /// <param name="id">The primary key of the Product tuple that gets returned</param>
        /// <returns>A Task object containing a Product object, it is null if no tuple in the database had a matching primary key</returns>
        /// <remarks>
        /// If no tuple, with the given input value of the "id" parameter as a primary key,
        /// exists in the database then a null object is returned
        /// </remarks>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await ProductDbService.GetObjectByIdAsync(id);
        }

        /// <summary>
        /// This method returns a reference to a Product object which 
        /// has its navigation property <see cref="Product.Keywords"/> filled
        /// </summary>
        /// <param name="id">This represents the primary key of the Product</param>
        /// <returns>A Task object representing the operation of retrieving the Product including its keywords from the database</returns>
        public async Task<Product?> GetProductIncludingKeywordsByIDAsync(int id)
        {
            return await ProductDbService.GetProductIncludingKeywordsByIDAsync(id);
        }

        /// <summary>
        /// Retrieves all tuples/rows in the "Products" table on the database
        /// </summary>
        /// <returns>A task object containing an IEnumerable containing Product objects</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await ProductDbService.GetObjectsAsync();
        }

        /// <summary>
        /// Updates the tuple, in the "Products" entity/table, where the primary key matches the primary key <br />
        /// on the supplied <see cref="Product"/> object in the database 
        /// </summary>
        /// <param name="product">A product object whose properties get sent to the database</param>
        /// <param name="idsOfKeywords">The ids, the primary key, of the keywords which the product should only contain after the update</param>
        /// <returns>A Task object which can be used to tell when a operation is done</returns>
        public async Task UpdateProductAsync(Product product, IEnumerable<int> idsOfKeywords)
        {
            await ProductDbService.UpdateProductAsync(product, idsOfKeywords);
        }

        /// <summary>
        /// Adds the provided object, of type Product, to the "Products" entity/table in the database
        /// </summary>
        /// <remarks>
        /// The Product object passed to this method must have its primary key, "ID", set to null.
        /// </remarks>
        /// <param name="product">An object of type Product which should be inserted into the database</param>
        /// <param name="idsOfKeywords">An array of integers which represent the ids of the keywords
        /// which the Product should have in the database</param>
        /// <returns>Returns a task object representing the Add operation</returns>
        public async Task AddProductAsync(Product product, int[] idsOfKeywords)
        {
            await ProductDbService.AddProductAsync(product, idsOfKeywords);
        }

        /// <summary>
        /// Sets <see cref="Product.Disabled"/> to <c>true</c>, on the product object specified by the primary key.
        /// And updates the "Products" entity/table in the database
        /// </summary>
        /// <param name="id">Primary key of a product tuple/object</param>
        /// <returns>A task object representing the DisableProduct operation</returns>
        public async Task DisableProductAsync(int id)
        {
            Product productToBeDisabled = await ProductDbService.GetObjectByIdAsync(id);
            productToBeDisabled.Disabled = true;
            await ProductDbService.UpdateObjectAsync(productToBeDisabled);
            Products = (await GetAllProductsAsync()).ToList();
        }

        /// <summary>
        /// Sets <see cref="Product.Disabled"/> to <c>false</c>, on the product object specified by the primary key.
        /// And updates the "Products" entity/table in the database
        /// </summary>
        /// <param name="id">Primary key of a product tuple/object</param>
        /// <returns>A task object representing the re-enable operation</returns>
		public async Task ReenableProductAsync(int id)
		{
			Product productToBeReenabled = await ProductDbService.GetObjectByIdAsync(id);
			productToBeReenabled.Disabled = false;
			await ProductDbService.UpdateObjectAsync(productToBeReenabled);
            Products = (await GetAllProductsAsync()).ToList();
        }

        /// <summary>
        /// This method orders the input IEnumerable by a name-specified property.
        /// If the <c>property</c> argument isn't "Name", "Price" or "Colour" then
        /// the IEnumerable will not get sorted and though the ordering will be reversed
        /// if the argument to <c>largeToSmall</c> is true.
        /// </summary>
        /// <param name="dataToBeSorted">The IEnumerable of Products to be sorted</param>
        /// <param name="property">Name of the Property which the IEnumerable should be ordered by</param>
        /// <param name="largeToSmall">Optional parameter, true if the ordering of the data should reversed before it's returned</param>
        /// <returns>The input IEnumerable but ordered by the name-specified property and optionally in descending order</returns>
        public IEnumerable<Product> Sort(IEnumerable<Product> dataToBeSorted, string property, bool largeToSmall=false)
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
        public async Task<IEnumerable<Product>> GetAllProductsIncludeKeywordsAsync()
        {
            return await ProductDbService.GetObjectsAsync(nameof(Product.Keywords));
        }

        /// <summary>
        /// This method returns a IEnumerable of Product objects where Keywords have been included in the navigation property <see cref="Product.Keywords"/>.
        /// <para>Before the IEnumerable is returned its content gets filtered based on the parameters</para>
        /// <para>If an argument is null then its filter is skipped</para>
        /// <para>Except for the price filter which is only skipped when both minePrice and maxPrice is null</para>
        /// </summary>
        /// <param name="searchString">A string the products name, colour or any of is keywords names must contain</param>
        /// <param name="minPrice">Minimum price a products price property be must equal to or larger than</param>
        /// <param name="maxPrice">Maksimum price a products price property be must equal to or smaller than</param>
        /// <param name="showDisabled">Whether products which are disabled should be included</param>
        /// <returns>
        /// An IEnumerable of Products including the Keyword navigation property 
        /// which has been filtered and sorted according to the given parameters.
        /// </returns>
        /// <remarks>
        /// If either price1 or price2 is not null then Products only get included 
        /// if they are equal to either or between price1 and price2 if price1 is 
        /// null but price2 isn't then price1 will act as 0.
        /// </remarks>
        public async Task<IEnumerable<Product>> GetAllProductsFilteredAsync(string searchString, double? minPrice, double? maxPrice, bool showDisabled)
        {
            IEnumerable<Product> Products = await GetAllProductsIncludeKeywordsAsync();

            if (searchString != null)
            {
                searchString = searchString.ToLower().Trim();
                Products = Products.Where(p =>
                    p.Name.ToLower().Contains(searchString) ||
                    p.Colour.ToLower().Contains(searchString) ||
                    p.Keywords.Any(k =>
                       k.Name.ToLower().Contains(searchString)
                       )
                    );
                if (searchString != "bårebuket")
                {
                    Products = Products.Where(p => !p.Keywords.Any(k => k.Name.ToLower() == "bårebuket"));
                }
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

            if (!showDisabled)
            {
                Products = from product in Products
                           where product.Disabled == false
                           select product;
            }
            return Products;
        }

        /// <summary>
        /// This method retrieves the first 4 products of which contains a keywords with the name "Buket".
        /// </summary>
        /// <returns>The first 4 Products which has a keyword named "Buket"</returns>
        public async Task<IEnumerable<Product>> GetFirst4BouquetProductsAsync()
        {
            return await ProductDbService.GetFirst4BouquetProductsAsync();
        }
    }
}
