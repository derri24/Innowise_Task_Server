using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using EntityTest.Properties;
using Server.Entities;
using Server.Models;
using Server.ProductResponses;

namespace Server.Services
{
    public interface IProductService
    {
        public CreateProductsResponse CreateProducts(CreateProductsModel createProductsModel);
        public ProductsResponse GetProductsByFridgeId(int fridgeId);
        public UpdateProductResponse GetProductById(int productId, int fridgeId);
        public UpdateProductResponse UpdateProduct(int fridgeId, UpdateProductModel updateProductModel);
        public DeleteProductResponse DeleteProduct(int productId);

        public void CallStoredProcedure();
    }

    public class ProductService : IProductService
    {
        public CreateProductsResponse CreateProducts(CreateProductsModel createProductsModel)
        {
            CreateProductsResponse createProductsResponse = new CreateProductsResponse();
            if (createProductsModel == null || createProductsModel.FridgeId <= 0 ||
                createProductsModel.Products.Count <= 0)
            {
                createProductsResponse.StatusResponse = StatusResponse.Error;
                return createProductsResponse;
            }

            using (EntityContext db = new EntityContext())
            {
                var fridge = db.Fridges.Include(fp => fp.FridgeProducts.Select(p => p.Product))
                    .FirstOrDefault(f => f.FridgeId == createProductsModel.FridgeId);
                if (fridge == null)
                {
                    createProductsResponse.StatusResponse = StatusResponse.Error;
                    return createProductsResponse;
                }

                foreach (var createProductModel in createProductsModel.Products)
                {
                    var foundProductByName =
                        db.Products.FirstOrDefault(p => p.Name.Trim() == createProductModel.Name.Trim());
                    FridgeProduct fridgeProduct = new FridgeProduct();
                    fridgeProduct.Fridge = fridge;
                    fridgeProduct.Quantity = createProductModel.Quantity;
                    fridgeProduct.ExpiryDate = createProductModel.ExpiryDate;
                    if (foundProductByName == null)
                    {
                        Product product = new Product();
                        product.Name = createProductModel.Name;
                        product.CaloriesCount = createProductModel.CaloriesCount;
                        product.DefaultQuantity = createProductModel.Quantity;
                        fridgeProduct.Product = product;
                    }
                    else
                    {
                        if (fridge.FridgeProducts.FirstOrDefault(fp =>
                                fp.Product.ProductId == foundProductByName.ProductId) != null)
                            continue;
                        fridgeProduct.Product = foundProductByName;
                    }

                    fridge.FridgeProducts.Add(fridgeProduct);
                    db.SaveChanges();
                    createProductModel.ProductId = fridgeProduct.Product.ProductId;
                }
            }

            createProductsResponse.StatusResponse = StatusResponse.Success;
            return createProductsResponse;
        }

        public UpdateProductResponse UpdateProduct(int fridgeId, UpdateProductModel updatedProductModel)
        {
            UpdateProductResponse updateProductResponse = new UpdateProductResponse();
            if (updatedProductModel == null
                || updatedProductModel.Quantity < 0
                || updatedProductModel.ProductId <= 0)
            {
                updateProductResponse.StatusResponse = StatusResponse.Error;
                return updateProductResponse;
            }

            using (EntityContext db = new EntityContext())
            {
                var fridge = db.Fridges.Include(f => f.FridgeProducts.Select(fp => fp.Product))
                    .FirstOrDefault(f => f.FridgeId == fridgeId);
                if (fridge != null)
                {
                    var fridgeProduct =
                        fridge.FridgeProducts.FirstOrDefault(p => p.Product.ProductId == updatedProductModel.ProductId);
                    if (fridgeProduct != null)
                    {
                        fridgeProduct.ExpiryDate = updatedProductModel.ExpiryDate;
                        fridgeProduct.Quantity = updatedProductModel.Quantity;
                    }
                }

                db.SaveChanges();
            }

            updateProductResponse.StatusResponse = StatusResponse.Success;
            return updateProductResponse;
        }

        public ProductsResponse GetProductsByFridgeId(int fridgeId)
        {
            ProductsResponse productsResponse = new ProductsResponse();
            if (fridgeId <= 0)
            {
                productsResponse.StatusResponse = StatusResponse.Error;
                return productsResponse;
            }

            ProductsModel productsModel = new ProductsModel();
            using (EntityContext db = new EntityContext())
            {
                var fridgeProducts = db.Fridges.Include(f => f.FridgeProducts.Select(fp => fp.Product))
                    .FirstOrDefault(f => f.FridgeId == fridgeId)
                    ?.FridgeProducts;
                if (fridgeProducts != null)
                {
                    foreach (var fridgeProduct in fridgeProducts)
                    {
                        ProductModel productModel = new ProductModel();
                        productModel.Name = fridgeProduct.Product.Name;
                        productModel.CaloriesCount = fridgeProduct.Product.CaloriesCount;
                        productModel.Quantity = fridgeProduct.Quantity;

                        productModel.ExpiryDate = fridgeProduct.ExpiryDate;
                        productModel.ProductId = fridgeProduct.Product.ProductId;
                        productsModel.Products.Add(productModel);
                    }
                }

                productsModel.FridgeId = fridgeId;
            }

            productsResponse.Model = productsModel;
            productsResponse.StatusResponse = StatusResponse.Success;
            return productsResponse;
        }

        public UpdateProductResponse GetProductById(int productId, int fridgeId)
        {
            UpdateProductResponse updateProductResponse = new UpdateProductResponse();
            if (productId <= 0 || fridgeId <= 0)
            {
                updateProductResponse.StatusResponse = StatusResponse.Error;
                return updateProductResponse;
            }

            UpdateProductModel updateProductModel = new UpdateProductModel();
            using (EntityContext db = new EntityContext())
            {
                var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    updateProductModel.Name = product.Name;
                    updateProductModel.ProductId = product.ProductId;

                    var fridgeProduct = db.Fridges.Include(f => f.FridgeProducts.Select(fp => fp.Product))
                        .FirstOrDefault(f => f.FridgeId == fridgeId)
                        ?.FridgeProducts.FirstOrDefault(fp => fp.Product.ProductId == productId);
                    if (fridgeProduct != null)
                    {
                        updateProductModel.ExpiryDate = fridgeProduct.ExpiryDate;
                        updateProductModel.Quantity = fridgeProduct.Quantity;
                    }

                    updateProductResponse.StatusResponse = StatusResponse.Success;
                    updateProductResponse.Model = updateProductModel;
                    return updateProductResponse;
                }
            }

            updateProductResponse.StatusResponse = StatusResponse.Error;
            return updateProductResponse;
        }

        public DeleteProductResponse DeleteProduct(int productId)
        {
            DeleteProductResponse deleteProductResponse = new DeleteProductResponse();
            if (productId <= 0)
            {
                deleteProductResponse.StatusResponse = StatusResponse.Error;
                return deleteProductResponse;
            }

            using (EntityContext db = new EntityContext())
            {
                var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }

            deleteProductResponse.StatusResponse = StatusResponse.Success;
            return deleteProductResponse;
        }
        
        public void CallStoredProcedure()
        {
            using (var connection = new SqlConnection(EntityContext.GetConnectionString()))
            {
                using (var command = new SqlCommand("dbo.stored_procedure", connection) { 
                           CommandType = CommandType.StoredProcedure }) {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
    
    
}