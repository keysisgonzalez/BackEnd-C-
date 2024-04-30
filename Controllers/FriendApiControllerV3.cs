using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Friends;
using System.Collections.Generic;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Models;
using System.Collections;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v3/friends")]
    [ApiController]
    public class FriendApiControllerV3 : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;

        public FriendApiControllerV3(IFriendService service,
          ILogger<FriendApiControllerV3> logger,
          IAuthenticationService<int> authService) : base(logger)
        {
            _service = service; 
            _authService = authService;
        }

        // GET ALL V3
        [HttpGet("")]
        public ActionResult<ItemsResponse<FriendV3>> GetAllV3()
        {            
            int code = 200;
            BaseResponse response = null;

            try
            {                
                List<FriendV3> friendListV3 = _service.GetAllV3();
                
                if (friendListV3 == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource Not Found.");
                }                
                else
                {
                    response = new ItemsResponse<FriendV3> { Items = friendListV3 };
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

        //GET BY ID V3
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FriendV3>> GetByIdV3(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                FriendV3 friendV3 = _service.GetByIdV3(id);

                if (friendV3 == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<FriendV3> { Item = friendV3 };
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
        public ActionResult<ItemResponse<Paged<FriendV3>>> PaginationV3(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV3> page = _service.PaginationV3(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<FriendV3>> { Item = page };
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

        //SEARCH PAGINATED
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<FriendV3>>> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV3> page = _service.SearchPaginatedV3(pageIndex, pageSize, query);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<FriendV3>> { Item = page };
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
