using CI_API.Application.ServiceInterface;
using CI_API.Common.CommonModels;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Dependancy Injection
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }
        #endregion

        #region Login
        [HttpPost("Login")]
        public async Task<JsonResult> Login([FromBody] LoginViewModel userDetail)
        {
            if (ModelState.IsValid)
            {
                return await userService.Login(userDetail);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region Register
        [HttpPost("Register")]
        public async Task<JsonResult> Register([FromBody] RegisterViewModel userDetail)
        {
            if (ModelState.IsValid)
            {
                var model = await userService.Register(userDetail);
                return model;
            }
            else
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion

        #region ForgetPassword
        [HttpPost("ForgetPassword/{email}")]
        public async Task<JsonResult> ForgetPassword(string email)
        {
            if (email != null)
            {
                return await userService.ForgetPassword(email);
            }
            else
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion

        #region ResetPassword
        [HttpPost("ResetPassword/{email}/{token}/{newPassword}")]
        public async Task<JsonResult> ResetPassword(string email, string token, string newPassword)
        {
            if (email != null || token != null || newPassword != null)
            {
                return await userService.ResetPassword(email, token, newPassword);
            }
            else
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }

        #endregion

       

    }
}
