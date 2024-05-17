using Blomsterbinderiet.EFDbContext;
using Blomsterbinderiet.Models;
using Microsoft.EntityFrameworkCore;

namespace Blomsterbinderiet.Service
{
    public class ProductDbService : DbGenericService<Product>
    {
        public async Task<Product?> GetProductIncludingKeywordsByID(int id)
        {
            using (var context = new BlomstDbContext())
            {
                return await context.Products.AsNoTracking()
                                             .Include(p => p.Keywords)
                                             .FirstOrDefaultAsync(p => p.ID == id);
            }
        }

        public async Task AddProductAsync(Product product, int[] idsOfKeywords)
        {
            using (var context = new BlomstDbContext())
            {
                product.Keywords = await context.Keywords.Where(k => idsOfKeywords.Contains(k.ID)).ToListAsync();
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateObjectAsync(Product product, IEnumerable<int> idsOfKeywords)
        {
            using (var context = new BlomstDbContext())
            {
                //start tasks
                Task<Product> productTask = context.Set<Product>().Include(p => p.Keywords).FirstAsync(p => p.ID == product.ID);
                Task<List<Keyword>> keywordsTask = context.Set<Keyword>().Where(k => idsOfKeywords.Contains(k.ID)).ToListAsync();

                Product tempProduct = await productTask;
                //remove all of its keywords
                tempProduct.Keywords.Clear();

                //pass the values from product from the parameter to the product being tracked
                tempProduct.Colour = product.Colour;
                tempProduct.Description = product.Description;
                tempProduct.Name = product.Name;
                tempProduct.Price = product.Price;
                if(product.Image != null)
                {
                    tempProduct.Image = product.Image;
                }

                //add the new keywords
                tempProduct.Keywords = await keywordsTask;

                //update and save
                context.Set<Product>().Update(tempProduct);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetFirst4BuketProducts()
        {
            using (var context = new BlomstDbContext())
            {
                return await context.Products.AsNoTracking()
                                             .Include(p => p.Keywords)
                                             .Where(p => p.Keywords.Any(k => k.Name == "Buket"))
                                             .Take(4)
                                             .ToListAsync();
            }
        }
    }
}
