using CI_API.Common.CommonMethods;
using CI_API.Common.CommonModels;
using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Repository
{
    public class VolunteerMissionRepository : IVolunteerMissionRepository
    {
        #region Dependency Injection of DbContext 

        private readonly CiPlatformDbContext _cIDbContext;

        public VolunteerMissionRepository(CiPlatformDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }
        #endregion

        #region GetMissionData
        public async Task<JsonResult> AllMissiondata(long missionId, long userId)
        {
            try
            {
                var missionData =await Task.FromResult(_cIDbContext.Missions.Where(m => m.MissionId == missionId).FirstOrDefault()) ;

                if (missionData != null)
                {
                    var data1 = await Task.FromResult(from mission in _cIDbContext.Missions
                                                      join city in _cIDbContext.Cities on mission.CityId equals city.CityId
                                                      join contry in _cIDbContext.Countries on mission.CountryId equals contry.CountryId
                                                      join theme in _cIDbContext.MissionThemes on mission.ThemeId equals theme.MissionThemeId
                                                      join mapp in _cIDbContext.MissionApplications on mission.MissionId equals mapp.MissionId
                                                      join goal in _cIDbContext.GoalMissions on mission.MissionId equals goal.MissionId into goalMissions
                                                      from goal in goalMissions.DefaultIfEmpty()
                                                      join mskill in _cIDbContext.MissionSkills on mission.MissionId equals mskill.MissionId
                                                      join fmission in _cIDbContext.FavoriteMissions on mission.MissionId equals fmission.MissionId
                                                      join skillll in _cIDbContext.Skills on mskill.SkillId equals skillll.SkillId
                                                      join rating in _cIDbContext.MissionRatings on mission.MissionId equals rating.MissionId
                                                      where mission.MissionId.Equals(missionId)

                                                      select new VolunteerMissionViewModel
                                                      {
                                                          missionId = mission.MissionId,
                                                          title = mission.Title,
                                                          shortDescription = mission.ShortDescription,
                                                          description = mission.Description,
                                                          startDate = mission.StartDate,
                                                          endDate = mission.EndDate,
                                                          countryName = contry.Name,
                                                          cityName = city.Name,
                                                          themeName = theme.Title,
                                                          missionType = mission.MissionType,
                                                          organizationName = mission.OrganizationName,
                                                          organizationDetail = mission.OrganizationDetail,
                                                          leftSeats = mission.TotalSeats - (from c in _cIDbContext.MissionApplications where c.MissionId == missionId select c).Count(),
                                                          goalObjectiveText = goal != null ? goal.GoalObjectiveText : String.Empty,
                                                          goalValue = goal != null ? goal.GoalValue : String.Empty,
                                                          ratings = (from r in _cIDbContext.MissionRatings
                                                                     where r.MissionId.Equals(missionId) && r.UserId.Equals(userId)
                                                                     select r.Rating).FirstOrDefault(),
                                                          IsFavMission = (from fmission in _cIDbContext.FavoriteMissions
                                                                          where fmission.MissionId == missionId && fmission.UserId == userId
                                                                          select true).Any(),
                                                         
                                                      });


                    return new JsonResult(new apiResponse<VolunteerMissionViewModel> { Message = ResponseMessages.AllMissionSuccess, StatusCode = responseStatusCode.Success, Data = data1.FirstOrDefault(), Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<VolunteerMissionViewModel> { Message = ResponseMessages.IdNotFound, StatusCode = responseStatusCode.InvalidData, Result = true });
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

        #region GetRecentVolunteer
        public async Task<JsonResult> GetRecentVolunteer(int userId)
        {
            try
            {
                var isUser = await Task.FromResult(_cIDbContext.Users.Where(u => u.UserId == userId).FirstOrDefault());
                if (isUser != null)
                {
                    var getrecent = (from user in _cIDbContext.Users
                                     where user.UserId != userId
                                     select new UserDetailViewModel
                                     {
                                         firstName = user.FirstName,
                                         lastName = user.LastName,
                                         avatar = user.Avatar
                                     }).ToList();
                    return new JsonResult(new apiResponse<List<UserDetailViewModel>> { Message = ResponseMessages.Usersuccess, StatusCode = responseStatusCode.Success, Data = getrecent, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.IdNotFound, StatusCode = responseStatusCode.InvalidData, Result = true });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetUserDetails
        public async Task<JsonResult> GetUserDetails(int userId)
        {
            try
            {
                var isUser =await Task.FromResult(_cIDbContext.Users.Where(u => u.UserId == userId).FirstOrDefault());
                if (isUser != null)
                {
                    var getuserdetails =await Task.FromResult( (from user in _cIDbContext.Users
                                          where user.UserId != userId
                                          select new UserDetailViewModel
                                          {
                                              userId = user.UserId,
                                              firstName = user.FirstName,
                                              lastName = user.LastName,
                                              email = user.Email,
                                              avatar = user.Avatar

                                          }).ToList());
                    return new JsonResult(new apiResponse<List<UserDetailViewModel>> { Message = ResponseMessages.Usersuccess, StatusCode = responseStatusCode.Success, Data = getuserdetails, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.IdNotFound, StatusCode = responseStatusCode.InvalidData, Result = true });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetComments
        public async Task<JsonResult> GetComment(int missionId)
        {
            try
            {
                var isMission =await Task.FromResult(_cIDbContext.Missions.Where(m => m.MissionId == missionId).FirstOrDefault());
                if (isMission != null)
                {
                    var getComment =await Task.FromResult((from comment in _cIDbContext.Comments
                                      join mission in _cIDbContext.Missions on comment.MissionId equals mission.MissionId
                                      join user in _cIDbContext.Users on comment.UserId equals user.UserId
                                      where comment.MissionId == missionId
                                      select new VolunteerMissionViewModel
                                      {
                                          commentText = comment.CommentText,
                                          userId = comment.UserId,
                                          missionId = missionId,
                                          approvalStatus = comment.ApprovalStatus,
                                          avatar = user.Avatar,
                                          createdAtcomment = comment.CreatedAt
                                      }).ToList());
                    return new JsonResult(new apiResponse<List<VolunteerMissionViewModel>> { Message = ResponseMessages.LoginSuccess, StatusCode = responseStatusCode.Success, Data = getComment, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.IdNotFound, StatusCode = responseStatusCode.InvalidData, Result = true });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AddToFavourite
        public async Task<JsonResult> AddToFavourite(int missionId, int userId)
        {
            try
            {
                var mission =await Task.FromResult(_cIDbContext.Missions.Where(m => m.MissionId == missionId).FirstOrDefault());
                if (mission != null)
                {
                    var mid = await Task.FromResult(_cIDbContext.FavoriteMissions.Where(x => x.UserId == userId && x.MissionId == missionId).FirstOrDefault());
                    if (mid == null)
                    {
                        var fav = new FavoriteMission
                        {
                            MissionId = missionId,
                            UserId = userId
                        };
                        _cIDbContext.Add(fav);
                        _cIDbContext.SaveChanges();
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.AddToFavourite, StatusCode = responseStatusCode.Success, Result = true });
                    }
                    else
                    {
                        _cIDbContext.FavoriteMissions.Remove(mid);
                        _cIDbContext.SaveChanges();
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.RemoveToFavourite, StatusCode = responseStatusCode.Success, Result = true });
                    }
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.IdNotFound, StatusCode = responseStatusCode.InvalidData, Result = true });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Apply Mission
        public async Task<JsonResult> ApplyMission(int missionId, int userId)
        {
            try
            {
                var isUserApplied = await Task.FromResult(_cIDbContext.MissionApplications.Where(m => m.MissionId == missionId && m.UserId == userId).FirstOrDefault());
                if (isUserApplied == null)
                {
                    var applymission = new MissionApplication
                    {
                        MissionId = missionId,
                        UserId = userId,
                        ApprovalStatus = "pending",
                        CreatedAt = DateTime.Now,
                    };
                    _cIDbContext.Add(applymission);
                    _cIDbContext.SaveChanges();
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.ApplyMission, StatusCode = responseStatusCode.Success, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.IdNotFound, StatusCode = responseStatusCode.InvalidData, Result = true });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region comment
        public async Task<JsonResult> Comment(int missionId, int userId, String Commenttext)
        {
            try
            {
                var IsapplymissionOrNot = await Task.FromResult( _cIDbContext.MissionApplications.Where(cid => cid.MissionId == missionId && cid.UserId == userId && cid.ApprovalStatus == "approve").FirstOrDefault());
                {
                    if (IsapplymissionOrNot != null)
                    {
                        var comment = new Comment
                        {
                            MissionId = missionId,
                            UserId = userId,
                            CommentText = Commenttext

                        };
                        _cIDbContext.Add(comment);
                        _cIDbContext.SaveChanges();
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.CommentDone, StatusCode = responseStatusCode.Success, Result = true });

                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.CommentNotDone, StatusCode = responseStatusCode.Success, Result = true });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Recommanded A Co-Worker
        public async Task<JsonResult> RecommandedCoworker(string Email, int missionId, long userId)
        {
            try
            {
                var isUser = await Task.FromResult(_cIDbContext.Users.Where(p => p.Email == Email).FirstOrDefault());
                if (isUser != null)
                {
                    var invite = new MissionInvite
                    {
                        ToUserId = isUser.UserId,
                        MissionId = missionId,
                        FromUserId = userId,
                        CreatedAt = DateTime.Now,
                    };
                    _cIDbContext.Add(invite);
                    _cIDbContext.SaveChanges();
                    var sentemail = await Task.FromResult(CommonMethods.RecommandedCoWorkerEmail(Email, missionId));
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.SentEmailRecommanded, StatusCode = responseStatusCode.Success, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.IdNotFound, StatusCode = responseStatusCode.InvalidData, Result = false });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
