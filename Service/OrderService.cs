using Blomsterbinderiet.EFDbContext;
using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class OrderService
    {
        public List<Order> Orders { get; set; }

        private DbGenericService<Order> DbService { get; set; }
        public OrderService(DbGenericService<Order> dbService)
        {
            DbService = dbService;
            ;
            Orders = dbService.GetObjectsAsync().Result.ToList();
        }

        public List<Order> GetAllOrders()

        {
            Orders = DbService.GetObjectsAsync().Result.ToList();
            List<Order> orders = Orders;
            return orders;
        }

        public Order GetOrderById(int id)
        {
           Order order = DbService.GetObjectByIdAsync(id).Result;
            return order;
        }
        public async Task UpdateOrderAsync(Order order)
        {
            await DbService.UpdateObjectAsync(order);
           
        }
    }
}
