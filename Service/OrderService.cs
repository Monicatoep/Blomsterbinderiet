using Blomsterbinderiet.EFDbContext;
using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class OrderService
    {
        public List<Order> Orders { get; set; }
        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        private DbGenericService<Order> DbService { get; set; }
        public DbGenericService<OrderLine> OrderlineService { get; set; }
        public OrderService(DbGenericService<Order> dbService, DbGenericService<OrderLine> orderlineService)
        {
            DbService = dbService;
            
            Orders = dbService.GetObjectsAsync().Result.ToList();
            OrderlineService = orderlineService;
        }

        public List<Models.Order> GetAllOrders()

        {
            Orders = DbService.GetObjectsAsync().Result.ToList();
            List<Models.Order> orders = Orders;
            return orders;
        }

        public Models.Order GetOrderById(int id)
        {
           Models.Order order = DbService.GetObjectByIdAsync(id).Result;
            return order;
        }
        public async Task UpdateOrderAsync(Models.Order order)
        {
            await DbService.UpdateObjectAsync(order);
           
        }

        public double GetOrderSum(List<OrderLine> orderLines)
        {
            double orderSum = 0;
            if (orderLines == null)
            {
                return orderSum; 
            }
            
            foreach (OrderLine orderLine in orderLines)
            {
                orderSum += orderLine.Product.Price * orderLine.Amount;
            }
            return orderSum;
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
