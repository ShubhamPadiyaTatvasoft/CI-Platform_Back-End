using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CI_API.Common.CommonModels;
using CI_API.Common.CommonMethods;

namespace CI_API.Data.Repository
{
    public class UserRepository : IUserRepository
    {

        #region Dependency Injection of DbContext 

        private readonly CipContext cIDbContext;

        public UserRepository(CipContext _cIDbContext)
        {
            cIDbContext = _cIDbContext;
        }
        #endregion

        #region Login

        public async Task<JsonResult> Login(LoginViewModel userDetail)
        {
            try
            {

                User loginData = await Task.FromResult(cIDbContext.Users.Where(U => U.Email == userDetail.email).FirstOrDefault());
                
                byte[] byteForPassword = Convert.FromBase64String(loginData.Password);
                string decryptedPassword = Encoding.ASCII.GetString(byteForPassword);

                if (loginData != null && decryptedPassword == userDetail.password)
                {
                    string token = "";
                    token = CommonMethods.CreateJwt(loginData);
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.LoginSuccess, StatusCode = responseStatusCode.Success, Data = token, Result = true });

                }
                else
                {

                    if (await Task.FromResult(cIDbContext.Users.Where(U => U.Email == userDetail.email)) == null)
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InvalidLoginCredentials, StatusCode = responseStatusCode.InvalidData, Result = false });
                    }
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }

        #endregion

        #region Register

        public async Task<JsonResult> RegisterUser(RegisterViewModel userDetail)
        {
            try
            {

                var userExist = await Task.FromResult(cIDbContext.Users.Where(U => U.Email == userDetail.email).FirstOrDefault());
                if (userExist != null)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserAlreadyExist, StatusCode = responseStatusCode.AlreadyExist, Result = false });
                }
                else
                {
                    byte[] byteForPassword = Encoding.ASCII.GetBytes(userDetail.password);
                    string encryptedPassword = Convert.ToBase64String(byteForPassword);
                    User user = new User()
                    {

                        FirstName = userDetail.firstName,
                        LastName = userDetail.lastName,
                        Email = userDetail.email,
                        Password = encryptedPassword,
                        PhoneNumber = (long)userDetail.phoneNumber,
                    };
                    cIDbContext.Add(user);
                    cIDbContext.SaveChanges();
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.RegistrationSuccess, StatusCode = responseStatusCode.Success, Result = true });
                }
            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }

        #endregion

        #region ForgetPassword
        public async Task<JsonResult> ForgetPassword(string email)
        {
            try
            {
                var userExists = await Task.FromResult(cIDbContext.Users.Where(U => U.Email == email).FirstOrDefault());

                if (userExists != null)
                {
                    PasswordReset requestExists = await Task.FromResult(cIDbContext.PasswordResets.Where(P => P.Email == email).FirstOrDefault());
                    if (requestExists != null)
                    {
                        cIDbContext.PasswordResets.Remove(requestExists);
                    }

                    string tokenAfterMailSent = await Task.FromResult(CommonMethods.SendEmailForPasswordReset(userExists));

                    if (tokenAfterMailSent != "Email not sent")
                    {
                        PasswordReset passwordReset = new PasswordReset()
                        {
                            Email = email,
                            Token = tokenAfterMailSent,
                        };

                        cIDbContext.Add(passwordReset);
                        var abc = cIDbContext.SaveChanges();
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.EmailSentSuccessfully, StatusCode = responseStatusCode.Success, Result = true });
                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.EmailNotSend, StatusCode = responseStatusCode.BadRequest, Result = false });
                    }
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                }

            }

            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }

        #endregion

        #region ResetPassword
        public async Task<JsonResult> ResetPassword(string email, string token, string newPassword)
        {
            byte[] byteForEmail;
            string decryptedEmail;
            try
            {
                byteForEmail = Convert.FromBase64String(email);
                decryptedEmail = Encoding.ASCII.GetString(byteForEmail);

                User? userExist = await Task.FromResult(cIDbContext.Users.Where(U => U.Email == decryptedEmail).FirstOrDefault());

                if (userExist != null)
                {
                    var tokenExists = await Task.FromResult(cIDbContext.PasswordResets.Where(R => R.Token == token && R.Email == decryptedEmail).FirstOrDefault());

                    if (tokenExists != null)
                    {
                        if (tokenExists.CreatedAt.AddMinutes(15) > DateTime.Now)
                        {

                            byte[] byteForPassword = Encoding.ASCII.GetBytes(newPassword);
                            string encryptedPassword = Convert.ToBase64String(byteForPassword);

                            userExist.Password = encryptedPassword;

                            cIDbContext.PasswordResets.Remove(tokenExists);
                            cIDbContext.SaveChanges();

                            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.PasswordResetSuccess, StatusCode = responseStatusCode.Success, Result = true });
                        }
                        else
                        {
                            cIDbContext.PasswordResets.Remove(tokenExists);
                            cIDbContext.SaveChanges();
                            return new JsonResult(new apiResponse<string> { Message = ResponseMessages.LinkExpired, StatusCode = responseStatusCode.InvalidData, Result = false });
                        }
                    }
                    else
                    {
                        return new JsonResult(new apiResponse<string> { Message = ResponseMessages.LinkExpired, StatusCode = responseStatusCode.InvalidData, Result = false });
                    }
                }
                else
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                }

            }
            catch
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
        }

        #endregion

    }
}
