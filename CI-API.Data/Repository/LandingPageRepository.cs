using CI_API.Common.CommonModels;
using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Repository
{
    public class LandingPageRepository : ILandingPageRepository
    {

        #region Dependency Injection of DbContext 

        private readonly CiPlatformDbContext _cIDbContext;

        public LandingPageRepository(CiPlatformDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
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
            catch
            {
                return new JsonResult(new apiResponse<LandingPageViewModel> { Message = ResponseMessages.InvalidLoginCredentials, StatusCode = responseStatusCode.BadRequest, Result = true });
                
            }
        }
        #endregion

    }
}
