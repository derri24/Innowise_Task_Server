using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Models;
using Server.Services;

namespace Tests;


[TestClass]
public class ProductTests
{

    [TestMethod]
    public void GettingProductByIdTest()
    {
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "atlant";
        createFridgeModel.Description = "good fridge";
        FridgeService fridgeService = new FridgeService();
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        
        ProductModel productModel = new ProductModel();
        productModel.Name = "carrot";
        productModel.CaloriesCount = 77;
        productModel.Quantity = 5;
        productModel.ExpiryDate = new DateTime(2022, 4, 12);
        CreateProductsModel createProductsModel = new CreateProductsModel();
        createProductsModel.Products.Add(productModel);
        createProductsModel.FridgeId = createFridgeResponse.FridgeId;
        
        ProductService productService = new ProductService();
        productService.CreateProducts(createProductsModel);
        var productId = createProductsModel.Products.FirstOrDefault().ProductId;
        var productResponse = productService.GetProductById(productId, createProductsModel.FridgeId);
        
        Assert.IsNotNull(productResponse.Model);
        Assert.AreEqual(productModel.Name,productResponse.Model.Name);
        Assert.AreEqual(productModel.Quantity,productResponse.Model.Quantity);
        Assert.AreEqual(productModel.ExpiryDate,productResponse.Model.ExpiryDate);
        Assert.AreEqual(StatusResponse.Success, productResponse.StatusResponse);
    
        var incorrectProductResponse = productService.GetProductById(-5,2);
        
        Assert.IsNull(incorrectProductResponse.Model);
        Assert.AreEqual(StatusResponse.Error, incorrectProductResponse.StatusResponse);
        
        fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
    }
    
    [TestMethod]
    public void GettingProductsByFridgeIdTest()
    {
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "atlant";
        createFridgeModel.Description = "good fridge";
        FridgeService fridgeService = new FridgeService();
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        
        ProductModel firstProductModel = new ProductModel();
        firstProductModel.Name = "eggs";
        firstProductModel.CaloriesCount = 56;
        firstProductModel.Quantity = 10;
        firstProductModel.ExpiryDate = new DateTime(2022, 4, 12);
        
        ProductModel secondProductModel = new ProductModel();
        secondProductModel.Name = "cheese";
        secondProductModel.CaloriesCount = 90;
        secondProductModel.Quantity = 2;
        secondProductModel.ExpiryDate = new DateTime(2022, 4, 12);

        CreateProductsModel createProductsModel = new CreateProductsModel();
        createProductsModel.Products.Add(firstProductModel);
        createProductsModel.Products.Add(secondProductModel);
        createProductsModel.FridgeId = createFridgeResponse.FridgeId;
        
        ProductService productService = new ProductService();
        productService.CreateProducts(createProductsModel);
        
        var productsResponse = productService.GetProductsByFridgeId( createProductsModel.FridgeId);
        
        Assert.IsTrue(productsResponse.Model.Products.Count ==2);
        Assert.AreEqual(StatusResponse.Success,productsResponse.StatusResponse);
        Assert.IsNull(productService.GetProductsByFridgeId(-1).Model);
    
        fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
    }
    
    [TestMethod]
    public void DeletingProductTest()
    {
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "atlant";
        createFridgeModel.Description = "good fridge";
        FridgeService fridgeService = new FridgeService();
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        
        ProductModel productModel = new ProductModel();
        productModel.Name = "eggs";
        productModel.CaloriesCount = 56;
        productModel.Quantity = 10;
        productModel.ExpiryDate = new DateTime(2022, 4, 12);
        
        CreateProductsModel createProductsModel = new CreateProductsModel();
        createProductsModel.Products.Add(productModel);
        createProductsModel.FridgeId = createFridgeResponse.FridgeId;
        ProductService productService = new ProductService();
        productService.CreateProducts(createProductsModel);
        var productId = createProductsModel.Products.FirstOrDefault().ProductId;
        var deleteProductResponse = productService.DeleteProduct(productId);

        var productResponse = productService.GetProductById(productId, createProductsModel.FridgeId);
        
        Assert.IsNull(productResponse.Model);
        Assert.AreEqual(StatusResponse.Success, deleteProductResponse.StatusResponse);
        
        var incorrectDeleteProductResponse = productService.DeleteProduct(-2);
        
        Assert.AreEqual(StatusResponse.Error, incorrectDeleteProductResponse.StatusResponse);
        
        fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
    }
    
