using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.ViewModels {
    public class LoginViewModel {

        [Required(ErrorMessage = "Required Field")]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}