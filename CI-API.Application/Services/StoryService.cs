using CI_API.Application.ServiceInterface;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Application.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        public StoryService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public async Task<JsonResult> SaveOrUpdateStory(VolunteerStoryFormViewModel volunteerStoryForm, long createdBy)
        {
            return await _storyRepository.SaveOrUpdateStory(volunteerStoryForm, createdBy);
        }
    }
}
