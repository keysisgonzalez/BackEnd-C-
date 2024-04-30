using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]

    //This line declares a public class named `UserApiController` that inherits from `BaseApiController`.
    ////The `BaseApiController` is a base class contains common functionality for all API controllers in the application.
    public class UserApiController : BaseApiController
    {
        //This line declares a private field of type `IUserServiceV1` named `_service` and initializes it to `null`.
        //`IUserServiceV1` is an interface that defines a contract for the user service, which includes methods for performing operations related to users.
        private IUserServiceV1 _service = null;

        //is an interface that defines a contract for the authentication service, which includes methods for performing authentication-related operations.
        private IAuthenticationService<int> _authService = null;

        //This is the constructor for the `UserApiController` class. It takes three parameters: an instance of `IUserServiceV1`,
        //an instance of `ILogger<UserApiController>`, and an instance of `IAuthenticationService<int>`.
        //The `: base(logger)` part calls the base class constructor with the `logger` parameter.
        public UserApiController(IUserServiceV1 service,
            ILogger<UserApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            //This assigns the `service` parameter to the `_service` field. This is called dependency injection,
            //which allows the `UserApiController` to use the methods defined by the `IUserServiceV1` interface.
            _service = service;
            
            _authService = authService; //used in the Add/Create method to retreive the ID of the current user

        }

        //GET All
        //this attribute indicates the API method is a Get
        [HttpGet]
        //server side validation
        //It is a public method named `GetAll` that returns an `ActionResult` object containing an `ItemsResponse<User>`. 
        //`ItemsResponse<User>` is a generic type where `User` is the type of items in the response.
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            //we're initializing a status code variable `code` with a value of `200` 
            //and the `BaseResponse` object response with a value of null.
            //These will be returned.
            int code = 200;
            BaseResponse response = null;

            //`try`: This keyword starts a block of code that will be attempted (or "tried") for execution. 
            //If any exceptions occur within this block, they will be caught and handled by the `catch` block.
            try
            {
                // This line is calling a method `GetAll` from a service object `_service`, and assigns the result to a `List < User >` object `list`
                List<User> list = _service.GetAll();

                //If there's any data a 404 will be returned
                //This line checks if the `list` is `null`. If it is, it means that no data was returned from the `GetAll` method.
                if (list == null)
                {
                    //If the `list` is `null`, the status code is set to `404` (HTTP status code for "Not Found") and an `ErrorResponse` object is created with a message of "App Resource Not Found".
                    code = 404;
                    response = new ErrorResponse("App Resourse Not Found.");
                }
                else
                {
                    //If the `list` is not `null`, an `ItemsResponse<User>` object is created with the `Items` property set to the `list`.
                    response = new ItemsResponse<User> { Items = list };
                }
            }
            //This block of code will be executed if an exception is thrown in the `try` block. The caught exception is stored in the variable `ex`.
            catch (Exception ex)
            {
                // If an exception is caught, the status code is set to `500` (HTTP status code for "Internal Server Error") and an `ErrorResponse` object is created with the message set to the exception's message.
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            //This line returns an HTTP response with the status code and response object that were set in the previous lines. If no exceptions occurred and data was found, it will return a 200 status code and the data.
            //If no data was found, it will return a 404 status code and an error message. If an exception occurred, it will return a 500 status code and the exception's message.
            return StatusCode(code, response);

        }
        #region Basic GET ALL Code        
        //This is the declaration of a public method named `GetAll`
        //which returns an `ActionResult` that wraps an `ItemsResponse<User>`. 
        //public ActionResult<ItemsResponse<User>> GetAll()
        //{
        //// This line is calling the `GetAll` method from a UserService,
        //// which is expected to return a list of User objects. The result is stored in the `list` variable.
        //    List<User> list = _service.GetAll();

        ////A new instance of `ItemsResponse<User>` is being created and assigned to the `response` variable.
        ////`ItemsResponse < User >` is a generic class used to standardize the structure of the response sent by the API.

        //    ItemsResponse<User> response = new ItemsResponse<User>();
        ////This line is assigning the list of User objects retrieved from the service to the `Items` property of the `response` object. 
        //response.Items = list;

        //    if (list == null)
        //    {
        //        return NotFound404(response);
        //    }
        //    else
        //    {
        //        return Ok(response);}
        //    } 
        #endregion

        //GET BY ID
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<User>> Get(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                User user = _service.Get(id);                

                if (user == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<User> { Item = user };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        #region Basic GET BY ID code 
        //public ActionResult<ItemResponse<User>> Get(int id)
        //{
        //    User user = _service.Get(id);
        //    ItemResponse<User> response = new ItemResponse<User>();
        //    response.Item = user;

        //    if (response.Item == null)
        //    {
        //        return NotFound404(response);
        //    }
        //    else
        //    {
        //        return Ok(response);
        //    }
        //} 
        #endregion

        //ADD/CREATE/INSERT
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {
            ObjectResult result = null;            

            try
            {             
                int id = _service.Add(model);
                ItemResponse<int> response = new ItemResponse<int>() {Item = id};

                result = Created201(response);
            }
            catch (Exception ex)
            {                
                ErrorResponse response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());

                result = StatusCode(500, response);

            }
            return result;
        }

        #region Basic ADD/CREATE/INSERT
        //public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        //{
        //    int id = _service.Add(model);

        //    ItemResponse<int> response = new ItemResponse<int>();

        //    response.Item = id;

        //    return Ok(response);
        //} 
        #endregion

        //UPDATE
        [HttpPut("{id:int}")]

        public ActionResult<SuccessResponse> Update(UserUpdateRequest model)
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

    }
}
