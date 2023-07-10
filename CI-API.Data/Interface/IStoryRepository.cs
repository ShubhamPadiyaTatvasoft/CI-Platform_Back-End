using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Data.Interface
{
    public interface IStoryRepository
    {
        Task<JsonResult> SaveOrUpdateStory(VolunteerStoryFormViewModel volunteerStoryForm, long createdBy);
    }
}
