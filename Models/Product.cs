using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}
        [Required] 
        public string ProductName {get;set;}
        [Required]
        public string ProductDescription {get;set;}
        public string ImageUrl {get;set;}
        [Required]
        public int InitialQuantity {get;set;}
        [Required]
        public float Price {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<OrderDetail> ProductDetails {get;set;}
    }
}