using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Interface
{
    public interface ILandingPageRepository
    {
        #region Methods of LandingPageRepository
        public JsonResult LandingPage();

        public Task<JsonResult> GetMissionCards(GetMissionParamViewModel getMissionParamViewModel);

        public Task<JsonResult> FavMissionUpdated(long MissionId, long UserId);

        public Task<JsonResult> RecommendedMission(long MissionId, long FromUserId, long ToUserId, string Toemail);
        #endregion
    }
}
