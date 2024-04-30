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

        #region GetAllDataAsync + overloads
        public async Task<IEnumerable<T>> GetAllDataAsync(IEnumerable<Expression<Func<T, bool>>> conditions)
        {
            return await DbService.GetObjectsAsync(conditions);
        }

        public async Task<IEnumerable<T>> GetObjectsAsync(IEnumerable<string> includeProperties)
        {
            return await DbService.GetObjectsAsync(includes: includeProperties);
        }

        public async Task<IEnumerable<T>> GetAllDataAsync(IEnumerable<Expression<Func<T, bool>>> conditions, IEnumerable<string> includeProperties)
        {
            return await DbService.GetObjectsAsync(conditions,includeProperties);
        }

        public async Task<IEnumerable<T>> GetAllDataAsync(Expression<Func<T, bool>> condition)
        {
            return await DbService.GetObjectsAsync(new List<Expression<Func<T, bool>>>() { condition });
        }

        public async Task<IEnumerable<T>> GetAllDataAsync(string includeProperty)
        {
            return await DbService.GetObjectsAsync(includes: new List<string>() { includeProperty });
        }

        public async Task<IEnumerable<T>> GetAllDataAsync(Expression<Func<T, bool>> condition, string includeProperty)
        {
            return await DbService.GetObjectsAsync(new List<Expression<Func<T, bool>>>() { condition }, new List<string>() { includeProperty });
        }
        #endregion

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
