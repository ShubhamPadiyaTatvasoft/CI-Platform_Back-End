using CI_API.Application.ServiceInterface;
using CI_API.Data.Interface;
using CI_API.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.Services
{
    public  class VolunteerMissionService : IVolunteerMissionService
    {
        #region Dependency Injection of VolunteerMissionRepository Interface

        private readonly IVolunteerMissionRepository _VolunteerMissionRepository;
        public VolunteerMissionService(IVolunteerMissionRepository VolunteerMissionRepository)
        {
            _VolunteerMissionRepository = VolunteerMissionRepository;
        }
        #endregion

        #region VolunteerMission 
        public async Task<JsonResult> AllMissiondata(long missionId ,long userId)
        {
            return await _VolunteerMissionRepository.AllMissiondata(missionId, userId);       
        }
        public async Task<JsonResult> GetRecentVolunteer(int userId)
        {
            return await _VolunteerMissionRepository.GetRecentVolunteer(userId);    
        }
        public async Task<JsonResult> GetUserDetails(int userId)
        { 
            return await _VolunteerMissionRepository.GetUserDetails(userId);        
        }
        public async Task<JsonResult> GetComment(int missionId)
        {
            return await _VolunteerMissionRepository.GetComment(missionId);     
        }
        public async Task<JsonResult> AddToFavourite(int missionId, int userId)
        {
            return await _VolunteerMissionRepository.AddToFavourite(missionId, userId);     
        }
        public async Task<JsonResult> ApplyMission(int missionId, int userId)
        {
            return await _VolunteerMissionRepository.ApplyMission(missionId, userId);       
        }
        public async Task<JsonResult> Comment(int missionId, int userId, String Commenttext)
        {
            return await _VolunteerMissionRepository.Comment(missionId, userId, Commenttext);           
        }
        public async Task<JsonResult> RecommandedCoworker(string Email, int missionId, long userId)
        {
            return await _VolunteerMissionRepository.RecommandedCoworker(Email, missionId, userId);     
        }

        #endregion
    }
}
