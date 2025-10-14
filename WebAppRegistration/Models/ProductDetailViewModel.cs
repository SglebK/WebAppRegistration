using System.Collections.Generic;

namespace WebAppRegistration.Models
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<Product> RecommendedProducts { get; set; }
    }
}