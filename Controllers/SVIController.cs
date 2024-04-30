using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Requests.SVIAddRequest;
using Sabio.Services;
using Sabio.Services.CodingChallenge;
using Sabio.Web.Api.Controllers.CodingChallenge;
using Sabio.Web.Controllers;
using Sabio.Web.Models;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/svi")]
    [ApiController]
    public class SVIController : BaseApiController
    {
        private ISVIService _service = null;

        public SVIController(ISVIService service,
         ILogger<SVIController> logger) : base(logger)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(SVIAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.Add(model);

                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

                result = StatusCode(500, response);
            }
            
            return result;
        }

    }


}
