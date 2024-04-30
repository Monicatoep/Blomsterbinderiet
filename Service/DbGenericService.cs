using Blomsterbinderiet.EFDbContext;
using Microsoft.EntityFrameworkCore;

namespace Blomsterbinderiet.Service
{
    public class DbGenericService<T> where T : class
    {
        public async Task AddObjectAsync(T obj)
        {
            using (var context = new BlomstDbContext())
            {
                context.Set<T>().Add(obj);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteObjectAsync(T obj)
        {
            using (var context = new BlomstDbContext())
            {
                context.Set<T>().Remove(obj);
                await context.SaveChangesAsync();
            }
        }

        public async Task<T> GetObjectByIdAsync(int id)
        {
            using (var context = new BlomstDbContext())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public async Task<IEnumerable<T>> GetObjectsAsync()
        {
            using (var context = new BlomstDbContext())
            {
                return await context.Set<T>().AsNoTracking().ToListAsync();
            }
        }

        public async Task SaveObjectsAsync(List<T> objs)
        {
            using (var context = new BlomstDbContext())
            {
                foreach (T obj in objs)
                {
                    context.Set<T>().Add(obj);
                    context.SaveChanges();
                }

                context.SaveChanges();
            }
        }

        public async Task UpdateObjectAsync(T obj)
        {
            using (var context = new BlomstDbContext())
            {
                context.Set<T>().Update(obj);
                await context.SaveChangesAsync();
            }
        }

        


        //https://stackoverflow.com/questions/77892880/are-there-any-safer-alternatives-to-hidden-input-fields-for-persisting-propertie
        //https://www.reddit.com/r/dotnet/comments/r1srvo/updating_only_changed_fields/
        public async Task UpdateObjectAsync(T obj, IEnumerable<string> updatedProperties)
        {
            using (var context = new BlomstDbContext())
            {
                context.Attach(obj);
                foreach(string property in updatedProperties)
                {
                    context.Entry(obj).Property(property).IsModified = true;
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
