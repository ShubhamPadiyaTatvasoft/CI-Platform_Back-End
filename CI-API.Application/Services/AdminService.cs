using CI_API.Application.ServiceInterface;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.Services
{
    public class AdminService: IAdminService
    {
        #region Dependency Injection of AdminRepository Interface

        private readonly IAdminRepository AdminRepository;
        public AdminService(IAdminRepository _AdminRepository)
        {
            AdminRepository = _AdminRepository;
        }
        #endregion

        #region GetAllUser
        public async Task<JsonResult> GetAllUser(string? search)
        {
            return await AdminRepository.GetAllUser(search);
        }
        #endregion

        #region GetAllMission
        public async Task<JsonResult> GetAllMission(string? search)
        {
            return await AdminRepository.GetAllMission(search);
        }
        #endregion

        #region GetListOfCityCountryThemeSkills
        public async Task<JsonResult> getListOfCountryTheme()
        {
            return await AdminRepository.getListOfCountryTheme();
        }
        #endregion

        #region GetListOfCityBasedOnCountry
        public async Task<JsonResult> getListOfCityBasedOnCountry(long? countryId)
        {
            return await AdminRepository.getListOfCityBasedOnCountry(countryId);
        }
        #endregion

        #region GetUserDataFromID
        public async Task<JsonResult> getUser(long? userId)
        {
            return await AdminRepository.getUser(userId);
        }
        #endregion

        #region UpdateUserData
        public async Task<JsonResult> updateUserData(UserDetailViewModel userDetailViewModel)
        {
            return await AdminRepository.updateUserData(userDetailViewModel);
        }
        #endregion

        #region DeleteUser
        public async Task<JsonResult> deleteUser(long? userId) {

            return await AdminRepository.deleteUser(userId);
        }
        #endregion

    }
}
