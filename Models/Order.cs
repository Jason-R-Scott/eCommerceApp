using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    public class Order
    {
        public int OrderId {get;set;}
        [Required]
        public DateTime OrderDate {get;set;}
        public DateTime UpdatedAt {get;set;}
        public float OrderTotal {get;set;}
        public int UserID {get;set;}
        public User Creator {get;set;}
        public List<Product> ProductsInOrder{get;set;}
        public List<OrderDetail> OrderDetails {get;set;}    

    }
}