using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Common.CommonModels
{
    public class ResponseMessages
    {
        #region ResponseMessages

        #region Registration
        public static string RegistrationSuccess = "Registration done successfully";
        public static string UserAlreadyExist = "User already exists";
        #endregion

        #region Login
        public static string LoginSuccess = "Login done successfully";
        public static string UserNotFound = "User not found!!!";
        public static string InvalidLoginCredentials = "Email or password is incorrect!!";
        #endregion

        #region ForgetPassword
        public static string EmailNotSend = "Email not send please try again after some time";
        public static string EmailSentSuccessfully = "Email sent successfully go to your Gmail(📧) and change the password!!!";
        public static string LinkIsExpired = "Link expired please generate another request!!";
        #endregion

        #region ResetPassword
        public static string PasswordResetSuccess = "Password changes successfully login with new credetials!!";
        public static string LinkExpired = "Link is expired please generate new request!!";
        #endregion

        #region CommonErrorMessage
        public static string InternalServerError = "Internal Server error";

        #endregion

        #endregion
    }
}
