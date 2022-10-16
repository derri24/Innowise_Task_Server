using Microsoft.AspNetCore.Mvc;
using Server.FridgeResponses;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FridgeController : ControllerBase
    {
        private IFridgeService _fridgeService;

        public FridgeController(IFridgeService fridgeService)
        {
            _fridgeService = fridgeService;
        }

        [HttpGet]
        public FridgesResponse GetFridges()
        {
            return _fridgeService.GetFridges();
        }

        [HttpGet]
        public UpdateFridgeResponse GetFridgeById(int fridgeId)
        {
            return _fridgeService.GetFridgeById(fridgeId);
        }
        
        [HttpPost]
        public CreateFridgeResponse CreateFridge([FromBody] CreateFridgeModel createFridgeModel)
        {
            return _fridgeService.CreateFridge(createFridgeModel);
        }
        
        [HttpPut]
        public UpdateFridgeResponse UpdateFridge([FromBody] UpdateFridgeModel updateFridgeModel)
        {
            return _fridgeService.UpdateFridge(updateFridgeModel);
        }
        
        [HttpDelete]
        public DeleteFridgeResponse DeleteFridge(int fridgeId)
        {
           return  _fridgeService.DeleteFridge(fridgeId);
        }
    }
}