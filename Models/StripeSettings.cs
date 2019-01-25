using Microsoft.Extensions.Options;

namespace ECommerceApp.Models
{
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}