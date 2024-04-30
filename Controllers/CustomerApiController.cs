using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Customers;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Customers;
using Sabio.Models.Requests.Friends;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerApiController : BaseApiController
    {
        private ICustomerService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CustomerApiController(ICustomerService service,
            ILogger<CustomerApiController> logger,
            IAuthenticationService<int> authService) :base(logger)
        {
            _service = service;
            _authService = authService;
        }

        // GET ALL
        [HttpGet("")]
        public ActionResult<ItemsResponse<Customer>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Customer> customerList = _service.GetAll();

                if(customerList == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource Not Found.");
                }
                else
                {
                    response = new ItemsResponse<Customer> { Items = customerList };
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

        // GET BY ID
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Customer>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Customer customer = _service.GetById(id);

                if (customer == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<Customer> { Item = customer };
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

        //ADD/CREATE/INSERT
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(CustomerAddRequest model)
        {
            ObjectResult result = null;

            int currentId = _authService.GetCurrentUserId();

            try
            {
                int id = _service.Add(model, currentId);

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

        //UPDATE
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(CustomerUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            int currentId = _authService.GetCurrentUserId();

            try
            {
                _service.Update(model, currentId);

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


    }
}
