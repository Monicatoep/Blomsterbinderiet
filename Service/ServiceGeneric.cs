using Blomsterbinderiet.Models;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection;

namespace Blomsterbinderiet.Service
{
    public class ServiceGeneric<T> where T:class
    {
        protected DbGenericService<T> DbService { get; set; }

        public ServiceGeneric(DbGenericService<T> dbService)
        {
            DbService = dbService;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbService.GetObjectByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllDataAsync()
        {
            return await DbService.GetObjectsAsync();
        }

        public async Task<IEnumerable<T>> GetAllDataAsync(string includeProperty)
        {
            return await DbService.GetObjectsAsync(includeProperty);
        }

        public async Task<IEnumerable<T>> GetAllDataAsync(IEnumerable<string>? includeProperties)
        {
            return await DbService.GetObjectsAsync(includeProperties);
        }
    }
}
