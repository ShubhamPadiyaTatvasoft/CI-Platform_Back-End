using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.ServiceInterface
{
    public interface IUserProfileService
    {
        public Task<JsonResult> GetCountriesList();
        public Task<JsonResult> GetCitiesList();
        public Task<JsonResult> GetCitiesList(long countryId);
        public Task<JsonResult> GetAvailability();
        public Task<JsonResult> GetSkillsList();
        public Task<JsonResult> GetUserDetails(long userId);
        public Task<JsonResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel);
        public Task<JsonResult> ContactUs(ContactUsViewModel contact);
        public Task<JsonResult> UpdateUserDetails(UserDetailsViewModel userDetails);
    }
}
