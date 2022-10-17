using Server.Models;

namespace Server.FridgeResponses
{
    public class FridgesResponse
    {
        public FridgesModel Model { get; set; }
        public StatusResponse StatusResponse { get; set; }
    }
}