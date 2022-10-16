using System;

using System.Linq;
using EntityTest.Properties;
using Server.Entities;
using Server.FridgeResponses;
using Server.Models;


namespace Server.Services
{
    public interface IFridgeService
    {
        public FridgesResponse GetFridges();
        public UpdateFridgeResponse GetFridgeById(int fridgeId);
        public CreateFridgeResponse CreateFridge(CreateFridgeModel createFridgeModel);
        public UpdateFridgeResponse UpdateFridge(UpdateFridgeModel updateFridgeModel);
        public DeleteFridgeResponse DeleteFridge(int fridgeId);
    }

    public class FridgeService : IFridgeService
    {
        public CreateFridgeResponse CreateFridge(CreateFridgeModel createFridgeModel)
        {
            CreateFridgeResponse createFridgeResponse = new CreateFridgeResponse();
            if (createFridgeModel == null)
            {
                createFridgeResponse.StatusResponse = StatusResponse.Error;
                return createFridgeResponse;
            }

            if (String.IsNullOrEmpty(createFridgeModel.Description)
                || String.IsNullOrEmpty(createFridgeModel.Model)
               )
            {
                createFridgeResponse.StatusResponse = StatusResponse.Error;
                return createFridgeResponse;
            }

            using (EntityContext db = new EntityContext())
            {
                Fridge fridge = new Fridge();
                fridge.Description = createFridgeModel.Description;
                fridge.Model = createFridgeModel.Model;
                db.Fridges.Add(fridge);
                db.SaveChanges();
                createFridgeResponse.FridgeId = fridge.FridgeId;
            }
            createFridgeResponse.StatusResponse = StatusResponse.Success;
            return createFridgeResponse;
        }

        public FridgesResponse GetFridges()
        {
            FridgesModel fridgesModel = new FridgesModel();
            using (EntityContext db = new EntityContext())
            {
                foreach (var fridge in db.Fridges)
                {
                    FridgeModel fridgeModel = new FridgeModel();
                    fridgeModel.Description = fridge.Description;
                    fridgeModel.Model = fridge.Model;
                    fridgeModel.FridgeId = fridge.FridgeId;
                    fridgesModel.Fridges.Add(fridgeModel);
                }
            }
            FridgesResponse fridgesResponse = new FridgesResponse();
            fridgesResponse.Model = fridgesModel;
            fridgesResponse.StatusResponse = StatusResponse.Success;
            return fridgesResponse;
        }

        public UpdateFridgeResponse GetFridgeById(int fridgeId)
        {
            UpdateFridgeResponse updateFridgeResponse = new UpdateFridgeResponse();
            if (fridgeId <= 0)
            {
                updateFridgeResponse.StatusResponse = StatusResponse.Error;
                return updateFridgeResponse;
            }

            UpdateFridgeModel updateFridgeModel = new UpdateFridgeModel();
            using (EntityContext db = new EntityContext())
            {
                var fridge = db.Fridges.FirstOrDefault(f => f.FridgeId == fridgeId);
                if (fridge != null)
                {
                    updateFridgeModel.Model = fridge.Model;
                    updateFridgeModel.Description = fridge.Description;
                    updateFridgeModel.FridgeId = fridge.FridgeId;
                    
                    updateFridgeResponse.Model = updateFridgeModel;
                    updateFridgeResponse.StatusResponse = StatusResponse.Success;
                    return updateFridgeResponse;
                }
            }
            updateFridgeResponse.StatusResponse = StatusResponse.Error;
            return updateFridgeResponse;
        }

        public UpdateFridgeResponse UpdateFridge(UpdateFridgeModel updateFridgeModel)
        {
            UpdateFridgeResponse updateFridgeResponse = new UpdateFridgeResponse();
            if (updateFridgeModel == null)
            {
                updateFridgeResponse.StatusResponse = StatusResponse.Error;
                return updateFridgeResponse;
            }
            
            if (updateFridgeModel.FridgeId <= 0
                || String.IsNullOrEmpty(updateFridgeModel.Description)
                || String.IsNullOrEmpty(updateFridgeModel.Model))
            {
                updateFridgeResponse.StatusResponse = StatusResponse.Error;
                return updateFridgeResponse;
            }
            
            using (EntityContext db = new EntityContext())
            {
                var fridge = db.Fridges
                    .FirstOrDefault(f => f.FridgeId == updateFridgeModel.FridgeId);
                if (fridge != null)
                {
                    fridge.Model = updateFridgeModel.Model;
                    fridge.Description = updateFridgeModel.Description;
                    db.SaveChanges();
               }
           }
            updateFridgeResponse.StatusResponse = StatusResponse.Success;
            return updateFridgeResponse;
        }

        public DeleteFridgeResponse DeleteFridge(int fridgeId)
        {
            DeleteFridgeResponse deleteFridgeResponse = new DeleteFridgeResponse();

            if (fridgeId <= 0)
            {
                deleteFridgeResponse.StatusResponse = StatusResponse.Error;
                return deleteFridgeResponse;
            }

            using (EntityContext db = new EntityContext())
            {
                var fridge = db.Fridges.FirstOrDefault(p => p.FridgeId == fridgeId);
                if (fridge != null)
                {
                    db.Fridges.Remove(fridge);
                    db.SaveChanges();
                }
            }

            deleteFridgeResponse.StatusResponse = StatusResponse.Success;
            return deleteFridgeResponse;
        }
    }
}