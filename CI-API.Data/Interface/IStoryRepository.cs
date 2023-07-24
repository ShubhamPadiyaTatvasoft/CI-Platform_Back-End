using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Data.Interface
{
    public interface IStoryRepository
    {

        Task<JsonResult> SaveOrUpdateStory(VolunteerStoryFormViewModel volunteerStoryForm);
        Task<JsonResult> GetMissionVolunteerApproved(long userId);
        Task<JsonResult> GetStoryCards();
        Task<JsonResult> GetStoryDetailsData(long StoryId, long userId);
        Task<JsonResult> GetStoryImage(long StoryId, long userId);

    }
}
