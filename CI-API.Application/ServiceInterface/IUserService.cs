using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.ServiceInterface
{
    public interface IUserService
    {
        #region Method of UserService
        public Task<JsonResult> Login(LoginViewModel userDetail);
        public Task<JsonResult> Register(RegisterViewModel userDetail);
        public Task<JsonResult> ForgetPassword(string email);
        public Task<JsonResult> ResetPassword(string email, string token, string newPassword);

        #endregion

    }
}
