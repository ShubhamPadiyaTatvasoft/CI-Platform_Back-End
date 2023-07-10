using CI_API.Common.CommonModels;
using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly CiPlatformDbContext _cIDbContext;

        public StoryRepository(CiPlatformDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }

        public async Task<JsonResult> SaveOrUpdateStory(VolunteerStoryFormViewModel volunteerStoryForm, long createdBy)
        {
            try
            {
                if (volunteerStoryForm.Id == 0)
                {
                    return await addStory(volunteerStoryForm, createdBy);
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.MissionNotApplid, StatusCode = responseStatusCode.InvalidData, Result = true });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }

        private async Task<JsonResult> addStory(VolunteerStoryFormViewModel volunteerStoryForm, long createdBy)
        {         
            Story story = new()
            {
                Status = "draft",
                Description = volunteerStoryForm?.Description,
                Theme = volunteerStoryForm.theme,
                Title = volunteerStoryForm?.Title,
                UserId = createdBy,
                MissionId = volunteerStoryForm.MissionId,
                CreatedAt = DateTime.Now,
            };
            _cIDbContext.Stories.Add(story);
            _cIDbContext.SaveChanges();

            VolunteerStoryInfoViewModel volunteerStoryInfoViewModel = new VolunteerStoryInfoViewModel()
            {
                Theme = story.Theme,
                Description = story.Description,
                Title = story.Title,
                MissionId = story.MissionId,
                Id = story.StoryId
            };

            volunteerStoryInfoViewModel.StoryMediaInfoList = await SaveVolunteerStoryMedia(volunteerStoryForm, story);
            return new JsonResult(new apiResponse<VolunteerStoryInfoViewModel> { Message = ResponseMessages.Usersuccess, StatusCode = responseStatusCode.Success, Data = volunteerStoryInfoViewModel, Result = true });
        }

        private Task<IEnumerable<StoryMediaInfoViewModel>> SaveVolunteerStoryMedia(VolunteerStoryFormViewModel volunteerStoryForm, Story story)
        {
            List<StoryMediaInfoViewModel> StoryMediaInfoList = new List<StoryMediaInfoViewModel>();

            foreach (string image in volunteerStoryForm.Images)
            {
                StoryMedium storyMedium = new StoryMedium()
                {
                    StoryId = story.StoryId,
                    StoryPath = image,
                    StoryType = "aaaa"
                };
                _cIDbContext.StoryMedia.Add(storyMedium);
                _cIDbContext.SaveChanges();
                StoryMediaInfoViewModel storyMediaInfoViewModel = new StoryMediaInfoViewModel()
                {
                    Name = storyMedium.StoryPath,
                    Id = storyMedium.StoryMediaId
                };
                StoryMediaInfoList.Add(storyMediaInfoViewModel);
            }

            return Task.FromResult<IEnumerable<StoryMediaInfoViewModel>>(StoryMediaInfoList);
        }

    }
}
