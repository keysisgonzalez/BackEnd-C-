using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Models.Domain.Friends;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System;
using Sabio.Models.Domain.Computers;
using Sabio.Models;
using System.Collections;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v3/computers")]
    [ApiController]
    public class ComputerApiControllerV3 : BaseApiController
    {
        private IComputerService _service = null;
        private IAuthenticationService<int> _authService = null;

        public ComputerApiControllerV3(IComputerService service,
          ILogger<ComputerApiControllerV3> logger,
          IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        //SEARCH PAGINATED
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<ComputerV3>>> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<ComputerV3> page = _service.SearchPaginatedV3(pageIndex, pageSize, query);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<ComputerV3>> { Item = page };
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
        public ActionResult<ItemResponse <Paged<ComputerV3>>> PaginationV3(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<ComputerV3> page = _service.PaginationV3(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<ComputerV3>> { Item = page };
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
        public ActionResult<ItemResponse<ComputerV3>> GetByIdV3(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                ComputerV3 computertV3 = _service.GetByIdV3(id);

                if (computertV3 == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<ComputerV3> { Item = computertV3 };
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

        // GET ALL V3
        [HttpGet("")]
        public ActionResult<ItemsResponse<ComputerV3>> GetAllV3()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<ComputerV3> computerListV3 = _service.GetAllV3();

                if (computerListV3 == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource Not Found.");
                }
                else
                {
                    response = new ItemsResponse<ComputerV3> { Items = computerListV3 };
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
