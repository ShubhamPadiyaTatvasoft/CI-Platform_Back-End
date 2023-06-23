using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Interface
{
    public interface IAdminRepository
    {
        #region Methods of AdminRepository

        public Task<JsonResult> GetAllUser(string? search);
        public Task<JsonResult> GetAllMission(string? search);
        public Task<JsonResult> getListOfCountryTheme();
        public Task<JsonResult> getListOfCityBasedOnCountry(long? countryId);
        public Task<JsonResult> getUser(long? userId);
        public Task<JsonResult> updateUserData(UserDetailViewModel userDetailViewModel);
        public Task<JsonResult> deleteUser(long? userId);


        #endregion
    }
}
