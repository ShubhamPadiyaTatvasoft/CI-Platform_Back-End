﻿using CI_API.Application.ServiceInterface;
using CI_API.Common.CommonModels;
using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingPageController : ControllerBase
    {

        #region Dependancy Injection
        private readonly ILandingPageService landingPageService;

        public LandingPageController(ILandingPageService _landingPageService)
        {
            landingPageService = _landingPageService;
        }
        #endregion

        #region LandingPage

        [Authorize]
        [HttpGet("LandingPage")]
        public async Task<JsonResult> LandingPage()
        {
            return landingPageService.LandingPage();
        }
        #endregion

        #region
        [HttpPost("MissionCardsData")]
        public async Task<JsonResult> GetMissionData([FromBody]GetMissionParamViewModel landingPageViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return await landingPageService.GetMissionCards(landingPageViewModel);
                }
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
            catch (Exception)
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion

        #region
        [HttpPost("FavMissionUpdate")]
        public async Task<JsonResult> FavMission(long MissionId , long UserId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return await landingPageService.FavMissionService(MissionId, UserId);
                }
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
            catch (Exception)
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion

        #region
        [HttpPost("RecommendedMission")]
        public async Task<JsonResult> RecommendedMissionCall(long MissionId, long FromUserId, long ToUserId, string Toemail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return await landingPageService.RecommendedMissionService(MissionId, FromUserId, ToUserId, Toemail);
                }
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
            catch (Exception)
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion
    }
}
