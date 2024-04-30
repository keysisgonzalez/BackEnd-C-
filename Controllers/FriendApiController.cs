using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Microsoft.Extensions.Logging;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using Sabio.Models.Domain.Friends;
using Microsoft.Build.Exceptions;
using System;
using Sabio.Models.Requests.Friends;
using Sabio.Models;
using SendGrid;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/friends")]
    [ApiController]

    //declares a class that inherits the methods from BaseApiController
    //the BaseApiController is a base class that contains functionality for the PAI controllers in the app
    public class FriendApiController : BaseApiController
    {
        //declares a variable that initializes to null and is of type IFriendService that defines a contract for the friend service
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;

        //this is the constructor for the "FriendApiController" class. It takes 3 params which are instances of "IFriendService",
        //ILogger and IAuthenticationService. 
        public FriendApiController(IFriendService service,
           ILogger<FriendApiController> logger,
           IAuthenticationService<int> authService) : base(logger)
        {
            _service = service; //dependency injection: this assigns the "service" param to the variable "_service"
            _authService = authService;//used in the Add/Create method to retreive the ID of the current user
        }

        // GET ALL api/friends
        //this attribute indicates the API method is a Get 
        [HttpGet("")]
        //this public method returns an "ActionResult" object containing a response from the call
        public ActionResult<ItemsResponse<Friend>> GetAll()
        {
            //we're initializing a status code variable "code" with a value of `200` 
            //and the "BaseResponse" object response with a value of null.
            //These will be returned.
            int code = 200;
            BaseResponse response = null;
            
            try
            {
                //calling the method "GetAll" from a service object and assigns the result to a variable of type "List<Friend>" object "friendList"
                List<Friend> friendList = _service.GetAll();

                //this checks if no data(if list is null) is returned from the "GetAll" method
                if (friendList == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource Not Found.");
                }
                //if the list is not null an object will be created with the "Items" property set to "friendList" (the data that we're receiving)
                else
                {                    
                    response = new ItemsResponse<Friend> { Items = friendList };
                }
            }
            //this code will be executed if an exception from the condition is thrown and it will be stored in the variable "ex"
            catch (Exception ex)
            {
                //if an exception caught, an object will be created with the error message 
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            //this returns an HTTP response with the status code and response object
            return StatusCode(code, response);
        }

        //GET BY ID
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Friend>> Get(int id)
        {
            //we're initializing a status code variable "code" with a value of `200` 
            //and the "BaseResponse" object response with a value of null.
            //These will be returned.
            int code = 200;
            BaseResponse response = null;                        

            try
            {
                //calling the method "Get" from a service object and assigning the result to a variable of type "Friend" object "friendList"
                Friend friend = _service.Get(id);

                //this checks if no data(if friend is null) is returned from the "GetAll" method
                if (friend == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource Not Found.");
                }
                //if the list is not null an object will be created with the "Items" property set to "friendList" (the data that we're receiving)
                else
                {
                    response = new ItemResponse<Friend> { Item = friend };
                }
            }
            //this code will be executed if an exception from the condition is thrown and it will be stored in the variable "ex"
            catch (Exception ex)
            {
                //if an exception caught, an object will be created with the error message 
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            //this returns an HTTP response with the status code and response object
            return StatusCode(code, response);
        }

        //ADD/CREATE/INSERT
        //creating the API route
        [HttpPost]
        //declares a method name "Create" that takes a "FriendAddRequest" object as a param and returns an "ActionResult" object containing a response from the call
        public ActionResult<ItemResponse<int>> Create(FriendAddRequest model) 
        {
            //"ObjectResult" is a class that represents a custom result for an action method
            ObjectResult result = null;
            //this line calls the "GetCurrentUserId" method of the "_authService" object, and assigns the returned integer to the `currentId` variable
            int currentId = _authService.GetCurrentUserId();

            try
            {
                //calling the method "Add" from a service object and assigning the returned value to the variable "id"
                int id = _service.Add(model, currentId);

                //creating a new instance of "ItemResponse<int>",
                //setting the "Item" property to the "id" variable declared above,
                //and assinigning the value to the variable "response"
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                // calling the `Created201` method and passing it the response and assigning the value to the variable "result"
                result = Created201(response);
            }
            //this code will be executed if an exception from the condition is thrown and it will be stored in the variable "ex"
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
        public ActionResult<SuccessResponse> Update(FriendUpdateRequest model)
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
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        //PAGINATION
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Friend>>> GetPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Friend> page = _service.GetPage(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Friend>> { Item = page };
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
