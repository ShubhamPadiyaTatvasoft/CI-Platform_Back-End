using CI_API.Application.ServiceInterface;
using CI_API.Common.CommonModels;
using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService storyService;

        public StoryController(IStoryService _storyService)
        {
            storyService = _storyService;
        }

        [HttpPost("SaveOrUpdate")]
        public async Task<JsonResult> UpsertStory([FromForm] VolunteerStoryFormViewModel volunteerStoryForm)
        {      
            return await storyService.SaveOrUpdateStory(volunteerStoryForm);
        }

        [HttpGet("ListOfMission")]
        public async Task<JsonResult> ListOfMission(long userId)
        {
            return await storyService.GetMissionVolunteerApproved(userId);
        }

        [HttpGet("StoryCardsData")]
        public async Task<JsonResult> GetStoryData()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await storyService.GetStoryCards();
                }
                catch (Exception)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }

            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }

        [HttpGet("StoryDetailsData/{storyID}")]
        public async Task<JsonResult> GetStoryDetailsData(long userId, long StoryId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await storyService.GetStoryDetailsData(StoryId, userId);
                }
                catch (Exception)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }

            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }

        [HttpGet("StoryImage/{storyID}")]
        public async Task<JsonResult> GetStoryImage(long userId, long StoryId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await storyService.GetStoryImage(StoryId, userId);
                }
                catch (Exception)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }

            }
            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
        }
    }
}
