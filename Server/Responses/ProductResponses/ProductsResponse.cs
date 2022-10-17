using Server.Models;

namespace Server.ProductResponses
{
    public class ProductsResponse
    {
        public ProductsModel Model { get; set; }
        public StatusResponse StatusResponse { get; set; }
    }
}