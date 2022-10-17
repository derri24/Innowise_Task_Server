using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.ProductResponses;
using Server.Services;




namespace Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ProductsResponse GetProductsByFridgeId(int fridgeId)
        {
            return _productService.GetProductsByFridgeId(fridgeId);
        }

        [HttpPost]
        public CreateProductsResponse CreateProducts(CreateProductsModel createProductsModel)
        {
            return _productService.CreateProducts(createProductsModel);
        }

        [HttpGet]
        public UpdateProductResponse GetProductById(int productId, int fridgeId)
        {
            return _productService.GetProductById(productId, fridgeId);
        }

        [HttpPut]
        public UpdateProductResponse UpdateProduct(int fridgeId, [FromBody] UpdateProductModel updateProductModel)
        {
            return _productService.UpdateProduct(fridgeId, updateProductModel);
        }

        [HttpDelete]
        public DeleteProductResponse DeleteProduct(int productId)
        {
            return _productService.DeleteProduct(productId);
        }


        [HttpPut]
        public IActionResult CallStoredProcedure()
        {
            _productService.CallStoredProcedure();
            return Ok();
        }
    }
}