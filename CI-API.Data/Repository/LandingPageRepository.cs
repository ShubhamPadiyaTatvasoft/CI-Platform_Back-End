using Azure.Core;
using CI_API.Common.CommonModels;
using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;

namespace CI_API.Data.Repository
{
    public class LandingPageRepository : ILandingPageRepository
    {

        #region Dependency Injection of DbContext 

        private readonly CiPlatformDbContext _cIDbContext;
        private readonly ISqlHelperRepository sqlHelper;

        public LandingPageRepository(CiPlatformDbContext cIDbContext, ISqlHelperRepository _sqlHelper)
        {
            _cIDbContext = cIDbContext;
            sqlHelper = _sqlHelper;
        }
        #endregion

        #region LandingPage
        public JsonResult LandingPage()
        {
            try
            {
                List<Mission> AllMission = _cIDbContext.Missions.ToList();
                List<MissionTheme> AllMissionThemes = _cIDbContext.MissionThemes.ToList();
                List<MissionSkill> AllMissionSkills = _cIDbContext.MissionSkills.ToList();

                LandingPageViewModel landingPageViewModel = new()
                {
                    missions = AllMission,
                    missionThemes = AllMissionThemes,
                    missionSkills = AllMissionSkills,

                };

                return new JsonResult(new apiResponse<LandingPageViewModel> { Message = ResponseMessages.LoginSuccess, StatusCode = responseStatusCode.Success, Data = landingPageViewModel, Result = true });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region CardsGetAndPostMethod
        public async Task<JsonResult> GetMissionCards(GetMissionParamViewModel filterData)
        {
            try
            {
                var query = _cIDbContext.Missions.AsQueryable();

                if (filterData.CityIds.Any())
                {
                    query = query.Where(mission => filterData.CityIds.Contains(mission.CityId));
                }
                if (!filterData.CityIds.Any() && filterData.CountryIds.Any())
                {
                    query = query.Where(mission => filterData.CountryIds.Contains(mission.MissionId));
                }
                if (filterData.ThemeIds.Any())
                {
                    query = query.Where(mission => filterData.ThemeIds.Contains(mission.ThemeId));
                }
                if (filterData.SkillIds.Any())
                {
                    query = query.Where(mission => mission.MissionSkills.Any(skill => filterData.SkillIds.Contains(skill.SkillId)));
                }

                if (!string.IsNullOrWhiteSpace(filterData.Search))
                {
                    query = query.Where(mission => mission.Title.ToLower().Contains(filterData.Search) || mission.ShortDescription.ToLower().Contains(filterData.Search));
                }

                var missionData = await Task.FromResult(query.Select(mission => new MissionCardViewModel()
                {
                    Mission = mission,
                    CityName = mission.City.Name,
                    ThemeName = mission.Theme.Title,
                    AvgRating = (float)mission.MissionRatings.Average(ratings => ratings.Rating),
                    IsfavMission = mission.FavoriteMissions.Any(favourite => favourite.UserId == filterData.LoginUserId),
                    IsAppliedMission = mission.MissionApplications.Any(applied => applied.UserId == filterData.LoginUserId),
                    IsApproveMission = mission.MissionApplications.Any(approve => approve.UserId == filterData.LoginUserId && approve.ApprovalStatus == "approve"),
                    Seatleft = (long)(mission.TotalSeats - mission.MissionApplications.Count(mission => mission.ApprovalStatus == "approve")),
                    AlreadyVolunteer = mission.MissionApplications.Count(mission => mission.ApprovalStatus == "approve"),
                    MissionImagePath = mission.MissionMedia.FirstOrDefault(image => image.MediaType == "PNG").MediaPath,
                    TargetGoalValue = int.Parse(_cIDbContext.GoalMissions.FirstOrDefault(missions => missions.MissionId == query.FirstOrDefault(mission => mission.MissionType == "GOAL").MissionId).GoalValue),
                    AchieveGoalValue = (int)mission.Timesheets.Where(goal => goal.Action != null).Sum(goal => goal.Action),
                }).ToList());

                return new JsonResult(new apiResponse<List<MissionCardViewModel>> 
                { 
                    Message = ResponseMessages.Success , 
                    Data = missionData, Result = true, 
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
        #endregion

        #region FavMissionMethod
        public async Task<JsonResult> FavMissionUpdated(long MissionId, long UserId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("MissionId", MissionId);
                parameters.Add("Userid", UserId);


                int affectedRows = await sqlHelper.ChangesOnData<DynamicParameters>("FavMissionInsOrUpdSp", parameters);

                if (affectedRows > 0)
                {
                    return new JsonResult(new apiResponse<string>
                    {
                        Message = ResponseMessages.FavMissionSuccess,
                        StatusCode = responseStatusCode.Success,
                        Result = true,

                    });
                }
                else
                {
                    return new JsonResult(new apiResponse<string>
                    {
                        Message = ResponseMessages.InvalidData,
                        StatusCode = responseStatusCode.InvalidData,
                        Result = false
                    });
                }
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
        #endregion

        #region RecommendedMissionMethod
        public async Task<JsonResult> RecommendedMission(long MissionId, long FromUserId, long ToUserId, string Toemail)
        {

            try
            {
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("MissionId", MissionId);
                parameters.Add("FromUserid", FromUserId);
                parameters.Add("ToUserid", ToUserId);

                int affectedRows = await sqlHelper.ChangesOnData<DynamicParameters>("RecommendedMissionSp", parameters);

                if (affectedRows > 0)
                {
                    SendmailtoFriends(Toemail, MissionId);
                    return new JsonResult(new apiResponse<string>
                    {
                        Message= ResponseMessages.RecommendedSuccess,
                        StatusCode = responseStatusCode.Success,
                        Result = true
                    });
                }
                else
                {
                    return new JsonResult(new apiResponse<string>
                    {
                        Message = ResponseMessages.InternalServerError,
                        StatusCode = responseStatusCode.InvalidData,
                        Result = false
                    });
                }

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
        #endregion


        #region SendEmails
        public void SendmailtoFriends(string email, long id)
        {
            var missionUrl = new UriBuilder();
            missionUrl.Scheme = "http";
            missionUrl.Host = "localhost";
            missionUrl.Port = 4200;
            missionUrl.Path = "Mission";
            missionUrl.Query = "ids=" + id;
            missionUrl.ToString();

            var emailfrom = new MailAddress("amangandhi0523@gmail.com");
            var frompwd = "ifuempfdxibysfbg";
            var toEmail = new MailAddress(email);

            string body = "Here is Mission Recommendation Link <br>" + missionUrl;

            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailfrom.Address, frompwd)
            };

            MailMessage message = new MailMessage(emailfrom, toEmail);
            message.Subject = "Password Reset Link";
            message.Body = body;
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
        #endregion
    }
}
