using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.Models;

namespace Server.Entities
{
    public class Fridge
    {
        [Required] public int FridgeId { get; set; }
        [Required] public string Model { get; set; }
        public string Description { get; set; }

        public List<FridgeProduct> FridgeProducts { get; set; }

        public Fridge()
        {
            FridgeProducts = new List<FridgeProduct>();
        }
    }
}