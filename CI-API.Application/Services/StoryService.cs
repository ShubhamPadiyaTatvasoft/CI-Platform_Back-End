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

        public async Task<JsonResult> SaveOrUpdateStory(VolunteerStoryFormViewModel volunteerStoryForm)
        {
            return await _storyRepository.SaveOrUpdateStory(volunteerStoryForm);
        }
        public async Task<JsonResult> GetMissionVolunteerApproved(long userId)
        {
            return await _storyRepository.GetMissionVolunteerApproved(userId);
        }
        public async Task<JsonResult> GetStoryCards()
        {
            return await _storyRepository.GetStoryCards();
        }

        public async Task<JsonResult> GetStoryDetailsData(long StoryId, long userId)
        {
            return await _storyRepository.GetStoryDetailsData(StoryId, userId);
        }

        public async Task<JsonResult> GetStoryImage(long StoryId, long userId)
          {
            return await _storyRepository.GetStoryImage(StoryId, userId);
    }
}
}
