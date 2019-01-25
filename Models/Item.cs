using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ECommerceApp.Models
{
    public class Item
    {
        public int ItemId {get;set;}
        public int ProductId {get;set;}
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int UserID {get;set;}
        public User User {get;set;}
    }
}           