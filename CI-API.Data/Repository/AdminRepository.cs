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

        #region User

        #region GetAllUser

        public async Task<JsonResult> GetAllUser(string? search)
        {
            try
            {

                if (String.IsNullOrEmpty(search))
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

                    if (userHasTobeDeleted != null)
                    {
                        userHasTobeDeleted.DeletedAt = DateTime.Now;
                        userHasTobeDeleted.Status = StaticCode.UserInActive;

                        cIDbContext.SaveChanges();

                        return new JsonResult(new apiResponse<User> { Message = ResponseMessages.UserDeletedSuccess, StatusCode = responseStatusCode.Success, Result = true });
                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                    }


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
        private JsonResult updateAddUser(User userHasTobeUpdated, UserDetailViewModel userDetailViewModel)
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
                    if (userDetailViewModel.status == StaticCode.UserActive)
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

        #endregion

        #region mission

        #region GetAllMission
        public async Task<JsonResult> GetAllMission(string? search)
        {
            try
            {
                if (String.IsNullOrEmpty(search))
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

        #region GetMissionDataFromId
        public async Task<JsonResult> getMissionDataFromId(long? missionId)
        {
            try
            {


                Mission? missionData = await Task.FromResult(cIDbContext.Missions.Where(M => M.MissionId == missionId).FirstOrDefault());
                List<MissionDocument> documentsForMission = await Task.FromResult(cIDbContext.MissionDocuments.Where(MD => MD.MissionId == missionId).ToList());
                List<MissionMedium> mediaForMission = await Task.FromResult(cIDbContext.MissionMedia.Where(MM => MM.MissionId == missionId).ToList());
                List<MissionSkill> skillForMission = await Task.FromResult(cIDbContext.MissionSkills.Where(MS => MS.MissionId == missionId).ToList());
                if (missionData?.MissionType == StaticCode.GoalMission)
                {
                    GoalMission? goalValueForMission = await Task.FromResult(cIDbContext.GoalMissions.Where(GM => GM.MissionId == missionId).FirstOrDefault());
                    LandingPageViewModel missionDataForAdminPanel = new()
                    {
                        mission = missionData,
                        missionDocuments = documentsForMission,
                        missionMedia = mediaForMission,
                        goalMissions = goalValueForMission,
                        missionSkills = skillForMission
                    };
                    return new JsonResult(new apiResponse<LandingPageViewModel> { StatusCode = responseStatusCode.Success, Data = missionDataForAdminPanel, Result = true });
                }
                else
                {
                    LandingPageViewModel missionDataForAdminPanel = new()
                    {
                        mission = missionData,
                        missionDocuments = documentsForMission,
                        missionMedia = mediaForMission,
                        missionSkills = skillForMission
                    };
                    return new JsonResult(new apiResponse<LandingPageViewModel> { StatusCode = responseStatusCode.Success, Data = missionDataForAdminPanel, Result = true });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }

        }
        #endregion

        #region AddUpdateMission
        public async Task<JsonResult> addUpdateMission(MissionDataViewModel missionDataViewModel)
        {
            try
            {

                if (missionDataViewModel.missionId == 0)
                {
                    return await addMission(missionDataViewModel);
                }
                else
                {
                    return await updateMission(missionDataViewModel);
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }

        }
        #endregion

        #region AddNewMission
        private async Task<JsonResult> addMission(MissionDataViewModel missionDataViewModel)
        {
            try
            {
                Mission newMission = new()
                {
                    MissionType = missionDataViewModel.missionType,
                    CityId = missionDataViewModel.city,
                    CountryId = missionDataViewModel.country,
                    ThemeId = missionDataViewModel.missionTheme,
                    Title = missionDataViewModel.title,
                    ShortDescription = missionDataViewModel.shortDescription,
                    Description = missionDataViewModel.description,
                    StartDate = missionDataViewModel.startDate,
                    EndDate = missionDataViewModel.endDate,
                    Deadline = missionDataViewModel.deadlineDate,
                    Status = missionDataViewModel.status,
                    TotalSeats = missionDataViewModel.totalSeats,
                    OrganizationName = missionDataViewModel.organizationName,
                    OrganizationDetail = missionDataViewModel.organizationDetails,
                    Availability = missionDataViewModel.availability,

                };

                cIDbContext.Add(newMission);
                cIDbContext.SaveChanges();
                if (missionDataViewModel.missionSkills.Count != 0)
                {
                    foreach (var skill in missionDataViewModel.missionSkills)
                    {
                        MissionSkill missionSkill = new MissionSkill()
                        {
                            MissionId = newMission.MissionId,
                            SkillId = skill,
                        };
                        cIDbContext.MissionSkills.Add(missionSkill);
                        cIDbContext.SaveChanges();

                    }
                }

                if (missionDataViewModel.missionImageNameType.Count() != 0)
                {
                    for (var i = 0; i < missionDataViewModel.missionImages.Count(); i++)
                    {
                        for (var j = 0; j < missionDataViewModel.missionImageNameType.Count(); j++)
                        {
                            if (i == j)
                            {
                                MissionMedium missionMedium = new()
                                {
                                    MissionId = newMission.MissionId,
                                    MediaName = missionDataViewModel.missionImageNameType[j].Split('.')[0],
                                    MediaType = missionDataViewModel.missionImageNameType[j].Split('.')[1],
                                    MediaPath = missionDataViewModel.missionImages[j]
                                };
                                cIDbContext.MissionMedia.Add(missionMedium);
                                cIDbContext.SaveChanges();
                            }
                            else
                            {
                                continue;
                            }
                        }

                    };
                }
                if (missionDataViewModel.missionDocumentsNameType.Count() != 0)
                {
                    for (var i = 0; i < missionDataViewModel.missionDocuments.Count(); i++)
                    {
                        for (var j = 0; j < missionDataViewModel.missionDocumentsNameType.Count(); j++)
                        {
                            if (i == j)
                            {
                                MissionDocument missionDocuments = new()
                                {
                                    MissionId = newMission.MissionId,
                                    DocumentName = missionDataViewModel.missionDocumentsNameType[j].Split('.')[0],
                                    DocumentType = missionDataViewModel.missionDocumentsNameType[j].Split('.')[1],
                                    DocumentPath = missionDataViewModel.missionDocuments[j]
                                };
                                cIDbContext.MissionDocuments.Add(missionDocuments);
                                cIDbContext.SaveChanges();
                            }
                            else
                            {
                                continue;
                            }
                        }
                    };
                }
                if (missionDataViewModel.missionType == StaticCode.GoalMission)
                {
                    GoalMission goalMission = new()
                    {
                        MissionId = newMission.MissionId,
                        GoalObjectiveText = missionDataViewModel.goalText,
                        GoalValue = missionDataViewModel.goalValue.ToString(),

                    };
                    cIDbContext.GoalMissions.Add(goalMission);
                    cIDbContext.SaveChanges();
                }
                return new JsonResult(new apiResponse<User> { Message = ResponseMessages.MissionAddedSuccessfully, StatusCode = responseStatusCode.Success, Result = true });
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion

        #region UpdateMission
        private async Task<JsonResult> updateMission(MissionDataViewModel missionDataViewModel)
        {
            try
            {
                Mission? mission = await Task.FromResult(cIDbContext.Missions.Where(M => M.MissionId == missionDataViewModel.missionId).FirstOrDefault());
                if (mission != null)
                {
                    mission.Description = missionDataViewModel.description;
                    mission.Title = missionDataViewModel.title;
                    mission.Availability = missionDataViewModel.availability;
                    mission.Status = missionDataViewModel.status;
                    mission.CityId = missionDataViewModel.city;
                    mission.CountryId = missionDataViewModel.country;
                    mission.ThemeId = missionDataViewModel.missionTheme;
                    mission.ShortDescription = missionDataViewModel.shortDescription;
                    mission.StartDate = missionDataViewModel.startDate;
                    mission.EndDate = missionDataViewModel.endDate;
                    mission.Deadline = missionDataViewModel.deadlineDate;
                    mission.TotalSeats = missionDataViewModel.totalSeats;
                    mission.OrganizationName = missionDataViewModel.organizationName;
                    mission.OrganizationDetail = missionDataViewModel.organizationDetails;
                    mission.MissionType = missionDataViewModel.missionType;
                    mission.UpdatedAt = DateTime.Now;
                    if (missionDataViewModel.status == StaticCode.MissionActive)
                    {
                        mission.DeletedAt = null;
                    }

                    cIDbContext.SaveChanges();

                    GoalMission? goalMission = await Task.FromResult(cIDbContext.GoalMissions.Where(G => G.MissionId == missionDataViewModel.missionId).FirstOrDefault());
                    if (goalMission != null && missionDataViewModel.missionType == StaticCode.TimeMission)
                    {
                        cIDbContext.GoalMissions.Remove(goalMission);
                    }

                    if (missionDataViewModel.missionType == StaticCode.GoalMission)
                    {
                        if (goalMission != null)
                        {
                            goalMission.GoalObjectiveText = missionDataViewModel.goalText;
                            goalMission.GoalValue = missionDataViewModel.goalValue.ToString();
                            goalMission.UpdatedAt = DateTime.Now;
                            goalMission.DeletedAt = null;
                        }
                        else
                        {
                            GoalMission newGoalMission = new()
                            {
                                MissionId = mission.MissionId,
                                GoalObjectiveText = missionDataViewModel.goalText,
                                GoalValue = missionDataViewModel.goalValue.ToString(),

                            };
                            cIDbContext.GoalMissions.Add(newGoalMission);
                        }
                        cIDbContext.SaveChanges();
                    }

                    List<MissionSkill>? missionSkill = await Task.FromResult(cIDbContext.MissionSkills.Where(MS => MS.MissionId == missionDataViewModel.missionId).ToList());
                    if (missionSkill != null)
                    {
                        foreach (var skill in missionSkill)
                        {
                            cIDbContext.MissionSkills.Remove(skill);
                        }
                    }


                    List<MissionMedium>? missionOldMedia = await Task.FromResult(cIDbContext.MissionMedia.Where(MM => MM.MissionId == missionDataViewModel.missionId).ToList());
                    if (missionOldMedia != null)
                    {
                        foreach (var media in missionOldMedia)
                        {
                            cIDbContext.MissionMedia.Remove(media);
                        }
                    }

                    List<MissionDocument>? missionOldDocuments = await Task.FromResult(cIDbContext.MissionDocuments.Where(MD => MD.MissionId == missionDataViewModel.missionId).ToList());
                    if (missionOldDocuments != null)
                    {
                        foreach (var document in missionOldDocuments)
                        {
                            cIDbContext.MissionDocuments.Remove(document);
                        }

                    }

                    if (missionDataViewModel.missionSkills.Count() != 0)
                    {
                        foreach (var skill in missionDataViewModel.missionSkills)
                        {
                            MissionSkill newMissionSkill = new MissionSkill()
                            {
                                MissionId = mission.MissionId,
                                SkillId = skill,
                            };
                            cIDbContext.MissionSkills.Add(newMissionSkill);
                            cIDbContext.SaveChanges();
                        }
                    }

                    if (missionDataViewModel.missionImageNameType.Count() != 0)
                    {
                        for (var i = 0; i < missionDataViewModel.missionImages.Count(); i++)
                        {
                            for (var j = 0; j < missionDataViewModel.missionImageNameType.Count(); j++)
                            {
                                if (i == j)
                                {
                                    MissionMedium missionMedium = new()
                                    {
                                        MissionId = mission.MissionId,
                                        MediaName = missionDataViewModel.missionImageNameType[j].Split('.')[0],
                                        MediaType = missionDataViewModel.missionImageNameType[j].Split('.')[1],
                                        MediaPath = missionDataViewModel.missionImages[j]
                                    };
                                    cIDbContext.MissionMedia.Add(missionMedium);
                                    cIDbContext.SaveChanges();
                                }
                                else
                                {
                                    continue;
                                }
                            }

                        };
                    }
                    if (missionDataViewModel.missionDocumentsNameType.Count() != 0)
                    {
                        for (var i = 0; i < missionDataViewModel.missionDocuments.Count(); i++)
                        {
                            for (var j = 0; j < missionDataViewModel.missionDocumentsNameType.Count(); j++)
                            {
                                if (i == j)
                                {
                                    MissionDocument missionDocuments = new()
                                    {
                                        MissionId = mission.MissionId,
                                        DocumentName = missionDataViewModel.missionDocumentsNameType[j].Split('.')[0],
                                        DocumentType = missionDataViewModel.missionDocumentsNameType[j].Split('.')[1],
                                        DocumentPath = missionDataViewModel.missionDocuments[j]
                                    };
                                    cIDbContext.MissionDocuments.Add(missionDocuments);
                                    cIDbContext.SaveChanges();
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        };
                    }
                    return new JsonResult(new apiResponse<User> { Message = ResponseMessages.MissionUpdatedSuccessfully, StatusCode = responseStatusCode.Success, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.MissionNotUpdatedSuccessfully, StatusCode = responseStatusCode.RequestFailed, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion

        #region DeleteMission
        public async Task<JsonResult> deleteMission(long? missionId)
        {
            try
            {
                Mission? mission = await Task.FromResult(cIDbContext.Missions.Where(M => M.MissionId == missionId).FirstOrDefault());
                if (mission != null)
                {
                    mission.DeletedAt = DateTime.Now;
                    mission.Status = StaticCode.MissionInactive;

                    GoalMission? goalMission = await Task.FromResult(cIDbContext.GoalMissions.Where(G => G.MissionId == missionId).FirstOrDefault());
                    if (goalMission != null)
                    {
                        goalMission.DeletedAt = DateTime.Now;

                    }

                    List<MissionMedium>? missionOldMedia = await Task.FromResult(cIDbContext.MissionMedia.Where(MM => MM.MissionId == missionId).ToList());
                    if (missionOldMedia != null)
                    {
                        foreach (var media in missionOldMedia)
                        {
                            media.DeletedAt = DateTime.Now;

                        }
                    }

                    List<MissionDocument>? missionOldDocuments = await Task.FromResult(cIDbContext.MissionDocuments.Where(MD => MD.MissionId == missionId).ToList());
                    if (missionOldDocuments != null)
                    {
                        foreach (var document in missionOldDocuments)
                        {
                            document.DeletedAt = DateTime.Now;

                        }
                    }

                    List<MissionSkill>? missionSkill = await Task.FromResult(cIDbContext.MissionSkills.Where(MS => MS.MissionId == missionId).ToList());
                    if (missionSkill != null)
                    {
                        foreach (var skill in missionSkill)
                        {
                            skill.DeletedAt = DateTime.Now;

                        }
                    }

                    cIDbContext.SaveChanges();

                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.MissionDeletedSuccessfully, StatusCode = responseStatusCode.Success, Result = true });

                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }



            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #endregion

        #region CMSPages

        #region GetAllCMSPage
        public async Task<JsonResult> GetAllCMSPage(string? search)
        {
            try
            {
                if (String.IsNullOrEmpty(search))
                {

                    List<CmsPage>? cmsPages = await Task.FromResult(cIDbContext.CmsPages.Where(CM => CM.Title.Contains(search) || CM.Slug.Contains(search) || CM.Status.Contains(search)).ToList());

                    return new JsonResult(new apiResponse<List<CmsPage>> { StatusCode = responseStatusCode.Success, Data = cmsPages, Result = true });
                }
                else
                {
                    List<CmsPage>? cmsPages = await Task.FromResult(cIDbContext.CmsPages.ToList());

                    return new JsonResult(new apiResponse<List<CmsPage>> { StatusCode = responseStatusCode.Success, Data = cmsPages, Result = true });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #region GetCmsDataFromId
        public async Task<JsonResult> GetCmsDataFromId(long? cmsId)
        {
            try
            {
                if (cmsId != null)
                {
                    CmsPage? cms = cIDbContext.CmsPages.Where(c => c.CmsPageId == cmsId).FirstOrDefault();
                    return new JsonResult(new apiResponse<CmsPage> { StatusCode = responseStatusCode.Success, Data = cms, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

                }


            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #region AddEditCms
        public async Task<JsonResult> AddEditCms(CmsPage cms)
        {
            try
            {

                if (cms.CmsPageId != 0)
                {
                    CmsPage? cmsDatatoBeUpdated = cIDbContext.CmsPages.Where(C => C.CmsPageId == cms.CmsPageId).FirstOrDefault();

                    cmsDatatoBeUpdated.Title = cms.Title;
                    cmsDatatoBeUpdated.Description = cms.Description;
                    cmsDatatoBeUpdated.Slug = cms.Slug;
                    cmsDatatoBeUpdated.Status = cms.Status;
                    cmsDatatoBeUpdated.DeletedAt = null;
                    cmsDatatoBeUpdated.UpdatedAt = DateTime.Now;


                }
                else
                {
                    CmsPage newCms = new()
                    {
                        Title = cms.Title,
                        Description = cms.Description,
                        Slug = cms.Slug,
                        Status = cms.Status,
                    };
                    cIDbContext.Add(newCms);


                }
                cIDbContext.SaveChanges();

                if (cms.CmsPageId != 0)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.CmsUpdateSuccess, StatusCode = responseStatusCode.Success, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.CmsAddedSuccess, StatusCode = responseStatusCode.Success, Result = true });
                }


            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion


        #region DeleteCms
        public async Task<JsonResult> DeleteCms(long? cmsId)
        {
            try
            {
                if (cmsId != 0)
                {
                    CmsPage? cmsToBeDeleted = cIDbContext.CmsPages.Where(C => C.CmsPageId == cmsId).FirstOrDefault();
                    if (cmsToBeDeleted != null)
                    {
                        cmsToBeDeleted.DeletedAt = DateTime.Now;
                        cmsToBeDeleted.Status = StaticCode.CmsInActive;

                        cIDbContext.SaveChanges();
                    }

                }
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.CmsDeletedSuccess, StatusCode = responseStatusCode.Success, Result = true });

            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion
        #endregion

        #region missionApplication

        #region GetAllMissionApplication
        public async Task<JsonResult> GetAllMissionApplication(string? search)
        {
            try
            {

                if (String.IsNullOrEmpty(search))
                {
                    var missionApplications = await Task.FromResult(from missionApplication in cIDbContext.MissionApplications.Where(MA => MA.ApprovalStatus == StaticCode.missionApplicationPending)
                                                                    join mission in cIDbContext.Missions on missionApplication.MissionId equals mission.MissionId
                                                                    join user in cIDbContext.Users on missionApplication.UserId equals user.UserId
                                                                    select new VolunteerMissionViewModel
                                                                    {
                                                                        missionId = mission.MissionId,
                                                                        missionTitle = mission.Title,
                                                                        userName = user.FirstName + " " + user.LastName,
                                                                        missionApplicationId = missionApplication.MissionApplicationId,
                                                                        userId = user.UserId,   
                                                                        appliedDate = missionApplication.AppliedAt,

                                                                    });


                    return new JsonResult(new apiResponse<List<VolunteerMissionViewModel>> { StatusCode = responseStatusCode.Success, Data = missionApplications.ToList(), Result = true });

                }
                else
                {
                    var missionApplications = await Task.FromResult(from missionApplication in cIDbContext.MissionApplications.Where(MA => MA.ApprovalStatus == StaticCode.missionApplicationPending && MA.Mission.Title.Contains(search) || MA.User.FirstName.Contains(search) || MA.User.LastName.Contains(search))
                                                                    join mission in cIDbContext.Missions on missionApplication.MissionId equals mission.MissionId
                                                                    join user in cIDbContext.Users on missionApplication.UserId equals user.UserId


                                                                    select new VolunteerMissionViewModel
                                                                    {
                                                                        missionId = mission.MissionId,
                                                                        missionTitle = mission.Title,
                                                                        userName = user.FirstName + " " + user.LastName,
                                                                        missionApplicationId = missionApplication.MissionApplicationId,
                                                                        userId = user.UserId,
                                                                        appliedDate = missionApplication.AppliedAt,

                                                                    });


                    return new JsonResult(new apiResponse<List<VolunteerMissionViewModel>> { StatusCode = responseStatusCode.Success, Data = missionApplications.ToList(), Result = true });

                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #region ApproveRejectMissionApplication
        public async Task<JsonResult> ApproveRejectMissionApplication(MissionApplicationViewModel application)
        {

            try
            {
                if (application.missionApplicationId != null)
                {
                    MissionApplication? applicationToBeReviewed = cIDbContext.MissionApplications.Where(MA => MA.MissionApplicationId == application.missionApplicationId).FirstOrDefault();
                    if (applicationToBeReviewed != null)
                    {
                        applicationToBeReviewed.ApprovalStatus = application.approvalStatus;
                        applicationToBeReviewed.UpdatedAt = DateTime.Now;
                        cIDbContext.SaveChanges();
                        if (application.approvalStatus == StaticCode.missionApplicationApprove)
                        {
                            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.MissionApplicationApproved, StatusCode = responseStatusCode.Success, Result = false });
                        }
                        else
                        {
                            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.MissionApplicationRejected, StatusCode = responseStatusCode.Success, Result = false });
                        }
                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                    }

                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }


            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }

        }
        #endregion

        #endregion

        #region stories

        #region GetAllStories
        public async Task<JsonResult> GetAllStories(string? search)
        {
            try
            {

                if (search == null)
                {
                    var missionStories = await Task.FromResult(from story in cIDbContext.Stories.Where(s => s.Status == StaticCode.storyStatusPending)
                                                               join mission in cIDbContext.Missions on story.MissionId equals mission.MissionId
                                                               join user in cIDbContext.Users on story.UserId equals user.UserId


                                                               select new VolunteerMissionViewModel
                                                               {
                                                                   missionTitle = mission.Title,
                                                                   userName = user.FirstName + " " + user.LastName,
                                                                   storyTitle = story.Title,
                                                                   storyId = story.StoryId,

                                                               });


                    return new JsonResult(new apiResponse<List<VolunteerMissionViewModel>> { StatusCode = responseStatusCode.Success, Data = missionStories.ToList(), Result = true });

                }
                else
                {
                    var missionStories = await Task.FromResult(from story in cIDbContext.Stories.Where(s => s.Status == StaticCode.storyStatusPending && s.Title.Contains(search) || s.Mission.Title.Contains(search) || s.User.FirstName.Contains(search) || s.User.LastName.Contains(search))
                                                               join mission in cIDbContext.Missions on story.MissionId equals mission.MissionId
                                                               join user in cIDbContext.Users on story.UserId equals user.UserId


                                                               select new VolunteerMissionViewModel
                                                               {
                                                                   missionTitle = mission.Title,
                                                                   userName = user.FirstName + " " + user.LastName,
                                                                   storyTitle = story.Title,
                                                                   storyId = story.StoryId,

                                                               });


                    return new JsonResult(new apiResponse<List<VolunteerMissionViewModel>> { StatusCode = responseStatusCode.Success, Data = missionStories.ToList(), Result = true });

                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #region ApproveRejectDeleteStory
        public async Task<JsonResult> ApproveRejectDeleteStory(AdminPanelStoryViewModel storyData)
        {
            try
            {
                if (storyData != null)
                {
                    if (storyData.storyStatus == StaticCode.storyStatusDelete)
                    {
                        return await DeleteStory(storyData);
                    }
                    else
                    {
                        Story? storyToBeApprovedOrRejected = await Task.FromResult(cIDbContext.Stories.Where(S => S.StoryId == storyData.storyId).FirstOrDefault());
                        if (storyToBeApprovedOrRejected != null)
                        {
                            storyToBeApprovedOrRejected.UpdatedAt = DateTime.Now;
                            storyToBeApprovedOrRejected.Status = storyData.storyStatus;
                            if (storyData.storyStatus == StaticCode.storyStatusApprove)
                            {
                                storyToBeApprovedOrRejected.PublishedAt = DateTime.Now;
                            }
                            cIDbContext.SaveChanges();
                            if (storyData.storyStatus == StaticCode.storyStatusApprove)
                            {
                                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.StoryApproved, StatusCode = responseStatusCode.Success, Result = false });
                            }
                            else
                            {
                                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.StoryRejected, StatusCode = responseStatusCode.Success, Result = false });
                            }
                        }
                        else
                        {
                            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

                        }

                    }

                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }

        }
        #endregion

        #region Delete Story
        public async Task<JsonResult> DeleteStory(AdminPanelStoryViewModel storyData)
        {
            try
            {
                Story? storyToBeDeleted = await Task.FromResult(cIDbContext.Stories.Where(S => S.StoryId == storyData.storyId).FirstOrDefault());
                if (storyToBeDeleted != null)
                {
                    storyToBeDeleted.Status = StaticCode.storyStatusDelete;
                    storyToBeDeleted.DeletedAt = DateTime.Now;

                    List<StoryMedium> storyMedia = cIDbContext.StoryMedia.Where(SM => SM.StoryId == storyToBeDeleted.StoryId).ToList();
                    if (storyMedia != null)
                    {
                        foreach (var item in storyMedia)
                        {
                            cIDbContext.Remove(item);


                        }
                    }
                    cIDbContext.SaveChanges();
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.StoryDeleted, StatusCode = responseStatusCode.Success, Result = false });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #endregion

        #region banner

        #region GetAllBanners
        public async Task<JsonResult> GetAllBanners(string search)
        {
            try
            {

                if (String.IsNullOrEmpty(search))
                {
                    List<Banner> banners = await Task.FromResult(cIDbContext.Banners.ToList());
                    return new JsonResult(new apiResponse<List<Banner>> { StatusCode = responseStatusCode.Success, Data = banners, Result = true });

                }
                else
                {
                    List<Banner> bannersSearch = await Task.FromResult(cIDbContext.Banners.Where(B => B.Text.Contains(search)).ToList());
                    return new JsonResult(new apiResponse<List<Banner>> { StatusCode = responseStatusCode.Success, Data = bannersSearch, Result = true });

                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion

        #region AddUpdateBanner
        public async Task<JsonResult> AddUpdateBanner(BannerDataViewModel bannerData)
        {
            try
            {
                if (bannerData != null)
                {
                    if (bannerData.bannerId != 0)
                    {
                        Banner? bannerTobeUpdated = cIDbContext.Banners.Where(B => B.SortOrder == bannerData.sortOrder && B.BannerId == bannerData.bannerId).FirstOrDefault();
                        if (bannerTobeUpdated != null)
                        {
                            bannerTobeUpdated.Text = bannerData.bannerText;
                            bannerTobeUpdated.SortOrder = (int)bannerData.sortOrder;
                            bannerTobeUpdated.UpdatedAt = DateTime.Now;
                            bannerTobeUpdated.Image = bannerData.images[0];

                            cIDbContext.SaveChanges();
                            return new JsonResult(new apiResponse<List<Banner>> { Message = ResponseMessages.BannerUpdateSuccess, StatusCode = responseStatusCode.Success, Result = true });

                        }

                        else if (cIDbContext.Banners.Where(B => B.SortOrder == bannerData.sortOrder).FirstOrDefault() != null)
                        {
                            return new JsonResult(new apiResponse<List<Banner>> { Message = ResponseMessages.BannerSortOrderAlreadyExist, StatusCode = responseStatusCode.AlreadyExist, Result = true });
                        }
                        else
                        {
                            Banner? bannerTobeUpdatedWithNewSortOrder = cIDbContext.Banners.Where(B => B.BannerId == bannerData.bannerId).FirstOrDefault();
                            if (bannerTobeUpdatedWithNewSortOrder != null)
                            {
                                bannerTobeUpdatedWithNewSortOrder.Text = bannerData.bannerText;
                                bannerTobeUpdatedWithNewSortOrder.SortOrder = (int)bannerData.sortOrder;
                                bannerTobeUpdatedWithNewSortOrder.UpdatedAt = DateTime.Now;
                                bannerTobeUpdatedWithNewSortOrder.Image = bannerData.images[0];

                                cIDbContext.SaveChanges();
                                return new JsonResult(new apiResponse<List<Banner>> { Message = ResponseMessages.BannerUpdateSuccess, StatusCode = responseStatusCode.Success, Result = true });
                            }
                            else
                            {
                                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                            }

                        }

                    }
                    else
                    {
                        Banner? previousBanner = cIDbContext.Banners.Where(B => B.SortOrder == bannerData.sortOrder).FirstOrDefault();
                        if (previousBanner == null)
                        {
                            Banner newBanner = new()
                            {
                                Text = bannerData.bannerText,
                                Image = bannerData.images[0],
                                SortOrder = (int?)bannerData.sortOrder,
                            };
                            cIDbContext.Banners.Add(newBanner);
                            cIDbContext.SaveChanges();
                            return new JsonResult(new apiResponse<List<Banner>> { Message = ResponseMessages.BannerAddedSuccess, StatusCode = responseStatusCode.Success, Result = true });

                        }
                        else
                        {
                            return new JsonResult(new apiResponse<List<Banner>> { Message = ResponseMessages.BannerSortOrderAlreadyExist, StatusCode = responseStatusCode.AlreadyExist, Result = true });

                        }

                    }
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }

        }
        #endregion

        #region GetBannerDataFromId
        public async Task<JsonResult> GetBannerDataFromId(long? bannerId)
        {
            try
            {
                if (bannerId != 0)
                {
                    Banner? GetBannerDataFromId = cIDbContext.Banners.Where(B => B.BannerId == bannerId).FirstOrDefault();
                    return new JsonResult(new apiResponse<Banner> { StatusCode = responseStatusCode.Success, Data = GetBannerDataFromId, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }

        }
        #endregion

        #region DeleteUser
        public async Task<JsonResult> DeleteBanner(long? bannerId)
        {
            try
            {
                if (bannerId != 0)
                {
                    Banner? bannerToBeDeleted = cIDbContext.Banners.Where(b => b.BannerId == bannerId).FirstOrDefault();
                    if (bannerToBeDeleted != null)
                    {
                        cIDbContext.Banners.Remove(bannerToBeDeleted);
                        cIDbContext.SaveChanges();
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.BannerDeletedSuccess, StatusCode = responseStatusCode.Success, Result = true });

                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                    }
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }

        }
        #endregion

        #endregion

        #region theme

        #region GetAllThemes
        public async Task<JsonResult> GetAllThemes(string? search)
        {
            try
            {
                if (String.IsNullOrEmpty(search))
                {
                    List<MissionTheme> allThemes = cIDbContext.MissionThemes.ToList();
                    return new JsonResult(new apiResponse<List<MissionTheme>> { StatusCode = responseStatusCode.Success, Data = allThemes, Result = true });
                }
                else{

                    List<MissionTheme> allThemes = cIDbContext.MissionThemes.Where(MT=>MT.Title.Contains(search)).ToList();
                    return new JsonResult(new apiResponse<List<MissionTheme>> { StatusCode = responseStatusCode.Success, Data = allThemes, Result = true });
                }
               
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }

        #endregion

        #region GetThemeData
        public async Task<JsonResult> GetThemeData(long? themeId)
        {
            try
            {
                if (themeId != 0)
                {
                    MissionTheme? themeDataForEdit = cIDbContext.MissionThemes.Where(MT => MT.MissionThemeId == themeId).FirstOrDefault();
                    if(themeDataForEdit!=null)
                    {
                        return new JsonResult(new apiResponse<MissionTheme> {  StatusCode = responseStatusCode.Success,Data=themeDataForEdit, Result = false });

                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                    }
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }
        #endregion

        #region AddUpdateBanner
        public async Task<JsonResult> AddUpdateTheme(AdminPanelThemeSkillViewModel themeData)
        {
            try
            {
                if (themeData.themeId != 0)
                {
                    MissionTheme? themeToBeUpdated = cIDbContext.MissionThemes.Where(MT => MT.MissionThemeId == themeData.themeId).FirstOrDefault();
                    if (themeToBeUpdated != null)
                    {
                        themeToBeUpdated.Title = themeData.themeTitle;
                        if (themeData.themeStatus == "InActive")
                        {
                            List<Mission>? themeBasedMission = cIDbContext.Missions.Where(M => M.ThemeId == themeData.themeId).ToList();
                            if (themeBasedMission.Count() > 0)
                            {
                                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.DeleteThemeBasedMissionFirst, StatusCode = responseStatusCode.AlreadyExist, Result = true });
                            }
                        }

                       
                        themeToBeUpdated.Status = themeData.themeStatus;
                        themeToBeUpdated.UpdatedAt = DateTime.Now;
                        themeToBeUpdated.DeletedAt = null;
                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                    }
                }
                else
                {
                    MissionTheme newMissionTheme = new()
                    {
                        Title = themeData.themeTitle,
                        Status = themeData.themeStatus,
                    };
                    cIDbContext.MissionThemes.Add(newMissionTheme);
                }
                cIDbContext.SaveChanges();
                if (themeData.themeId == 0)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.ThemeAddedSuccess, StatusCode = responseStatusCode.Success, Result = true });
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.ThemeUpdateSuccess, StatusCode = responseStatusCode.Success, Result = true });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

            }
        }
        #endregion

        #region DeleteTheme
        public async Task<JsonResult> DeleteTheme(long? themeId)
        {

            try
            {
                if (themeId != 0)
                {
                    MissionTheme? missionThemeToBeDeleted = cIDbContext.MissionThemes.Where(MT => MT.MissionThemeId == themeId).FirstOrDefault();
                    if (missionThemeToBeDeleted != null)
                    {
                        List<Mission>? themeBasedMission = cIDbContext.Missions.Where(M => M.ThemeId == themeId).ToList();
                        if (themeBasedMission.Count() > 0)
                        {
                            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.DeleteThemeBasedMissionFirst, StatusCode = responseStatusCode.AlreadyExist, Result = true });
                        }
                        else
                        {
                            missionThemeToBeDeleted.DeletedAt = DateTime.Now;
                            missionThemeToBeDeleted.Status = "InActive";
                            cIDbContext.SaveChanges();
                        }
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.ThemeDeletedSuccess, StatusCode = responseStatusCode.Success, Result = true });

                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.NotFound, Result = false });
                    }
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }

        }
        #endregion
        #endregion

        #region common

        #region GetListOfCountryTheme

        public async Task<JsonResult> getListOfCountryTheme()
        {
            try
            {
                List<MissionTheme> AllMissionThemes = await Task.FromResult(cIDbContext.MissionThemes.Where(M=>M.Status=="Active").ToList());
                List<Country> AllCountries = await Task.FromResult(cIDbContext.Countries.ToList());
                List<Skill> AllSkiils = await Task.FromResult(cIDbContext.Skills.ToList());

                CityCountryThemeSkillViewModel cityCountryThemeSkillViewModel = new()
                {
                    country = AllCountries,
                    themes = AllMissionThemes,
                    skills = AllSkiils,
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

        #endregion

        
        
    }
}
