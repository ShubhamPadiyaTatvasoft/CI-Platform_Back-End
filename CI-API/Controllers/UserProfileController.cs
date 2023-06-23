using CI_API.Application.ServiceInterface;
using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        IUserProfileService userProfileService;
        public UserProfileController(IUserProfileService _userProfileService)
        {
            userProfileService = _userProfileService;
        }

        [HttpGet("GetCountryList")]
        public Task<JsonResult> GetCountriesList()
        {
            return userProfileService.GetCountriesList();
        }

        [HttpGet("GetCitiesList")]
        public Task<JsonResult> GetCitiesList()
        {
            return userProfileService.GetCitiesList();
        }

        [HttpGet("GetCitiesListByCountryId")]
        public Task<JsonResult> GetCitiesList(long countryId)
        {
            return userProfileService.GetCitiesList(countryId);
        }

        [HttpGet("GetAvailability")]
        public Task<JsonResult> GetAvailability()
        {
            return userProfileService.GetAvailability();
        }

        [HttpGet("GetSkillsList")]
        public Task<JsonResult> GetSkillsList()
        {
            return userProfileService.GetSkillsList();
        }

        [HttpGet("GetUserDetails")]
        public Task<JsonResult> GetUserDetails(long userId)
        {
            return userProfileService.GetUserDetails(userId);
        }

        [HttpPost("ChangePassword")]
        public Task<JsonResult> ChangePassword([FromBody]ChangePasswordViewModel changePassword)
        {
            return userProfileService.ChangePassword(changePassword);
        }

        [HttpPost("ContactUs")]
        public Task<JsonResult> ContactUs(ContactUsViewModel contact)
        {
            return userProfileService.ContactUs(contact);
        }

        [HttpPost("UpdateUserDetails")]
        public Task<JsonResult> UpdateUserDetails2([FromForm] UserDetailsViewModel userDetails)
        {
            return userProfileService.UpdateUserDetails(userDetails);
        }

    }
}
