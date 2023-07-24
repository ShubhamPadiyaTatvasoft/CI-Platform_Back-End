using CI_API.Common.CommonModels;
using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoryRepository(CiPlatformDbContext cIDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _cIDbContext = cIDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<JsonResult> SaveOrUpdateStory(VolunteerStoryFormViewModel volunteerStoryForm)
        {
            try
            {
                if (volunteerStoryForm.Id == 0)
                {
                    return await addStory(volunteerStoryForm);
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

        private async Task<JsonResult> addStory(VolunteerStoryFormViewModel volunteerStoryForm)
        {
            Story story = new()
            {
                Status = "draft",
                Description = volunteerStoryForm?.Description,
                Theme = volunteerStoryForm.theme,
                Title = volunteerStoryForm?.Title,
                UserId = volunteerStoryForm.UserId,
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
            return new JsonResult(new apiResponse<VolunteerStoryInfoViewModel> { Message = ResponseMessages.StorySavesuccess, StatusCode = responseStatusCode.Success, Data = volunteerStoryInfoViewModel, Result = true });
        }

        private async Task<IEnumerable<StoryMediaInfoViewModel>> SaveVolunteerStoryMedia(VolunteerStoryFormViewModel volunteerStoryForm, Story story)
        {
            List<StoryMediaInfoViewModel> StoryMediaInfoList = new List<StoryMediaInfoViewModel>();

            foreach (var file in volunteerStoryForm.Images)
            {
                var fileType = Path.GetExtension(file.FileName);
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images\uploadfiles", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                StoryMedium storyMedium = new StoryMedium()
                {
                    StoryId = story.StoryId,
                    StoryPath = "/Images/uploadfiles/" + fileName,
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

            return await Task.FromResult<IEnumerable<StoryMediaInfoViewModel>>(StoryMediaInfoList);
        }

        public async Task<JsonResult> GetMissionVolunteerApproved(long userId)
        {
            bool isUserIdAvailable = await Task.FromResult(_cIDbContext.MissionApplications.Any(x => x.UserId == userId));
            if (isUserIdAvailable)
            {
                List<MissionApplication> missionApplicationsApproved = await Task.FromResult(_cIDbContext.MissionApplications.Where(x => x.UserId == userId && x.ApprovalStatus == "approved").Include(x => x.Mission).Include(x => x.User).ToList());
                return new JsonResult(new apiResponse<List<MissionApplication>> { StatusCode = responseStatusCode.Success, Data = missionApplicationsApproved, Result = true });
            }
            else
            {
                return new JsonResult(new apiResponse<List<MissionApplication>> { Message = ResponseMessages.MissionNotFound, StatusCode = responseStatusCode.NotFound, Data = null, Result = false });
            }
        }

        public async Task<JsonResult> GetStoryCards()
        {
            try
            {
                var stories = (
                    from s in _cIDbContext.Stories
                    join u in _cIDbContext.Users on s.UserId equals u.UserId
                    join sm in _cIDbContext.StoryMedia on s.StoryId equals sm.StoryId into storyMediaJoin
                    from sm in storyMediaJoin.Where(sm => sm.StoryType == "image/png" || sm.StoryType == "jpeg" || sm.StoryType == "jpg" || sm.StoryType == "img" || sm.StoryType == "png").Take(1).DefaultIfEmpty()
                    where s.Status == "approved"
                    orderby s.PublishedAt ascending
                    select new StoryDataViewModel
                    {
                        StoryId = s.StoryId,
                        Title = s.Title,
                        Description = s.Description,
                        StoryImage = sm.StoryPath,
                        Avatar = u.Avatar,
                        UserName = u.FirstName + " " + u.LastName,
                        PublishedAt = s.PublishedAt,
                        Theme = s.Theme,
                    }
                ).ToList();

                return new JsonResult(new apiResponse<List<StoryDataViewModel>>
                {
                    Message = ResponseMessages.Success,
                    Data = stories,
                    Result = true,
                    StatusCode = responseStatusCode.Success
                });
            }
            catch (Exception)
            {
                return new JsonResult(new apiResponse<string>
                {
                    Message = ResponseMessages.InternalServerError,
                    StatusCode = responseStatusCode.BadRequest,
                    Result = false
                });
            }
        }

        public async Task<JsonResult> GetStoryDetailsData(long StoryId, long userId)
        {
            try
            {
                StoryDetailsViewModel storyDetails = new StoryDetailsViewModel();
                storyDetails.StoryId = StoryId;
               
                Story? story = _cIDbContext.Stories.FirstOrDefault(x => x.StoryId == StoryId);
                User user = _cIDbContext.Users.FirstOrDefault(x => x.UserId == userId);

                if (story == null || user == null)
                {
                    return new JsonResult(new apiResponse<string>
                    {
                        Message = ResponseMessages.IdNotFound,
                        StatusCode = responseStatusCode.NotFound,
                        Result = false
                    });
                }

                storyDetails.Avatar = user.Avatar;
                storyDetails.VolunteerName = user.FirstName + " " + user.LastName;
                storyDetails.StoryTitle = story.Title;
                storyDetails.VolunteerStoryDescription = story.Description;
                if (user.WhyIVolunteer != null)
                {
                    storyDetails.WhyIVolunteer = user.WhyIVolunteer;
                }

                storyDetails.MissionId = story.MissionId;
                if (story != null)
                {
                    storyDetails.StoryView = story.Views + 1;
                    _cIDbContext.SaveChanges();
                }

                story.Views = storyDetails.StoryView;
               
                _cIDbContext.SaveChanges();
                _cIDbContext.Stories.Update(story);

                return new JsonResult(new apiResponse<StoryDetailsViewModel>
                {
                    Message = ResponseMessages.Success,
                    Data = storyDetails,
                    Result = true,
                    StatusCode = responseStatusCode.Success
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new apiResponse<string>
                {
                    Message = ResponseMessages.InternalServerError,
                    StatusCode = responseStatusCode.BadRequest,
                    Result = false
                });
            }
        }

        public async Task<JsonResult> GetStoryImage(long StoryId, long userId)
        {
            try
            {
                StoryDetailsViewModel storyDetails = new StoryDetailsViewModel();
                storyDetails.StoryId = StoryId;
                storyDetails.StoryImages = new List<string>();

                Story? story = _cIDbContext.Stories.FirstOrDefault(x => x.StoryId == StoryId);
                User user = _cIDbContext.Users.FirstOrDefault(x => x.UserId == userId);

                if (story == null || user == null)
                {
                    return new JsonResult(new apiResponse<string>
                    {
                        Message = ResponseMessages.IdNotFound,
                        StatusCode = responseStatusCode.NotFound,
                        Result = false
                    });
                }

                List<string> storyImages = _cIDbContext.StoryMedia
                    .Where(x => x.StoryId == StoryId)
                    .Select(x => x.StoryPath)
                    .ToList();

                storyDetails.StoryImages = storyImages;

                _cIDbContext.SaveChanges();
                _cIDbContext.Stories.Update(story);

                return new JsonResult(new apiResponse<StoryDetailsViewModel>
                {
                    Message = ResponseMessages.Success,
                    Data = storyDetails,
                    Result = true,
                    StatusCode = responseStatusCode.Success
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new apiResponse<string>
                {
                    Message = ResponseMessages.InternalServerError,
                    StatusCode = responseStatusCode.BadRequest,
                    Result = false
                });
            }
        }
    }
}

