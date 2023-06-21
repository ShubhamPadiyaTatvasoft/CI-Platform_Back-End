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
    public class AdminController : ControllerBase
    {
        #region Dependancy Injection
        private readonly IAdminService AdminService;

        public AdminController(IAdminService _AdminService)
        {
            AdminService = _AdminService;
        }
        #endregion

        #region getAllUser

        [Authorize]
        [HttpPost("GetAllUser")]
        public async Task<JsonResult> GetAllUser([FromBody] searchViewModel search)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetAllUser(search.search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion

        #region GetAllMission
        [Authorize]
        [HttpPost("GetAllMission")]
        public async Task<JsonResult> GetAllMission([FromBody] searchViewModel search)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetAllMission(search.search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region GetListOfCityCountryThemeSkills
        [Authorize]
        [HttpGet("GetListOfCityCountryThemeSkills")]
        public async Task<JsonResult> getListOfCityCountryThemeSkills()
        {
            if (ModelState.IsValid)
            {
                return await AdminService.getListOfCountryTheme();
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region GetListOfCityBasedOnCountry
        [Authorize]
        [HttpPost("GetListOfCityBasedOnCountry")]
        public async Task<JsonResult> getListOfCityBasedOnCountry([FromBody] long? countryId)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.getListOfCityBasedOnCountry(countryId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region GetUserDataFromID
        [Authorize]
        [HttpPost("GetUserDataFromID")]
        public async Task<JsonResult> getUserDataFromID([FromBody] long? userId)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.getUser(userId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion
    }
}
