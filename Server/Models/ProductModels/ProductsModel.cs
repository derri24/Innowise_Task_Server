using System.Collections.Generic;

namespace Server.Models
{
    public class ProductsModel
    {
        public int FridgeId { get; set; }
        public List<ProductModel> Products { get; set; }
        public ProductsModel()
        {
            Products = new List<ProductModel>();
        }
    }
}