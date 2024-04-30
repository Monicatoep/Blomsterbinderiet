using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blomsterbinderiet.Models
{
    public class OrderLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public int OrderID { get; set; }
        public Order Order { get; set; }
        [Required]
        public int ProductID { get; set; }
        public Product Product { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        [Required]
        [DisplayName("Mængde")]
        public int Amount { get; set; }

        public OrderLine()
        {
        }

        public OrderLine(Order order, Product product, int amount)
        {
            Order = null;
            OrderID = order.Id;
            Product = null;
            ProductID = product.ID;
            ProductPrice = product.Price;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{{{nameof(ID)}={ID.ToString()}, {nameof(OrderID)}={OrderID.ToString()}, {nameof(Order)}={Order}, {nameof(ProductID)}={ProductID.ToString()}, {nameof(Product)}={Product}, {nameof(Amount)}={Amount.ToString()}}}";
        }
    }
}
