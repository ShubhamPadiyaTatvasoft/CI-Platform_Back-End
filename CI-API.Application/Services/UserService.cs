using CI_API.Application.ServiceInterface;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.Services
{
   
    public class UserService : IUserService
    {

        #region Dependency Injection of UserRepository Interface
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository _userRepository)
        {
             userRepository = _userRepository;
        }

        #endregion

        #region Login
        public async Task<JsonResult> Login(LoginViewModel userDetail)
        {
            return await userRepository.Login(userDetail); 
        }
        #endregion

        #region Register
        public async Task<JsonResult> Register(RegisterViewModel userDetail)
        {
            return await userRepository.RegisterUser(userDetail); 
        }
        #endregion

        #region ForgetPassword

        public async Task<JsonResult> ForgetPassword(string email)
        {
            return await userRepository.ForgetPassword(email);
        }
        #endregion

        #region ResetPassword

        public async Task<JsonResult> ResetPassword(string email, string token, string newPassword)
        {
            return await userRepository.ResetPassword(email, token, newPassword);    
        }
        #endregion

        

    }
}
