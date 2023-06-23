using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Common.CommonMethods
{
    public class CommonMethods
    {
        #region CreateJwt
        public static string CreateJwt(User user)
        {
            try
            {
                var jwtTokenHaandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("hellociplateform...");

                var identity = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}")
                });

                var credetials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddMinutes(30),
                    SigningCredentials = credetials,
                };

                var token = jwtTokenHaandler.CreateToken(tokenDescriptor);
                return jwtTokenHaandler.WriteToken(token);
            }
            catch 
            {
                return "";
            }
        }
        #endregion

        #region SendMailForResetPassword
        public static string SendEmailForPasswordReset(User userExists)
        {
            byte[] byteForEmail = Encoding.ASCII.GetBytes(userExists.Email);
            string encryptedEmail = Convert.ToBase64String(byteForEmail);
            var token = Guid.NewGuid().ToString();
            UriBuilder builder = new();
            builder.Scheme = "http";
            builder.Host = "localhost";
            builder.Port = 4200;
            builder.Path = "/resetPassword";
            builder.Query = "token=" + token + "&email=" + encryptedEmail;
            var resetLink = builder.ToString();
            // Send email to user with reset password link
            // ...
            var fromAddress = new MailAddress("evanzandu@gmail.com", "CI Platform");
            var toAddress = new MailAddress(userExists.Email);
            var subject = "Password reset request";
            var body = $"<h3>Hello {userExists.FirstName}</h3>,<br />we received password reset request from your side,<br /><br />Please click on the following link to reset your password <br /> this link is valid till 15 min   <br /><a href='{resetLink}'><h3>Click here</h3></a>";
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("evanzandu@gmail.com", "timrrquqhqzvdpns"),
                    EnableSsl = true
                };
                smtpClient.Send(message);
                return token;
            }
            catch 
            {
                return "Email not sent";
                
            }
        }

        #endregion

    }
}
