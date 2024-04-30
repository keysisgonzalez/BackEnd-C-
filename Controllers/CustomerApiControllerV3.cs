using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System;
using Sabio.Models.Domain.Customers;
using Sabio.Models.Domain.Friends;
using System.Data.SqlClient;
using System.Data;
using Sabio.Models;
using System.Collections;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v3/customers")]
    [ApiController]
    public class CustomerApiControllerV3 : BaseApiController
    {
        private ICustomerService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CustomerApiControllerV3(ICustomerService service,
          ILogger<CustomerApiControllerV3> logger,
          IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        // GET ALL V3
        [HttpGet("")]
        public ActionResult<ItemsResponse<CustomerV3>> GetAllV3()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<CustomerV3> customerListV3 = _service.GetAllV3();

                if (customerListV3 == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource Not Found.");
                }
                else
                {
                    response = new ItemsResponse<CustomerV3> { Items = customerListV3 };
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

        // GET BY ID V3
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<CustomerV3>> GetByIdV3(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                CustomerV3 customerV3 = _service.GetByIdV3(id);

                if (customerV3 == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<CustomerV3> { Item = customerV3 };
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

        //PAGINATION
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<CustomerV3>>> PaginationV3(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<CustomerV3> page = _service.PaginationV3(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<CustomerV3>> { Item = page };
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

        //PAGINATION
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<CustomerV3>>> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<CustomerV3> page = _service.SearchPaginatedV3(pageIndex, pageSize, query);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<CustomerV3>> { Item = page };
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
