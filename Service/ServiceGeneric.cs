using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class ServiceGeneric<T> where T:class
    {
        protected DbGenericService<T> DbService { get; set; }

        public ServiceGeneric(DbGenericService<T> dbService)
        {
            DbService = dbService;
        }

        public async Task<IEnumerable<T>> GetAllDataAsync(IEnumerable<Func<T, bool>> conditions)
        {
            IEnumerable<T> data = (await DbService.GetObjectsAsync());
            foreach (Func<T, bool> condition in conditions)
            {
                data = data.Where(condition);
            }
            return data;
        }

        public async Task<IEnumerable<T>> GetAllDataAsync(IEnumerable<Func<T, bool>> conditions, IEnumerable<string> includeProperties)
        {
            IEnumerable<T> data = (await DbService.GetObjectsAsync(includeProperties));
            foreach (Func<T, bool> condition in conditions)
            {
                data = data.Where(condition);
            }
            return data;
        }

        public async Task<IEnumerable<T>> GetObjectsAsync(IEnumerable<string> includeProperties)
        {
            return await DbService.GetObjectsAsync(includeProperties);
        }
    }
}
