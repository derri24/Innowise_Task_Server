using Server.Models;

namespace Server.FridgeResponses
{
    public class UpdateFridgeResponse
    {
        public UpdateFridgeModel Model { get; set; }
        public StatusResponse StatusResponse { get; set; }
    }
}