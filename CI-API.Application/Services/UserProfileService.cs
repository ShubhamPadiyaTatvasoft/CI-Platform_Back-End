using CI_API.Application.ServiceInterface;
using CI_API.Core.ViewModel;
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
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository userProfileRepository;
        public UserProfileService(IUserProfileRepository _userProfileRepository)
        {
            userProfileRepository = _userProfileRepository;
        }

        public async Task<JsonResult> GetCountriesList()
        {
            return await userProfileRepository.GetCountriesList();
        }

        public async Task<JsonResult> GetCitiesList()
        {
            return await userProfileRepository.GetCitiesList();
        }

        public async Task<JsonResult> GetCitiesList(long countryId)
        {
            return await userProfileRepository.GetCitiesList(countryId);
        }

        public async Task<JsonResult> GetAvailability()
        {
            return await userProfileRepository.GetAvailability();
        }

        public async Task<JsonResult> GetSkillsList()
        {
            return await userProfileRepository.GetSkillsList();
        }

        public async Task<JsonResult> GetUserDetails(long userId)
        {
            return await userProfileRepository.GetUserDetails(userId);
        }

        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            return await userProfileRepository.ChangePassword(changePassword);
        }

        public async Task<JsonResult> ContactUs(ContactUsViewModel contact)
        {
            return await userProfileRepository.ContactUs(contact);
        }

        public async Task<JsonResult> UpdateUserDetails(UserDetailsViewModel userDetails)
        {
            return await userProfileRepository.UpdateUserDetails(userDetails);
        }
    }
}
