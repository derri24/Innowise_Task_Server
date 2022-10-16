using System;

namespace Server.Models
{
    public class ProductModel
    { 
        public int ProductId { get; set; }
        public string Name { get; set; }
        
        public int Quantity { get; set; }
        public int CaloriesCount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}