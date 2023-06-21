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
    public class AdminRepository : IAdminRepository
    {

        #region Dependency Injection of DbContext 

        private readonly CiPlatformDbContext _cIDbContext;

        public AdminRepository(CiPlatformDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }
        #endregion

        #region GetAllUser

        public async Task<JsonResult> GetAllUser(string? search)
        {
            try
            {

                if (search != null)
                {
                    List<User> userData = _cIDbContext.Users.Where(U => U.FirstName.Contains(search) || U.LastName.Contains(search)).ToList();

                    return new JsonResult(new apiResponse<List<User>> { StatusCode = responseStatusCode.Success, Data = userData, Result = true });
                }
                else
                {
                    List<User> userData = _cIDbContext.Users.ToList();
                    return new JsonResult(new apiResponse<List<User>> { StatusCode = responseStatusCode.Success, Data = userData, Result = true });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<LandingPageViewModel> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }

        }
        #endregion

        #region GetAllMission
        public async Task<JsonResult> GetAllMission(string? search)
        {
            try
            {
                List<Mission> AllMission = await Task.FromResult(_cIDbContext.Missions.ToList());
                List<MissionTheme> AllMissionThemes = await Task.FromResult(_cIDbContext.MissionThemes.ToList());
                List<MissionSkill> AllMissionSkills = await Task.FromResult(_cIDbContext.MissionSkills.ToList());

                LandingPageViewModel landingPageViewModel = new()
                {
                    missions = AllMission,
                    missionThemes = AllMissionThemes,
                    missionSkills = AllMissionSkills,

                };

                return new JsonResult(new apiResponse<LandingPageViewModel> { StatusCode = responseStatusCode.Success, Data = landingPageViewModel, Result = true });
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #region GetListOfCountryTheme

        public async Task<JsonResult> getListOfCountryTheme()
        {
            try
            {
                List<MissionTheme> AllMissionThemes = await Task.FromResult(_cIDbContext.MissionThemes.ToList());
                List<Country> AllCountries = await Task.FromResult(_cIDbContext.Countries.ToList());

                CityCountryThemeSkillViewModel cityCountryThemeSkillViewModel = new()
                {
                    country = AllCountries,
                    themes = AllMissionThemes,
                };

                return new JsonResult(new apiResponse<CityCountryThemeSkillViewModel> { StatusCode = responseStatusCode.Success, Data = cityCountryThemeSkillViewModel, Result = true });
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }

        }
        #endregion

        #region GetListOfCityBasedOnCountry

        public async Task<JsonResult> getListOfCityBasedOnCountry(long? countryId)
        {
            try
            {

                List<City> AllCities = await Task.FromResult(_cIDbContext.Cities.Where(c => c.CountryId == countryId).ToList());

                CityCountryThemeSkillViewModel cityCountryThemeSkillViewModel = new()
                {
                    city = AllCities,
                };

                return new JsonResult(new apiResponse<CityCountryThemeSkillViewModel> { StatusCode = responseStatusCode.Success, Data = cityCountryThemeSkillViewModel, Result = true });
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }

        }
        #endregion

        #region GetUserDataFromID

        public async Task<JsonResult> getUser(long? userId)
        {
            try
            {
                if (userId != null)
                {
                    User user = await Task.FromResult(_cIDbContext.Users.Where(U => U.UserId == userId).FirstOrDefault());
                    if (user != null)
                    {
                        return new JsonResult(new apiResponse<User> { StatusCode = responseStatusCode.Success, Data = user, Result = true });
                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                    }

                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                }

            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }

        }
        #endregion

        #region UpdateUserData
        public async Task<JsonResult> updateUserData(UserDetailViewModel userDetailViewModel)
        {

            if (userDetailViewModel != null)
            {
                byte[] byteForPassword = Encoding.ASCII.GetBytes(userDetailViewModel.password);
                string encryptedPassword = Convert.ToBase64String(byteForPassword);

                User? userHasTobeUpdated =await Task.FromResult(_cIDbContext.Users.Where(U=>U.UserId==userDetailViewModel.userId).FirstOrDefault());

                userHasTobeUpdated.FirstName = userDetailViewModel.firstName;
                userHasTobeUpdated.LastName = userDetailViewModel.lastName;
                userHasTobeUpdated.Password = encryptedPassword;
                userHasTobeUpdated.Email = userDetailViewModel.email;
                userHasTobeUpdated.CountryId = userDetailViewModel.countryId;
                userHasTobeUpdated.CityId = userDetailViewModel.cityId;
                userHasTobeUpdated.PhoneNumber = userDetailViewModel.phoneNumber;
                userHasTobeUpdated.Role = userDetailViewModel.role;
                userHasTobeUpdated.Status = userDetailViewModel.status;
                userHasTobeUpdated.Manager = userDetailViewModel.manager;
                userHasTobeUpdated.UpdatedAt = DateTime.Now;

                _cIDbContext.SaveChanges();

                return new JsonResult(new apiResponse<User> { Message = ResponseMessages.UserUpdatedSuccess, StatusCode = responseStatusCode.Success, Result = true });

            }
            else
            {
            return new JsonResult(new apiResponse<User> {Message=ResponseMessages.UserNotUpdated, StatusCode = responseStatusCode.RequestFailed, Result = false });

            }

        }
        #endregion
    }
}
