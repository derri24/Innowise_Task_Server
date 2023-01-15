using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Server.Models;
using Server.Services;

namespace Tests;

[TestClass]
public class FridgeTests
{
    [TestMethod]
    public void GettingFridgesTest()
    {
        FridgeService fridgeService = new FridgeService();

        var fridgesResponse = fridgeService.GetFridges();
        
        Assert.AreEqual(StatusResponse.Success, fridgesResponse.StatusResponse);
        Assert.IsNotNull(fridgesResponse.Model);
        
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "samsung 200";
        createFridgeModel.Description = "good fridge";
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        var updateFridgeResponse = fridgeService.GetFridgeById(createFridgeResponse.FridgeId);
        
        Assert.IsNotNull(updateFridgeResponse);
        Assert.AreEqual(createFridgeModel.Model, updateFridgeResponse.Model.Model);
        Assert.AreEqual(createFridgeModel.Description, updateFridgeResponse.Model.Description);
        
        fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
    }


    [TestMethod]
    public void GettingFridgeByIdTest()
    {
        FridgeService fridgeService = new FridgeService();
        
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "atlant 250";
        createFridgeModel.Description = "good fridge";
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        var updateFridgeResponse = fridgeService.GetFridgeById(createFridgeResponse.FridgeId);
        
        Assert.AreEqual(StatusResponse.Error, fridgeService.GetFridgeById(-1).StatusResponse);
        Assert.IsNotNull(updateFridgeResponse);
        Assert.AreEqual(createFridgeModel.Model, updateFridgeResponse.Model.Model);
        Assert.AreEqual(createFridgeModel.Description, updateFridgeResponse.Model.Description);
        
        fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
    }


    [TestMethod]
    public void CreatingFridgeTest()
    {
        FridgeService fridgeService = new FridgeService();

        CreateFridgeModel firstCreateFridgeModel = new CreateFridgeModel();
        firstCreateFridgeModel.Model = "atlant 4";
        firstCreateFridgeModel.Description = "nice fridge";
        var firstCreateFridgeResponse = fridgeService.CreateFridge(firstCreateFridgeModel);
        
        CreateFridgeModel secondCreateFridgeModel = new CreateFridgeModel();
        secondCreateFridgeModel.Model = "atlant 654";
        fridgeService.CreateFridge(secondCreateFridgeModel);
        var secondCreateFridgeResponse = fridgeService.CreateFridge(secondCreateFridgeModel);
        
        Assert.AreEqual(StatusResponse.Error, secondCreateFridgeResponse.StatusResponse);

        var foundFridgeResponse = fridgeService.GetFridgeById(firstCreateFridgeResponse.FridgeId);
        
        Assert.IsNotNull(foundFridgeResponse);
        Assert.AreEqual(firstCreateFridgeModel.Model, foundFridgeResponse.Model.Model);
        Assert.AreEqual(firstCreateFridgeModel.Description, foundFridgeResponse.Model.Description);
        Assert.AreEqual(StatusResponse.Success, firstCreateFridgeResponse.StatusResponse);
        Assert.AreEqual(StatusResponse.Error, fridgeService.CreateFridge(null).StatusResponse);

        fridgeService.DeleteFridge(firstCreateFridgeResponse.FridgeId);
    }

    [TestMethod]
    public void UpdatingFridgeByIdTest()
    {
        FridgeService fridgeService = new FridgeService();
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "samsung 200";
        createFridgeModel.Description = "good fridge";
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        
        UpdateFridgeModel firstUpdateFridgeModel = new UpdateFridgeModel();
        firstUpdateFridgeModel.FridgeId = createFridgeResponse.FridgeId;
        firstUpdateFridgeModel.Model = "update model";
        firstUpdateFridgeModel.Description = "update decsriprion";
        
        ProductModel productModel = new ProductModel();
        productModel.Name = "eggs";
        productModel.CaloriesCount = 56;
        productModel.ExpiryDate = new DateTime(2022, 4, 12);

        var firstUpdateFridgeResponse = fridgeService.UpdateFridge(firstUpdateFridgeModel);
        var foundFridgeResponse = fridgeService.GetFridgeById(createFridgeResponse.FridgeId);
        
        Assert.IsNotNull(foundFridgeResponse);
        Assert.AreEqual(firstUpdateFridgeModel.Model, foundFridgeResponse.Model.Model);
        Assert.AreEqual(firstUpdateFridgeModel.Description, foundFridgeResponse.Model.Description);
        Assert.AreEqual(StatusResponse.Success, firstUpdateFridgeResponse.StatusResponse);
        Assert.AreEqual(StatusResponse.Error, fridgeService.UpdateFridge(null).StatusResponse);
        
        UpdateFridgeModel secondUpdateFridgeModel = new UpdateFridgeModel();
        firstUpdateFridgeModel.Description = "update description";
        var secondUpdateFridgeResponse = fridgeService.UpdateFridge(secondUpdateFridgeModel);
        
        Assert.AreEqual(StatusResponse.Error, secondUpdateFridgeResponse.StatusResponse);
        
        fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
    }


    [TestMethod]
    public void DeletingFridgeTest()
    {
        FridgeService fridgeService = new FridgeService();
        CreateFridgeModel createFridgeModel = new CreateFridgeModel();
        createFridgeModel.Model = "samsung xr5";
        createFridgeModel.Description = "good fridge";
        var createFridgeResponse = fridgeService.CreateFridge(createFridgeModel);
        var deleteFridgeResponse = fridgeService.DeleteFridge(createFridgeResponse.FridgeId);
        var foundFridgeResponse = fridgeService.GetFridgeById(createFridgeResponse.FridgeId);
        
        Assert.IsNull(foundFridgeResponse.Model);
        Assert.AreEqual(StatusResponse.Success, deleteFridgeResponse.StatusResponse);
        Assert.AreEqual(StatusResponse.Error, fridgeService.DeleteFridge(-1).StatusResponse);
    }
}