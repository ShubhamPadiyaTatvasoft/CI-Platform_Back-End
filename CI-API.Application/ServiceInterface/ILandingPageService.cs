using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.ServiceInterface
{
    public interface ILandingPageService
    {
        #region Method of LandingPageService
        public JsonResult LandingPage();

        public Task<JsonResult> GetMissionCards(GetMissionParamViewModel missionData);

        public Task<JsonResult> FavMissionService(long MissionId, long UserId);

        public Task<JsonResult> RecommendedMissionService(long MissionId, long FromUserId, long ToUserId, string Toemail);

        public Task<JsonResult> GetRecommenedUserList(long MissionId, long LoginUserId);
        #endregion
    }
}
