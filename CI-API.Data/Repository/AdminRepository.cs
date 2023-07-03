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

                if (search != "")
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
                if (search != "")
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

        #region common

        #region GetListOfCountryTheme

        public async Task<JsonResult> getListOfCountryTheme()
        {
            try
            {
                List<MissionTheme> AllMissionThemes = await Task.FromResult(cIDbContext.MissionThemes.ToList());
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
