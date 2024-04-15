using Blomsterbinderiet.Pages.Customer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public User Customer { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public User Employee { get; set; }
        //public int DeliveryId { get; set; }
        //public Delivery Delivery { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string CommentShop { get; set; }
        //public ICollection<OrderLine> OrderLines { get; set; }
        //public enum MyEnum
        //{
        public Status OrderStatus { get; set; }
        
        public enum Status { Ny = 0, Afvist = 1, Bekræftet = 2, Klargøres = 3, Færdig = 4, Udlevet = 5 }

        public Order(int id, int customerId, User customer, int employeeId, User employee, DateTime orderDate, DateTime completedDate, string commentShop)
        {
            Id = id;
            CustomerId = customerId;
            Customer = customer;
            EmployeeId = employeeId;
            Employee = employee;
            OrderDate = orderDate;
            CompletedDate = completedDate;
            CommentShop = commentShop;
            OrderStatus = Status.Ny;
            
        }

        public Order()
        {
        }

        public decimal GetTotal()
        {
            decimal total = 0;
            return total ;
        }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(CustomerId)}={CustomerId.ToString()}, {nameof(Customer)}={Customer}, {nameof(EmployeeId)}={EmployeeId.ToString()}, {nameof(Employee)}={Employee}, {nameof(OrderDate)}={OrderDate.ToString()}, {nameof(CompletedDate)}={CompletedDate.ToString()}, {nameof(CommentShop)}={CommentShop}}}";
        }


        


    }
}
