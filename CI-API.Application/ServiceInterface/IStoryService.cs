using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Application.ServiceInterface
{
    public interface IStoryService
    {
        public Task<JsonResult> SaveOrUpdateStory(VolunteerStoryFormViewModel volunteerStoryForm);
        public Task<JsonResult> GetMissionVolunteerApproved(long userId);
        public Task<JsonResult> GetStoryCards();
        public Task<JsonResult> GetStoryDetailsData(long StoryId, long userId);
        public Task<JsonResult> GetStoryImage(long StoryId, long userId);
    }
}
