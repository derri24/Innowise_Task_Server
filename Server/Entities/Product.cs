using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities
{
    public class Product
    {
        [Required] public int ProductId { get; set; }
        [Required] public string Name { get; set; }
        public int CaloriesCount { get; set; }
        // public DateTime ExpiryDate { get; set; }
        public int DefaultQuantity { get; set; }

        public List<FridgeProduct> FridgeProducts { get; set; }

        public Product()
        {
            FridgeProducts = new List<FridgeProduct>();
        }
    }
}