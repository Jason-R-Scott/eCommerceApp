using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.ViewModels
{
    public class RegistrationViewModel
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage="Cannot contain numbers.")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Required Field")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage="Cannot contain numbers.")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Required Field")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage="Password must be atleast 8 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must include one number, one letter, and one special character")]
        public string Password { get; set; }


        [NotMapped]  //not sure if needed
        [Required(ErrorMessage = "Required Field")]
        [Compare("Password")]        
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}