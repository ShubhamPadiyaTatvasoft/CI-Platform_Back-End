using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Application.ServiceInterface
{
    public interface IStoryService
    {
        public Task<JsonResult> SaveOrUpdateStory(VolunteerStoryFormViewModel volunteerStoryForm, long createdBy);
    }
}
