namespace ECommerceApp.Models
{
    public class OrderDetail
    {
        public int OrderDetailId {get;set;}
        public int OrderId {get;set;}
        public int ProductId {get;set;}
        public string ProductName {get;set;}
        public float Price { get; set; }
        public int Quantity {get;set;}
        public float SubTotal {get;set;}
        public Order order {get;set;} 
        public Product productOrdered {get;set;}
        

 
    }
}