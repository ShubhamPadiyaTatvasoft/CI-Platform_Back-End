using CI_API.Common.CommonModels;
using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using CI_API.Core.ViewModel;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Repository
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly CiPlatformDbContext cIDbContext;

        public UserProfileRepository(CiPlatformDbContext _cIDbContext)
        {
            cIDbContext = _cIDbContext;
        }

        public async Task<JsonResult> GetUserDetails(long userId)
        {
            User user;
            try
            {
               user = await Task.FromResult(cIDbContext.Users.Where(user => user.UserId == userId).Include(user=>user.UserSkills).ThenInclude(userskill=> userskill.Skill).FirstOrDefault());
            }
            catch (Exception ex)
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.InternalServerError, Result = false });
            }
            

            return new JsonResult(new apiResponse<User> { Data = user, Message = ResponseMessages.Success, StatusCode = responseStatusCode.Success, Result = true });
        }

        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel changePassword)
        {

            try
            {
                User user = await Task.FromResult(cIDbContext.Users.Where(user => user.UserId == changePassword.UserId).FirstOrDefault());
                byte[] byteForPassword = Convert.FromBase64String(user.Password);
                string decryptedPassword = Encoding.ASCII.GetString(byteForPassword);
                if (decryptedPassword != changePassword.OldPassword)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.OldPasswordMismatch, StatusCode = responseStatusCode.InvalidData, Result = false });
                }
                if (changePassword.NewPassword == changePassword.ConfirmPassword)
                {
                    byte[] bytePassword = Encoding.ASCII.GetBytes(changePassword.NewPassword);
                    string encryptedPassword = Convert.ToBase64String(bytePassword);
                    user.Password = encryptedPassword;
                    cIDbContext.Users.Update(user);
                    cIDbContext.SaveChanges();
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.PasswordChanged, StatusCode = responseStatusCode.Success, Result = true });
                }
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });
            }
            catch (Exception ex)
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.InternalServerError, Result = false });
            }
        }

        public async Task<JsonResult> ContactUs(ContactUsViewModel contact)
        {
            try
            {
                User user = await Task.FromResult(cIDbContext.Users.Where(user => user.UserId == contact.UserId).FirstOrDefault());
                if (user == null)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                }
                ContactU contactUs = new ContactU() { Email = contact.Email, UserId = contact.UserId, UserName = contact.UserName, Subject = contact.Subject, Message = contact.Message };
                cIDbContext.ContactUs.Add(contactUs);
                cIDbContext.SaveChanges();
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.ContactSuccess, StatusCode = responseStatusCode.Success, Result = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.InternalServerError, Result = false });
            }

        }

        public async Task<JsonResult> UpdateUserDetails(UserDetailsViewModel userDetails)
        {
            try
            {
                User u = cIDbContext.Users.Where(user => user.UserId == long.Parse(userDetails.UserId)).FirstOrDefault();
                if (u == null)
                {
                    return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserNotFound, StatusCode = responseStatusCode.NotFound, Result = false });
                }
                string path=null;
                u.FirstName = userDetails.Name;
                u.LastName = userDetails.Surname;
                u.EmployeeId = userDetails.EmployeeId;
                u.Manager = userDetails.Manager;
                u.Title = userDetails.Title;
                u.Department = userDetails.Department;
                u.ProfileText = userDetails.ProfileText;
                u.WhyIVolunteer = userDetails.WhyVolunteer;
                u.CountryId = long.Parse(userDetails.CountryId);
                u.CityId = long.Parse(userDetails.CityId);
                u.Availability = userDetails.Availability;
                u.LinkedInUrl = userDetails.LinkedIn;
                if (userDetails.Image != null)
                {
                    string folder = "D:/Project/CI-Platform/CI-Platform_Front-End/CI-Platform_Front-End/src/assets/Images/Avatar";
                    string fileName = Guid.NewGuid().ToString() + '_' + userDetails.Image.FileName;
                    path = Path.Combine(folder, fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        userDetails.Image.CopyTo(fileStream);
                    }
                    u.Avatar = path;
                }
                cIDbContext.Update(u);
                if (userDetails.Skills !=null)
                {
                    List<UserSkill> userskills=cIDbContext.UserSkills.Where(skill=>skill.UserId==long.Parse(userDetails.UserId)).ToList();
                    cIDbContext.UserSkills.RemoveRange(userskills);
                    List<int> skillIds = userDetails.Skills.Split(',').Select(int.Parse).ToList();
                    skillIds.ForEach(skillId => { UserSkill u = new UserSkill() { SkillId = skillId, UserId = long.Parse(userDetails.UserId) }; cIDbContext.Add(u); });

                }
                cIDbContext.SaveChanges();
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.UserProfileUpdated, StatusCode = responseStatusCode.Success, Result = true });
            }

            catch (Exception ex)
            {
                return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.InternalServerError, Result = false });
            }
        }
    }
}
