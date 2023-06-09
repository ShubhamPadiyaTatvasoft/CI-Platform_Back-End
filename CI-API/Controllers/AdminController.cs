﻿using CI_API.Application.ServiceInterface;
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
        /// <summary>
        /// data of all the users
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
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
        /// <summary>
        /// returns userData from userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// it updates the user information
        /// </summary>
        /// <param name="userDetailViewModel"></param>
        /// <returns></returns>
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
        /// <summary>
        /// it deletes the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// get data of all the missions
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
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
        /// <summary>
        /// it updates the info of mission
        /// </summary>
        /// <param name="missionDataViewModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddUpdateMission")]
        public async Task<JsonResult> addUpdateMission([FromBody] MissionDataViewModel missionDataViewModel)
        {
            return await AdminService.addUpdateMission(missionDataViewModel);
        }
        #endregion

        #region GetMissionDataFromId
        /// <summary>
        /// get mission data from mission id For updation
        /// </summary>
        /// <param name="missionId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetMissionDataFromId")]
        public async Task<JsonResult> getMissionDataFromId([FromBody] long? missionId)
        {
            return await AdminService.getMissionDataFromId(missionId);
        }
        #endregion

        #region DeleteMission
        /// <summary>
        /// delete the mission
        /// </summary>
        /// <param name="missionId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// get data of all the cms pages
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllCMSPage")]
        public async Task<JsonResult> GetAllCMSPage(string? search)
        {
            if (ModelState.IsValid)
            {
                //return null;
                return await AdminService.GetAllCMSPage(search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region AddEditCms
        /// <summary>
        /// add or edit the cms data
        /// </summary>
        /// <param name="cms"></param>
        /// <returns></returns>
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
        /// <summary>
        /// get data of perticular cms page from its id
        /// </summary>
        /// <param name="cmsId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetCmsDataFromId/{cmsId}")]
        public async Task<JsonResult> GetCmsDataFromId(long? cmsId)
        {
            if (cmsId != 0)
            {
                return await AdminService.GetCmsDataFromId(cmsId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion

        #region DeleteCms
        /// <summary>
        /// delete the cms data
        /// </summary>
        /// <param name="cmsId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("DeleteCms")]
        public async Task<JsonResult> DeleteCms([FromBody] long? cmsId)
        {
            if (cmsId != 0)
            {
                return await AdminService.DeleteCms(cmsId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion

        #endregion

        #region missionApplication

        #region GetAllMissionApplication
        /// <summary>
        /// it gets all the data of mission applications
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllMissionApplication")]
        public async Task<JsonResult> GetAllMissionApplication(string? search)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetAllMissionApplication(search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }

        #endregion

        #region ApproveRejectMissionApplication
        /// <summary>
        /// it approve or reject the mission appplication
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ApproveRejectMissionApplication")]
        public async Task<JsonResult> ApproveRejectMissionApplication([FromBody] MissionApplicationViewModel application)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.ApproveRejectMissionApplication(application);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #endregion

        #region story

        #region GetAllStories
        /// <summary>
        /// get data of all the stories
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllStories")]
        public async Task<JsonResult> GetAllStories(string? search)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetAllStories(search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region ApproveRejectDeleteStory
        /// <summary>
        /// approve or reject the story
        /// </summary>
        /// <param name="storyData"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ApproveRejectDeleteStory")]
        public async Task<JsonResult> ApproveRejectDeleteStory([FromBody] AdminPanelStoryViewModel storyData)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.ApproveRejectDeleteStory(storyData);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #endregion

        #region banner

        #region GetAllBanners
        /// <summary>
        /// return all the banner's data
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>

        [HttpGet("GetAllBanners")]
        public async Task<JsonResult> GetAllBanners(string? search)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetAllBanners(search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region AddUpdateBanner
        /// <summary>
        /// for adding or updating the banner data
        /// </summary>
        /// <param name="bannerData"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddUpdateBanner")]
        public async Task<JsonResult> AddUpdateBanner(BannerDataViewModel bannerData)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.AddUpdateBanner(bannerData);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region GetBannerDataFromId
        /// <summary>
        /// get the banner data from its id
        /// </summary>
        /// <param name="bannerId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetBannerDataFromId/{bannerId}")]
        public async Task<JsonResult> GetBannerDataFromId(long? bannerId)
        {
            if (bannerId != 0)
            {
                return await AdminService.GetBannerDataFromId(bannerId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion

        #region DeleteBanner
        /// <summary>
        /// for deleting the banner data
        /// </summary>
        /// <param name="bannerId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("DeleteBanner")]
        public async Task<JsonResult> DeleteBanner(long? bannerId)
        {
            if (bannerId != 0)
            {
                return await AdminService.DeleteBanner(bannerId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion

        #endregion

        #region theme

        #region GetAllThemes
        /// <summary>
        /// return all themes's data
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllThemes")]
        public async Task<JsonResult> GetAllThemes(string? search)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetAllThemes(search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region GetThemeData
        /// <summary>
        /// return theme data from its's id
        /// </summary>
        /// <param name="themeId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetThemeData/{themeId}")]
        public async Task<JsonResult> GetThemeData(long? themeId)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetThemeData(themeId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region AddUpdateTheme
        /// <summary>
        /// add or update the theme data
        /// </summary>
        /// <param name="themeData"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddUpdateTheme")]
        public async Task<JsonResult> AddUpdateTheme(AdminPanelThemeSkillViewModel themeData)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.AddUpdateTheme(themeData);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region DeleteTheme
        /// <summary>
        /// delete theme data
        /// </summary>
        /// <param name="themeId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("DeleteTheme/{themeId}")]
        public async Task<JsonResult> DeleteTheme(long? themeId)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.DeleteTheme(themeId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #endregion

        #region skill

        #region GetAllSkills
        /// <summary>
        /// get all skills's data
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllSkills")]
        public async Task<JsonResult> GetAllSkills(string? search)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetAllSkills(search);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region GetSkillData
        /// <summary>
        /// get skill data from it's id
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetSkillData/{skillId}")]
        public async Task<JsonResult> GetSkillData(long? skillId)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.GetSkillData(skillId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region AddUpdateSkill
        /// <summary>
        /// add or update the skill data
        /// </summary>
        /// <param name="themeData"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddUpdateSkill")]
        public async Task<JsonResult> AddUpdateSkill(AdminPanelThemeSkillViewModel themeData)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.AddUpdateSkill(themeData);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #region DeleteSkill
        /// <summary>
        /// delete skill data
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("DeleteSkill/{skillId}")]
        public async Task<JsonResult> DeleteSkill(long? skillId)
        {
            if (ModelState.IsValid)
            {
                return await AdminService.DeleteSkill(skillId);
            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
        #endregion

        #endregion

        #region common

        #region GetListOfCtyCountryThemeSkills
        /// <summary>
        /// get all the country theme skills
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// get city based on country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
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
