using Blomsterbinderiet.Pages.Customer;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Blomsterbinderiet.Enums;
using System.Diagnostics.CodeAnalysis;

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
        //public int DeliveryId { get; set; }
        //public Delivery Delivery { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateOnly PickUpDate { get; set; }
        public TimeOnly PickUpTime { get; set; }

        public string? CommentShop { get; set; }
        //public ICollection<OrderLine> OrderLines { get; set; }
        public Status OrderStatus { get; set; }
        
        

        public Order(User customer, DateTime orderDate, DateOnly pickUpDate,TimeOnly pickUpTime)
        {
           
            CustomerID = customer.ID;
            EmployeeID = null;
            Employee = null;
            OrderDate = orderDate;
            CommentShop = null;
            PickUpDate = pickUpDate;
            PickUpTime  = pickUpTime;
            OrderStatus = Status.Ny;
            
        }

        public Order()
        {
        }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(CustomerID)}={CustomerID.ToString()}, {nameof(Customer)}={Customer}, {nameof(EmployeeID)}={EmployeeID.ToString()}, {nameof(Employee)}={Employee}, {nameof(OrderDate)}={OrderDate.ToString()}, {nameof(CompletedDate)}={CompletedDate.ToString()}, {nameof(CommentShop)}={CommentShop}}}";
        }


        


    }
}
