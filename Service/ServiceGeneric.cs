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

        public async Task<IEnumerable<T>> GetAllDataAsync(IEnumerable<string>? includeProperties = null, IEnumerable<Func<T, bool>>? conditions = null)
        {
            return await DbService.GetObjectsAsync(includeProperties, conditions);
        }

        public async Task<IEnumerable<T>> OrderBy(IEnumerable<T> listToBeSorted, string sortProperty, bool largeToSmall = false)
        {
            PropertyInfo property = typeof(T).GetProperty(sortProperty);
            if (property == null)
            {
                return listToBeSorted;
            }
            if(largeToSmall)
            {
                listToBeSorted = listToBeSorted.OrderByDescending(p => property.GetValue(p, null));
            } else
            {
                listToBeSorted = listToBeSorted.OrderBy(p => property.GetValue(p, null));
            }
            return listToBeSorted;
        }
    }
}
