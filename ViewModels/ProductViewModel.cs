using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        [MinLength(2,ErrorMessage="Product Name must be atleast 2 characters.")]
        public string ProductName {get;set;}
        [Required]
        [MinLength(10,ErrorMessage="Product Name must be atleast 10 characters.")]
        public string ProductDescription {get;set;}
        [Required]
        public int InitialQuantity {get;set;}
        [Required]
        public float Price {get;set;}   
        public float ImageUrl {get;set;}   
             
    }
}