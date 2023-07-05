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
        public static string DataNotFound = "Data Not Found";

        #endregion

        #region AdminPanel

        #region User
        public static string UserNotUpdated = "User data not updated please check details again and try after sometime";
        public static string UserNotDeleted = "User data not deleted please check details again and try after sometime";
        public static string UserUpdatedSuccess = "User data updated successfully";
        public static string UserDeletedSuccess = "User data deleted successfully";
        #endregion

        #region Mission
        public static string MissionAddedSuccessfully = "New mission added successfully";
        public static string MissionUpdatedSuccessfully = "Mission updated successfully";
        public static string MissionDeletedSuccessfully = "Mission deleted successfully";
        public static string MissionNotUpdatedSuccessfully = "Mission not updated ";

        #endregion

        #region CMS
        public static string CmsAddedSuccess = "New CMS Page added successfully";
        public static string CmsUpdateSuccess = "CMS Page updated successfully";
        public static string CmsDeletedSuccess = "CMS Page deleted successfully";
        #endregion

        #region missionApplication
        public static string MissionApplicationApproved = "Mission application approved successfully";
        public static string MissionApplicationRejected = "Mission application is rejected";

        #endregion

        #region storyAdmin
        public static string StoryApproved = "Story approved successfully";
        public static string StoryRejected = "Story is rejected";
        public static string StoryDeleted = "Story is deleted!!";

        #endregion

        #region bannerAdmin
        public static string BannerSortOrderAlreadyExist = "Please add different sortOrder banner for this sort order already exists";
        public static string BannerAddedSuccess = "Banner added successfully";
        public static string BannerUpdateSuccess = "Banner updated successfully";
        public static string BannerDeletedSuccess = "Banner deleted successfully";
        #endregion

        #region theme
        public static string ThemeAddedSuccess = "Theme added successfully";
        public static string ThemeUpdateSuccess = "Theme updated successfully";
        public static string ThemeDeletedSuccess = "Theme deleted successfully";
        public static string DeleteThemeBasedMissionFirst = "Please delete theme based mission first then you can delete or update the theme status";
        #endregion

        #endregion

        #region Volunteer-Mission
        public static string CommentDone = "Your comment has been sent successfully.";
        public static string CommentNotDone = "Please Apply this mission";
        public static string AddToFavourite = "This Mission Is  favourite ";
        public static string RemoveToFavourite = "Remove favourite";
        public static string RecommandedMission = "This Mission is shared With Your Friend"; 
        public static string ApplyMission = "successfully Applied The Mission";
        public static string AllMissionSuccess = "successfully get All Mission Data !";
        public static string IdNotFound = "Invalid Data";
        public static string Usersuccess = "successfully get All User Data !";
        public static string SentEmailRecommanded = "Email sent successfully go to your Gmail(📧) ";
        #endregion

        #region CommonMessage
        public static string Success = "Success";
        #endregion

        #region UserDetails
        public static string OldPasswordMismatch = "Old password does not match";
        public static string PasswordChanged = "Password changed successfully";
        public static string UserProfileUpdated = "Profile Updated Successfully";
        #endregion

        #region ContactUs
        public static string ContactSuccess = "Message sent successfully";
        public static string ContactError = "Error Occurred";
        #endregion

        #region
        public static string FavMissionSuccess = "Favourite Mission Change Successfully"; 
        public static string RecommendedSuccess = "Email Has Been Sent Successfully";
        public static string InvalidData = "Data Invalid";
        #endregion


        #endregion
    }
}
