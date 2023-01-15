using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities
{
    public class FridgeProduct
    {
        [Required] public int FridgeProductId { get; set; }
        [Required] public Fridge Fridge { get; set; }
        [Required] public Product Product { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public DateTime ExpiryDate { get; set; }
    }
}