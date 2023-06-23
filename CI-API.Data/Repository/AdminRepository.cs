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

        private readonly CiPlatformDbContext cIDbContext;

        public AdminRepository(CiPlatformDbContext _cIDbContext)
        {
            cIDbContext = _cIDbContext;
        }
        #endregion

        #region GetAllUser

        public async Task<JsonResult> GetAllUser(string? search)
        {
            try
            {

                if (search != null)
                {
                    List<User> userData = await Task.FromResult(cIDbContext.Users.Where(U => U.FirstName.Contains(search) || U.LastName.Contains(search)).ToList());

                    return new JsonResult(new apiResponse<List<User>> { StatusCode = responseStatusCode.Success, Data = userData, Result = true });
                }
                else
                {
                    List<User> userData = await Task.FromResult(cIDbContext.Users.ToList());
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
                if (search != null)
                {

                    List<Mission> AllMission = await Task.FromResult(cIDbContext.Missions.Where(M => M.Title.Contains(search) || M.Theme.Title.Contains(search)).ToList());
                    List<MissionTheme> AllMissionThemes = await Task.FromResult(cIDbContext.MissionThemes.ToList());
                    List<MissionSkill> AllMissionSkills = await Task.FromResult(cIDbContext.MissionSkills.ToList());

                    LandingPageViewModel landingPageViewModel = new()
                    {
                        missions = AllMission,
                        missionThemes = AllMissionThemes,
                        missionSkills = AllMissionSkills,

                    };

                    return new JsonResult(new apiResponse<LandingPageViewModel> { StatusCode = responseStatusCode.Success, Data = landingPageViewModel, Result = true });
                }
                else
                {
                    List<Mission> AllMission = await Task.FromResult(cIDbContext.Missions.ToList());
                    List<MissionTheme> AllMissionThemes = await Task.FromResult(cIDbContext.MissionThemes.ToList());
                    List<MissionSkill> AllMissionSkills = await Task.FromResult(cIDbContext.MissionSkills.ToList());

                    LandingPageViewModel landingPageViewModel = new()
                    {
                        missions = AllMission,
                        missionThemes = AllMissionThemes,
                        missionSkills = AllMissionSkills,

                    };

                    return new JsonResult(new apiResponse<LandingPageViewModel> { StatusCode = responseStatusCode.Success, Data = landingPageViewModel, Result = true });
                }
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
                List<MissionTheme> AllMissionThemes = await Task.FromResult(cIDbContext.MissionThemes.ToList());
                List<Country> AllCountries = await Task.FromResult(cIDbContext.Countries.ToList());

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

                List<City> AllCities = await Task.FromResult(cIDbContext.Cities.Where(c => c.CountryId == countryId).ToList());

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
                    User user = await Task.FromResult(cIDbContext.Users.Where(U => U.UserId == userId).FirstOrDefault());
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
            try
            {

                if (userDetailViewModel != null)
                {
                    if (userDetailViewModel.userId != 0)
                    {

                        User? userHasTobeUpdated = await Task.FromResult(cIDbContext.Users.Where(U => U.UserId == userDetailViewModel.userId).FirstOrDefault());
                        if (userHasTobeUpdated != null)
                        {
                            return updateAddUser(userHasTobeUpdated, userDetailViewModel);
                        }
                        else
                        {
                            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                        }
                    }

                    else
                    {
                        User userExist = await Task.FromResult(cIDbContext.Users.Where(U => U.Email == userDetailViewModel.email).FirstOrDefault());
                        if (userExist != null)
                        {
                            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserAlreadyExist, StatusCode = responseStatusCode.AlreadyExist, Result = false });
                        }
                        else
                        {
                            return updateAddUser(userExist, userDetailViewModel);
                        }
                    }
                }
                else
                {
                    return new JsonResult(new apiResponse<User> { Message = ResponseMessages.UserNotUpdated, StatusCode = responseStatusCode.RequestFailed, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }

        }
        #endregion

        #region DeleteUser
        public async Task<JsonResult> deleteUser(long? userId)
        {

            try
            {

                if (userId != null)
                {
                   
                    User? userHasTobeDeleted = await Task.FromResult(cIDbContext.Users.Where(U => U.UserId == userId).FirstOrDefault());

                    userHasTobeDeleted.DeletedAt = DateTime.Now;
                    userHasTobeDeleted.Status = StaticCode.InActive;

                    cIDbContext.SaveChanges();

                    return new JsonResult(new apiResponse<User> { Message = ResponseMessages.UserDeletedSuccess, StatusCode = responseStatusCode.Success, Result = true });

                }
                else
                {
                    return new JsonResult(new apiResponse<User> { Message = ResponseMessages.UserNotDeleted, StatusCode = responseStatusCode.RequestFailed, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #region userAddUpdate
        private JsonResult updateAddUser(User userHasTobeUpdated,UserDetailViewModel userDetailViewModel)
        {
            try
            {
                byte[] byteForPassword = Encoding.ASCII.GetBytes(userDetailViewModel.password);
                string encryptedPassword = Convert.ToBase64String(byteForPassword);
                if (userDetailViewModel.userId != 0)
                {
                    userHasTobeUpdated.FirstName = userDetailViewModel.firstName;
                    userHasTobeUpdated.LastName = userDetailViewModel.lastName;
                    userHasTobeUpdated.Password = encryptedPassword;
                    userHasTobeUpdated.Email = userDetailViewModel.email;
                    userHasTobeUpdated.CountryId = userDetailViewModel.countryId;
                    userHasTobeUpdated.CityId = userDetailViewModel.cityId;
                    userHasTobeUpdated.PhoneNumber = userDetailViewModel.phoneNumber;
                    userHasTobeUpdated.Role = userDetailViewModel.role;
                    userHasTobeUpdated.Status = userDetailViewModel.status;
                    if (userDetailViewModel.status == StaticCode.InActive)
                    {
                        userHasTobeUpdated.DeletedAt = null;
                    }
                    userHasTobeUpdated.Manager = userDetailViewModel.manager;
                    userHasTobeUpdated.UpdatedAt = DateTime.Now;

                    cIDbContext.SaveChanges();

                    return new JsonResult(new apiResponse<User> { Message = ResponseMessages.UserUpdatedSuccess, StatusCode = responseStatusCode.Success, Result = true });
                }
                else
                {

                    User user = new User()
                    {

                        FirstName = userDetailViewModel.firstName,
                        LastName = userDetailViewModel.lastName,
                        Email = userDetailViewModel.email,
                        Password = encryptedPassword,
                        PhoneNumber = userDetailViewModel.phoneNumber,
                        CityId = userDetailViewModel.cityId,
                        CountryId = userDetailViewModel.countryId,
                        Role = userDetailViewModel.role,
                        Status = userDetailViewModel.status,
                        Manager = userDetailViewModel.manager,

                    };
                    cIDbContext.Add(user);
                    cIDbContext.SaveChanges();
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.RegistrationSuccess, StatusCode = responseStatusCode.Success, Result = true });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }

        }
        #endregion
    }
}
