using API.Helper;
using Core.Dtos.User;
using Core.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAccountManager _accountManager;

        public UserController(ITokenService tokenService, IAccountManager accountManager)
        {
            _tokenService = tokenService;
            _accountManager = accountManager;
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(LoginUserModel usersdata)
        {
            try
            {
                var token = _tokenService.Authenticate(usersdata);
                if (token == null)
                    return Unauthorized();

                return Ok(token);
            }
            catch (ArgumentValidationException ex)
            {
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterUserModel model)
        {
            try
            {
                _accountManager.RegisterUser(model);
                return Ok();
            }
            catch (ArgumentValidationException ex)
            {
                return BadRequest(ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    ErrorHandler.ReturnErrorModel(ex.Message, HttpStatusCode.InternalServerError));
            }

        }
    }
}
