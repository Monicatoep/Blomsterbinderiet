using Blomsterbinderiet.DAO;
using Blomsterbinderiet.EFDbContext;
using Blomsterbinderiet.Enum;
using Blomsterbinderiet.Models;
using System.Security.Claims;

namespace Blomsterbinderiet.Service
{
    public class OrderService
    {
        public List<Order> Orders { get; set; }
        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        private DbGenericService<Order> DbService { get; set; }
        public DbGenericService<OrderLine> OrderlineService { get; set; }
        public UserService UserService { get; set; }
    
        public OrderService(DbGenericService<Order> dbService, DbGenericService<OrderLine> orderlineService)
        {
            DbService = dbService;
            
            Orders = dbService.GetObjectsAsync().Result.ToList();
            OrderlineService = orderlineService;
        }

        public async Task<List<Models.Order>> GetAllOrdersAsync()

        {
            Orders = DbService.GetObjectsAsync().Result.ToList();
            List<Models.Order> orders = Orders;
            return orders;
        }

        public async Task<Models.Order> GetOrderByIdAsync(int id)
        {
           Models.Order order = await DbService.GetObjectByIdAsync(id);
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

        public async Task DenyOrderAsync(int id)
        {
            Order order = DbService.GetObjectByIdAsync(id).Result;
            order.OrderStatus = Status.Afvist;
            await DbService.UpdateObjectAsync(order);
        }

        public async Task ConfirmOrderAsync(int id)
        {
            Order order = DbService.GetObjectByIdAsync(id).Result;
            order.OrderStatus = Status.Bekræftet;
            await DbService.UpdateObjectAsync(order);
        }

        public async Task OrderInProgressAsync(int id, string uId)
        {
            Order order = DbService.GetObjectByIdAsync(id).Result;
            order.OrderStatus = Status.Klargøres;

            string userId = uId;
            if (userId != null)
            {
                order.Employee = await UserService.GetUserByIdAsync(userId);
                order.EmployeeID = order.Employee.ID;
            }

            await DbService.UpdateObjectAsync(order);
        }

        public async Task ChangeOrderStatusAsync(int id)
        {
            Order order = DbService.GetObjectByIdAsync(id).Result;
            if (order.OrderStatus == Status.Klargøres)
            {
                order.OrderStatus = Status.Færdig;
            }
            else if (order.OrderStatus == Status.Færdig)
            {
                order.CompletedDate = DateTime.Now;
                order.OrderStatus = Status.Udleveret;
            }
            await DbService.UpdateObjectAsync(order);
        }

        public async Task<IEnumerable<MyOrdersDAO>> GetOrdersByUserIdAsync(int id)
        {
            int userId = id;
            var orders = from o in await GetAllOrdersAsync()
                     where o.CustomerID == userId
                     join l in OrderlineService.GetObjectsAsync().Result on o.Id equals l.OrderID into ol
                     orderby o.OrderDate descending
                     select new DAO.MyOrdersDAO{ Order = o, OrderLine = ol, Amount = ol.Sum((OrderLine o) => o.Amount) };
            return orders;
        }
    }
}
