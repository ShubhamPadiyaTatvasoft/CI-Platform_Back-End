using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Interface
{
    public interface IUserProfileRepository
    {
        
        public Task<JsonResult> GetUserDetails(long userId);
        public Task<JsonResult> ChangePassword(ChangePasswordViewModel changePassword);
        public Task<JsonResult> ContactUs(ContactUsViewModel contact);
        public Task<JsonResult> UpdateUserDetails(UserDetailsViewModel userDetails);

    }
}
