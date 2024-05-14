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
                //get the correct product including its tags (dont add asnotracking)
                Product tempProduct = await context.Set<Product>().Include(p => p.Keywords).FirstAsync(p => p.ID == product.ID);

                //remove all of its keywords
                tempProduct.Keywords.Clear();

                //add the new keywords
                tempProduct.Keywords = (await context.Set<Keyword>().Where(k => idsOfKeywords.Contains(k.ID)).ToListAsync());

                //pass the values from product from the parameter to the product being tracked
                tempProduct.Colour = product.Colour;
                tempProduct.Description = product.Description;
                tempProduct.Name = product.Name;
                tempProduct.Price = product.Price;
                if(product.Image != null)
                {
                    tempProduct.Image = product.Image;
                }

                //update and save
                context.Set<Product>().Update(tempProduct);
                await context.SaveChangesAsync();
            }
        }
    }
}
