using Blomsterbinderiet.EFDbContext;
using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class OrderService
    {
        public List<Order> Orders { get; set; }
        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        private DbGenericService<Order> DbService { get; set; }
        private DbGenericService<OrderLine> OrderlineService { get; set; }
        public OrderService(DbGenericService<Order> dbService, DbGenericService<OrderLine> orderlineService)
        {
            DbService = dbService;
            ;
            Orders = dbService.GetObjectsAsync().Result.ToList();
            OrderlineService = orderlineService;
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
        public async Task AddOrderAsync(Order order)
        {
            Orders.Add(order);
            await DbService.AddObjectAsync(order);
        }
        public async Task AddOrderLineAsync(OrderLine orderLine)
        {
            OrderLines.Add(orderLine);
            await OrderlineService.AddObjectAsync(orderLine);
        }
    }
}
