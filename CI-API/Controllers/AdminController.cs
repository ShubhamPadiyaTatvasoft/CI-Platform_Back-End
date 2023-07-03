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


        #region User

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

        #region UpdateUserData

        [Authorize]
        [HttpPost("UpdateUserData")]
        public async Task<JsonResult> updateUserData([FromBody] UserDetailViewModel userDetailViewModel)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.updateUserData(userDetailViewModel);

            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region DeleteUserData

        [Authorize]
        [HttpPost("DeleteUserData")]
        public async Task<JsonResult> deleteUserData([FromBody] long? userId)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.deleteUser(userId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #endregion

        #region Mission

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

        #region AddUpdateMission
        [Authorize]
        [HttpPost("AddUpdateMission")]
        public async Task<JsonResult> addUpdateMission([FromBody] MissionDataViewModel missionDataViewModel)
        {
            return await AdminService.addUpdateMission(missionDataViewModel);
        }
        #endregion

        #region GetMissionDataFromId
        [Authorize]
        [HttpPost("GetMissionDataFromId")]
        public async Task<JsonResult> getMissionDataFromId([FromBody] long? missionId)
        {
            return await AdminService.getMissionDataFromId(missionId);
        }
        #endregion

        #region DeleteMission
        [Authorize]
        [HttpPost("DeleteMission")]
        public async Task<JsonResult> deleteMission([FromBody] long? missionId)
        {
            return await AdminService.deleteMission(missionId);
        }
        #endregion

        #endregion

        #region CMSPage

        #region GetAllCMSPage
        [Authorize]
        [HttpPost("GetAllCMSPage")]
        public async Task<JsonResult> GetAllCMSPage([FromBody] searchViewModel search)
        {
            if (ModelState.IsValid)
            {
                //return null;
                return await AdminService.GetAllCMSPage(search.search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region AddEditCms
        [Authorize]
        [HttpPost("AddEditCms")]
        public async Task<JsonResult> AddEditCms([FromBody] CmsPage cms)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.AddEditCms(cms);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion

        #region GetCmsDataFromId
        [Authorize]
        [HttpPost("GetCmsDataFromId")]
        public async Task<JsonResult> GetCmsDataFromId([FromBody] long? cmsId)
        {
            if (cmsId != 0)
            {
                return await AdminService.GetCmsDataFromId(cmsId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion

        #region DeleteCms
        [Authorize]
        [HttpPost("DeleteCms")]
        public async Task<JsonResult> DeleteCms([FromBody] long? cmsId)
        {
            if(cmsId != 0)
            {
            return await AdminService.DeleteCms(cmsId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion
        #endregion

        #region common

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

        #endregion

    }
}
