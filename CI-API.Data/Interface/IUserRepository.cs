using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Interface
{
    public interface IUserRepository
    {
        #region Methods of UserRepository
        public Task<JsonResult> Login(LoginViewModel userDetail);
        public Task<JsonResult> RegisterUser(RegisterViewModel userDetail);
        public Task<JsonResult> ForgetPassword(string email);
        public Task<JsonResult> ResetPassword(string email, string token, string newPassword);

        #endregion
    }
}
