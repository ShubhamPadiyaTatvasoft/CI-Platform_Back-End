using CI_API.Application.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VolunteerMissionController : Controller
    {
        #region Dependancy Injection
        private readonly IVolunteerMissionService VolunteerMissionService;

        public VolunteerMissionController(IVolunteerMissionService _VolunteerMissionService)
        {
            VolunteerMissionService = _VolunteerMissionService;
        }
        #endregion

        [HttpGet("AllMissionData")]
        public async Task<JsonResult> GetMission(long missionId, long userId)
        {
            return await VolunteerMissionService.AllMissiondata(missionId,userId);
        }

        [HttpGet("GetRecentVolunteer")]
        public async Task<JsonResult> GetRecentVolunteer(int userId)
        {
            return await VolunteerMissionService.GetRecentVolunteer(userId);
        }

        [HttpGet("GetUserDetails")]
        public async Task<JsonResult> GetUserDetails(int userId)
        {
            return await VolunteerMissionService.GetUserDetails(userId);
        }

        [HttpGet("GetComment")]
        public async Task<JsonResult> GetComment(int missionId)
        {
            return await VolunteerMissionService.GetComment(missionId);
        }

        [HttpPost("AddToFavourite")]
        public async Task<JsonResult> AddToFavourite(int userId, int missionId)
        {
            return await VolunteerMissionService.AddToFavourite(userId, missionId);
        }

        [HttpPost("ApplyMission")]
        public async Task<JsonResult> ApplyMission(int userId, int missionId)
        {
            return await VolunteerMissionService.ApplyMission(userId, missionId);
        }

        [HttpPost("Comment")]
        public async Task<JsonResult> Comment(int userId, int missionId, String Commenttext)
        {
            return await VolunteerMissionService.Comment(userId, missionId, Commenttext);
        }

        [HttpPost("RecommandedCoworker")]
        public async Task<JsonResult> RecommandedCoworker(string Email, int missionId, long userId)
        {
            return await VolunteerMissionService.RecommandedCoworker(Email, missionId, userId);
        }








    }
}
