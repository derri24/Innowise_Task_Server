using Server.Models;

namespace Server.ProductResponses
{
    public class UpdateProductResponse
    {
        public StatusResponse StatusResponse { get; set; }
        public  UpdateProductModel Model{ get; set; }
    }
}