﻿using CI_API.Application.ServiceInterface;
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
        public Task<JsonResult> UpdateUserDetails2([FromBody] UserDetailsViewModel userDetails)
        {
            return userProfileService.UpdateUserDetails(userDetails);
        }

    }
}
