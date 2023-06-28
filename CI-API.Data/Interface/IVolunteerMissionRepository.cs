using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Interface
{
    public interface IVolunteerMissionRepository
    {
        public Task<JsonResult> AllMissiondata(long missionId, long userId);
        public Task<JsonResult> GetRecentVolunteer(int userId);
        public Task<JsonResult> GetUserDetails(int userId);
        public Task<JsonResult> GetComment(int missionId);
        public Task<JsonResult> AddToFavourite(int missionId, int userId);
        public Task<JsonResult> ApplyMission(int missionId, int userId);
        public Task<JsonResult> Comment(int missionId, int userId, String Commenttext);
        public Task<JsonResult> RecommandedCoworker(string Email, int missionId, long userId);
    }
}
