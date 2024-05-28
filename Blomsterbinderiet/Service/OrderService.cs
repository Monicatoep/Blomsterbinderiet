using Blomsterbinderiet.DAO;
using Blomsterbinderiet.Enums;
using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Service
{
    public class OrderService
    {
        private DbGenericService<OrderLine> OrderLineDbService { get; set; }
        private DbGenericService<Delivery> DeliveryDbService { get; set; }
        private DbGenericService<Order> OrderDbService { get; set; }
        private UserService UserService { get; set; }
        private ProductService ProductService { get; set; }
        public List<Order> Orders { get; set; }
        public List<Delivery> Deliveries { get; set; }
        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        public OrderService(DbGenericService<Order> dbService, DbGenericService<OrderLine> orderlineService, DbGenericService<Delivery> deliveryDbService, UserService userService, ProductService productService)
        {
            OrderDbService = dbService;
            Orders = dbService.GetObjectsAsync().Result.ToList();
            Deliveries = deliveryDbService.GetObjectsAsync().Result.ToList();
            OrderLineDbService = orderlineService;
            UserService = userService;
            ProductService = productService;
            DeliveryDbService = deliveryDbService;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            Orders = (await OrderDbService.GetObjectsAsync()).ToList();
            List<Order> orders = Orders;
            return orders;
        }
        public async Task<List<Order>> GetAllOrdersWeekAsync()

        {
            Orders = (await OrderDbService.GetObjectsAsync(nameof(Order.Customer))).ToList();
            List<Order> orders = (from order in Orders
                                         where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate>=DateTime.Now
                                         select order).ToList();
            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await OrderDbService.GetObjectByIdAsync(id);
        }

        public async Task<Delivery?> GetDeliveryByOrderIdAsync(int id)
        {
            return await DeliveryDbService.GetObjectByIdAsync(id);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await OrderDbService.UpdateObjectAsync(order);
        }

        public async Task UpdateOrderAsync(Order order, string property)
        {
            await OrderDbService.UpdateObjectAsync(order,new List<string>(){ property });
        }

        public double GetOrderSum(IEnumerable<OrderLine> orderLines)
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

        public async Task<double> GetOrderSumAsync(IEnumerable<OrderLine> orderLines)
        {
            double orderSum = 0;
            if (orderLines == null)
            {
                return orderSum;
            }

            foreach (OrderLine orderLine in orderLines)
            {
                orderSum += ((await ProductService.GetProductByIdAsync(orderLine.ProductID)).Price * orderLine.Amount);
            }
            return orderSum;
        }

        public async Task AddOrderAsync(Order order)
        {
            Orders.Add(order);
            await OrderDbService.AddObjectAsync(order);
        }

        public async Task AddOrderLineAsync(OrderLine orderLine)
        {
            OrderLines.Add(orderLine);
            await OrderLineDbService.AddObjectAsync(orderLine);
        }

        public async Task AddDeliveryAsync(Delivery delivery)
        {
            Deliveries.Add(delivery);
            await DeliveryDbService.AddObjectAsync(delivery);
        }

        public async Task DenyOrderAsync(int id)
        {
            Order order = await OrderDbService.GetObjectByIdAsync(id);
            order.OrderStatus = Status.Afvist;
            await OrderDbService.UpdateObjectAsync(order);
        }

        public async Task ConfirmOrderAsync(int id)
        {
            Order order = await OrderDbService.GetObjectByIdAsync(id);
            order.OrderStatus = Status.Bekræftet;
            await OrderDbService.UpdateObjectAsync(order);
        }

        public async Task OrderInProgressAsync(int id, string userID)
        {
            Order order = await OrderDbService.GetObjectByIdAsync(id);
            order.OrderStatus = Status.Klargøres;

            string userId = userID;
            
            if (userId != null)
            {
                order.Employee = await UserService.GetUserByIdAsync(userId);
                order.EmployeeID = order.Employee.ID;
            }

            await OrderDbService.UpdateObjectAsync(order);
        }

        public async Task ChangeOrderStatusAsync(int id)
        {
            Order order = await OrderDbService.GetObjectByIdAsync(id);
            if (order.OrderStatus == Status.Klargøres)
            {
                order.OrderStatus = Status.Færdig;
            }
            else if (order.OrderStatus == Status.Færdig)
            {
                order.CompletedDate = DateTime.Now;
                order.OrderStatus = Status.Udleveret;
            }
            await OrderDbService.UpdateObjectAsync(order);
        }

        public async Task<IEnumerable<MyOrdersDAO>> GetOrdersByUserIdAsync(int id)
        {
            int userId = id;
            var orders = from o in await GetAllOrdersAsync()
                     where o.CustomerID == userId
                     join l in OrderLineDbService.GetObjectsAsync().Result on o.Id equals l.OrderID into ol
                     orderby o.OrderDate descending
                     select new MyOrdersDAO{ Order = o, OrderLines = ol, Amount = ol.Sum((OrderLine o) => o.Amount) };
            return orders;
        }

        public async Task CreateNewOrderAsync(User user, DateTime pickUpDate, List<OrderLine> orderLines)
        {
            Models.Order order = new(user, DateTime.Now, pickUpDate);
            await AddOrderAsync(order);
            foreach (OrderLine line in orderLines)
            {
                await AddOrderLineAsync(new OrderLine(order, line.Product, line.Amount));
            }
        }

        public async Task CreateNewOrderWithDeliveryAsync(User user, DateTime pickUpTime, List<OrderLine> orderLines, Delivery delivery)
        {
            Delivery newDelivery = new(delivery.DeseasedName, delivery.Address);
            await AddDeliveryAsync(newDelivery);

            Order order = new(user, DateTime.Now, pickUpTime);
            order.DeliveryId = newDelivery.ID;
            await AddOrderAsync(order);

            foreach (OrderLine line in orderLines)
            {
                await AddOrderLineAsync(new OrderLine(order, line.Product, line.Amount));
            }
        }

        public  IEnumerable<Order> SortByDueDate()
        {
               return from order in Orders
                      where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                      orderby order.PickUpDate
                      select order;
        }

        public IEnumerable<Order> SortByDueDateDes()
        {
            return from order in Orders
                   where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                   orderby order.PickUpDate descending
                   select order;
        }

        public IEnumerable<Order> FilterByDueDate(DateOnly date)
        {
            TimeOnly time = TimeOnly.MinValue;
            DateTime dateTime = date.ToDateTime(time);
            
            return from order in Orders
                   where order.PickUpDate.Date.Equals(dateTime)
                   select order;

        }

        public IEnumerable<Order> FilterByDueDateToday()
        {
            DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly time = TimeOnly.MinValue;
            DateTime dateTime = dateNow.ToDateTime(time);
           
            return from order in Orders
                   where order.PickUpDate.Date.Equals(dateTime)
                   select order;
        }

        public IEnumerable<Order> SortByEmployee()
        {
            return from order in Orders
                   where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                   orderby order.EmployeeID 
                   select order;
        }

        public IEnumerable<Order> SortByEmployeeDes()
        {
            return from order in Orders
                   where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                   orderby order.EmployeeID descending
                   select order;
        }

        public IEnumerable<Order> FilterByEmployee(int id)
        {
            return from order in Orders
                   where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                   where order.EmployeeID == id
                   select order;
        }

        public IEnumerable<Order> FilterByEmployeeNull()
        {
            return from order in Orders
                   where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                   where order.EmployeeID == null
                   select order;
        }

        public IEnumerable<Order> SortByStatus()
        {
            return from order in Orders
                   where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                   orderby order.OrderStatus
                   select order;
        }

        public IEnumerable<Order> SortByStatusDes()
        {
            return from order in Orders
                   where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                   orderby order.OrderStatus descending
                   select order;
        }

        public IEnumerable<Order> FilterByStatus(Status status)
        {
            return from order in Orders
                   where order.PickUpDate < DateTime.Now.AddDays(7) && order.PickUpDate >= DateTime.Now
                   where order.OrderStatus==status
                   select order;
        } 

        public async Task<IEnumerable<OrderLine>> GetOrderlinesByOrderIdAsync(int id)
        {
            OrderLines = (await OrderLineDbService.GetObjectsAsync()).ToList();
            return from orderLine in OrderLines
                   where orderLine.OrderID == id
                   select orderLine;
        }
    }
}