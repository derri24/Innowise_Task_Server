using System.Collections.Generic;

namespace Server.Models
{
    public class FridgesModel
    { 
        public List<FridgeModel> Fridges { get; set; }
        public FridgesModel()
        {
            Fridges = new List<FridgeModel>();
        }
    }
}