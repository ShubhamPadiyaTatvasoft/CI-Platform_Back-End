﻿using CI_API.Application.ServiceInterface;
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

        #region GetAllCMSPage
        public async Task<JsonResult> GetAllCMSPage(string? search)
        {
            return await AdminRepository.GetAllCMSPage(search);
        }
        #endregion

        #region GetAllMissionApplication
        public async Task<JsonResult> GetAllMissionApplication(string? search)
        {
            return await AdminRepository.GetAllMissionApplication(search);
        }

        #endregion  

        #region GetAllBanners
        public async Task<JsonResult> GetAllBanners(string? search)
        {
            return await AdminRepository.GetAllBanners(search);
        }

        #endregion

        #region GetAllThemes
        public async Task<JsonResult> GetAllThemes(string? search)
        {
            return await AdminRepository.GetAllThemes(search);
        }

        #endregion

        #region GetAllSkills
        public async Task<JsonResult> GetAllSkills(string? search)
        {
            return await AdminRepository.GetAllSkills(search);
        }

        #endregion 

        #region GetAllMissionApplication
        public async Task<JsonResult> GetAllStories(string? search)
        {
            return await AdminRepository.GetAllStories(search);
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
        #region AddUpdateMission
        public async Task<JsonResult> addUpdateMission(MissionDataViewModel missionDataViewModel)
        {
            return await AdminRepository.addUpdateMission(missionDataViewModel);
        }
        #endregion

        #region GetMissionDataFromId
        public async Task<JsonResult> getMissionDataFromId(long? missionId)
        {
            return await AdminRepository.getMissionDataFromId(missionId);
        }
        #endregion 
        
        #region DeleteMission
        public async Task<JsonResult> deleteMission(long? missionId)
        {
            return await AdminRepository.deleteMission(missionId);
        }
        #endregion

        #region AddEditCms
        public async Task<JsonResult> AddEditCms(CmsPage cms)
        {
            return await AdminRepository.AddEditCms(cms);
        }
        #endregion

        #region GetCmsDataFromId
        public async Task<JsonResult> GetCmsDataFromId(long? cmsId)
        {
            return await AdminRepository.GetCmsDataFromId(cmsId);
        }
        #endregion
        #region DeleteCms
        public async Task<JsonResult> DeleteCms(long? cmsId)
        {
            return await AdminRepository.DeleteCms(cmsId);
        }
        #endregion

        #region ApproveRejectMissionApplication
        public async Task<JsonResult> ApproveRejectMissionApplication(MissionApplicationViewModel application)
        {
            return await AdminRepository.ApproveRejectMissionApplication(application);

        }
        #endregion

        #region ApproveRejectDeleteStory
        public async Task<JsonResult> ApproveRejectDeleteStory(AdminPanelStoryViewModel storyData)
        {
            return await AdminRepository.ApproveRejectDeleteStory(storyData);

        }
        #endregion

        #region AddUpdateBanner
        public async Task<JsonResult> AddUpdateBanner(BannerDataViewModel bannerData)
        {
            return await AdminRepository.AddUpdateBanner(bannerData);
        }
        #endregion

        #region GetBannerDataFromId
        public async Task<JsonResult> GetBannerDataFromId(long? bannerId)
        {
            return await AdminRepository.GetBannerDataFromId(bannerId);
        }
        #endregion 
        #region DeleteUser
        public async Task<JsonResult> DeleteBanner(long? bannerId) {

            return await AdminRepository.DeleteBanner(bannerId);
        }
        #endregion
        #region GetThemeData
        public async Task<JsonResult> GetThemeData(long? themeId)
        {
            return await AdminRepository.GetThemeData(themeId);
        }
        #endregion
        #region AddUpdateTheme
        public async Task<JsonResult> AddUpdateTheme(AdminPanelThemeSkillViewModel themeData)
        {
            return await AdminRepository.AddUpdateTheme(themeData);
        }
        #endregion
        #region DeleteTheme
        public async Task<JsonResult> DeleteTheme(long? themeId)
        {

            return await AdminRepository.DeleteTheme(themeId);
        }
        #endregion
        #region GetSkillData
        public async Task<JsonResult> GetSkillData(long? skillId)
        {
            return await AdminRepository.GetSkillData(skillId);
        }
        #endregion
        #region AddUpdateSkill
        public async Task<JsonResult> AddUpdateSkill(AdminPanelThemeSkillViewModel skillData)
        {
            return await AdminRepository.AddUpdateSkill(skillData);
        }
        #endregion
        #region DeleteSkill
        public async Task<JsonResult> DeleteSkill(long? skillId)
        {

            return await AdminRepository.DeleteSkill(skillId);
        }
        #endregion

    }
}
