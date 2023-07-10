using CI_API.Application.ServiceInterface;
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
        public async Task<JsonResult> UpsertStory([FromForm] VolunteerStoryFormViewModel volunteerStoryForm,long userId)
        {
      
            return await storyService.SaveOrUpdateStory(volunteerStoryForm, createdBy: userId);
        }      
    }
}
