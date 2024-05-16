using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Blomsterbinderiet.Enums;

namespace Blomsterbinderiet.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public User Customer { get; set; }
        public int? EmployeeID { get; set; }
        public User? Employee { get; set; }
        public int? DeliveryId { get; set; }
        public Delivery? Delivery { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime PickUpDate { get; set; }
        public string? CommentShop { get; set; }
        //public ICollection<OrderLine> OrderLines { get; set; }
        public Status OrderStatus { get; set; }

        public Order(User customer, DateTime orderDate, DateTime PickUpTime)
        {
            CustomerID = customer.ID;
            EmployeeID = null;
            Employee = null;
            OrderDate = orderDate;
            CommentShop = null;
            PickUpDate = PickUpTime;
            OrderStatus = Status.Ny;
        }

        public Order()
        {
        }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id}, {nameof(CustomerID)}={CustomerID}, {nameof(Customer)}={Customer}, {nameof(EmployeeID)}={EmployeeID}, {nameof(Employee)}={Employee}, {nameof(DeliveryId)}={DeliveryId}, {nameof(Delivery)}={Delivery}, {nameof(OrderDate)}={OrderDate}, {nameof(CompletedDate)}={CompletedDate}, {nameof(PickUpDate)}={PickUpDate}, {nameof(CommentShop)}={CommentShop}, {nameof(OrderStatus)}={OrderStatus}}}";
        }
    }
}
