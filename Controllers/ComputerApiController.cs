using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Computers;
using Sabio.Models.Domain.Customers;
using Sabio.Models.Requests.Computers;
using Sabio.Models.Requests.Customers;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using SendGrid;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/computers")]
    [ApiController]
    public class ComputerApiController : BaseApiController
    {
        private IComputerService _service = null;
        private IAuthenticationService<int> _authService = null;
        public ComputerApiController(IComputerService service,
           ILogger<ComputerApiController> logger,
           IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        //DELETE
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"SqlException Error: {ex.Message}");
            }

            return StatusCode(code, response);
        }

        //UPDATE
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(ComputerUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;           

            try
            {
                _service.Update(model);

                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"SqlException Error: {ex.Message}");
            }

            return StatusCode(code, response);
        }

        //ADD/CREATE/INSERT
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(ComputerAddRequest model)
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

        // GET BY ID
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Computer>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Computer computer = _service.GetById(id);

                if (computer == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<Computer> { Item = computer };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        // GET ALL
        [HttpGet("")]
        public ActionResult<ItemsResponse<Computer>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Computer> computerList = _service.GetAll();

                if (computerList == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource Not Found.");
                }
                else
                {
                    response = new ItemsResponse<Computer> { Items = computerList };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

    }
}
