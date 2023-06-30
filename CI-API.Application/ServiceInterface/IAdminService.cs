using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.ServiceInterface
{
    public interface IAdminService
    {
        #region Methods of AdminService

        public Task<JsonResult> GetAllUser(string? search);
        public Task<JsonResult> GetAllMission(string? search);
        public Task<JsonResult> GetAllCMSPage(string? search);
        public Task<JsonResult> getListOfCountryTheme();
        public Task<JsonResult> getListOfCityBasedOnCountry(long? countryId);
        public Task<JsonResult> getUser(long? userId);
        public Task<JsonResult> updateUserData(UserDetailViewModel userDetailViewModel);
        public Task<JsonResult> deleteUser(long? userId);
        public Task<JsonResult> getMissionDataFromId(long? missionId);
        public Task<JsonResult> addUpdateMission(MissionDataViewModel missionDataViewModel);
        public Task<JsonResult> deleteMission(long? missionId);

        #endregion
    }
}