    [TestMethod]
    public void CreateProductsTest()
    {
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "atlant";
        createFridgeModel.Description = "good fridge";
        FridgeService fridgeService = new FridgeService();
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        
        ProductModel firstProductModel = new ProductModel();
        firstProductModel.Name = "meat";
        firstProductModel.CaloriesCount = 490;
        firstProductModel.Quantity = 1;
        firstProductModel.ExpiryDate = new DateTime(2022, 8, 2);
        
        ProductModel secondProductModel = new ProductModel();
        secondProductModel.Name = "cheese";
        secondProductModel.CaloriesCount = 90;
        secondProductModel.Quantity = 2;
        secondProductModel.ExpiryDate = new DateTime(2022, 4, 12);

        CreateProductsModel createProductsModel = new CreateProductsModel();
        createProductsModel.Products.Add(firstProductModel);
        createProductsModel.Products.Add(secondProductModel);
        createProductsModel.FridgeId = createFridgeResponse.FridgeId;
        ProductService productService = new ProductService();
        var createProductsResponse = productService.CreateProducts(createProductsModel);
         
        Assert.AreEqual(StatusResponse.Success, createProductsResponse.StatusResponse);
        
        var  incorrectCreateProductsResponse = productService.CreateProducts(null);
        
        Assert.AreEqual(StatusResponse.Error, incorrectCreateProductsResponse.StatusResponse);

        CreateProductsModel incorrectCreateProductsModel = new CreateProductsModel();
        var secondIncorrectCreateProductsResponse = productService.CreateProducts(incorrectCreateProductsModel);
        
        Assert.AreEqual(StatusResponse.Error, secondIncorrectCreateProductsResponse.StatusResponse);
        
        incorrectCreateProductsModel.FridgeId = -7;
        incorrectCreateProductsModel.Products.Add(firstProductModel);
        secondIncorrectCreateProductsResponse = productService.CreateProducts(incorrectCreateProductsModel);
        
        Assert.AreEqual(StatusResponse.Error, secondIncorrectCreateProductsResponse.StatusResponse);
        
        fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
    }
    
    [TestMethod]
    public void UpdatingProductTest()
    {
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "atlant";
        createFridgeModel.Description = "good fridge";
        FridgeService fridgeService = new FridgeService();
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        
        ProductModel firstProductModel = new ProductModel();
        firstProductModel.Name = "pizza";
        firstProductModel.CaloriesCount = 76;
        
        firstProductModel.Quantity = 20;
        firstProductModel.ExpiryDate = new DateTime(2022, 4, 12);
        CreateProductsModel createProductsModel = new CreateProductsModel();
        createProductsModel.Products.Add(firstProductModel);
        createProductsModel.FridgeId = createFridgeResponse.FridgeId;
        ProductService productService = new ProductService();
        productService.CreateProducts(createProductsModel);
        var productId = createProductsModel.Products.FirstOrDefault().ProductId;

         UpdateProductModel firstUpdateProductModel = new UpdateProductModel();
         firstUpdateProductModel.ExpiryDate = new DateTime(2022, 9, 10);
         firstUpdateProductModel.Quantity = 2;
         firstUpdateProductModel.ProductId = productId;
         var firstUpdateProductResponse= productService.UpdateProduct(createFridgeResponse.FridgeId,firstUpdateProductModel);
        
        Assert.AreEqual(StatusResponse.Success, firstUpdateProductResponse.StatusResponse);
        
        var foundProduct = productService.GetProductById(productId,createProductsModel.FridgeId).Model;
        Assert.AreEqual(firstUpdateProductModel.Quantity, foundProduct.Quantity);
        Assert.AreEqual(firstUpdateProductModel.ExpiryDate, foundProduct.ExpiryDate);

        
        UpdateProductModel incorrectUpdateProductModel = new UpdateProductModel();
        incorrectUpdateProductModel.Quantity = -6;
        incorrectUpdateProductModel.ProductId = productId;
        var incorrectUpdateProductResponse1= productService.UpdateProduct(productId,incorrectUpdateProductModel);
        
        Assert.AreEqual(StatusResponse.Error, incorrectUpdateProductResponse1.StatusResponse);
        
        var incorrectUpdateProductResponse= productService.UpdateProduct(createProductsModel.FridgeId,null);
        
        Assert.AreEqual(StatusResponse.Error, incorrectUpdateProductResponse.StatusResponse);
        
        fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
    }
}