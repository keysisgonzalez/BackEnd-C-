using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using NuGet.ContentModel;
using Sabio.Models;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressApiController : BaseApiController
    {
        private IAddressService _service = null;
        private IAuthenticationService<int> _authService = null;
        public AddressApiController(IAddressService service,
            ILogger<AddressApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        //GET All - api/addresses
        [HttpGet("")]
        public ActionResult<ItemsResponse<Address>> GetAll()
        {
            int code = 200;
            BaseResponse response = null; //do not declare an instance

            try
            {
                List<Address> list = _service.GetRandomAddresses();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Recourse Not Found");
                }
                else
                {
                    response = new ItemsResponse<Address> { Items = list };
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

        //GET BY ID - api/addresses/{id:int}
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Address>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Address address = _service.Get(id);

                if (address == null)
                {
                    iCode = 404;

                    response = new ErrorResponse("Application Resource Not Found");
                }
                else
                {
                    response = new ItemResponse<Address> { Item = address };
                }
            }
            catch (SqlException sqlEx)
            {
                iCode = 500;

                response = new ErrorResponse($"SqlException Error: {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());

            }
            catch (ArgumentException argEx)
            {
                iCode = 500;

                response = new ErrorResponse($"SqlException Error: {argEx.Message}");
            }

            catch (Exception ex)
            {
                iCode = 500;

                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"SqlException Error: {ex.Message}");
                // return base.StatusCode(500, new ErrorResponse(ex.Message));
            }

            return StatusCode(iCode, response);
        }

        //ADD/CREATE/INSERT     
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model)
        {
            
            ObjectResult result = null; //do not declare an instance
            int currentUserId = _authService.GetCurrentUserId();

            try
            {
                int id = _service.Add(model, currentUserId);
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

        //DELETE
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null; //do not declare an instance

            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        //UPDATE
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(AddressUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null; //do not declare an instance

            try
            {
                //id of the new address
                _service.Update(model);

                response = new SuccessResponse();
            }          
            catch (Exception ex)
            {
                iCode = 500;

                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"SqlException Error: {ex.Message}");                
            }
            return StatusCode(iCode, response);
        }


    }
}
