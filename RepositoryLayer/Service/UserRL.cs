using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The User Repository Layer Class To Implement IUserRL Methods
    /// </summary>
    public class UserRL : IUserRL
    {
        /// <summary>
        /// Reference Object For FundooContext And IConfiguration
        /// </summary>
        private readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Declare And Intialize A Value For The Secret Key
        /// </summary>
        private static string Key = "47c53aa7571c33d2f98d02a4313c4ba1ea15e45c18794eb564b21c19591805ff";

        //Created Constructor To Initialize Fundoocontext For Each Instance
        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        /// <summary>
        /// Method to check if current email id provided by user is exist in db table or not
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public bool IsEmailIdExist(string emailId)
        {
            var emailIds = fundooContext.UserData.Where(eu => eu.EmailId == emailId).Count();
            return emailIds > 0;
        }

        //Method to register user with new user data into the db table
        public UserEntity Register(UserReg userReg)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userReg.FirstName;
                userEntity.LastName = userReg.LastName;
                userEntity.EmailId = userReg.EmailId;
                userEntity.Password = PasswordEncrypt(userReg.Password);
                fundooContext.UserData.Add(userEntity);
                int res = fundooContext.SaveChanges();
                if (res > 0)
                    return userEntity;
                else
                    return null;          
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to login user by checking existing db table with user credentials
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public LoginResponse Login(UserLogin userLogin)
        {
            try
            {
                LoginResponse loginResponse = new LoginResponse();
                //Checking If The EmailId Or PassWord Is Null
                if (string.IsNullOrEmpty(userLogin.EmailId) || string.IsNullOrEmpty(userLogin.Password))
                    return null;
                else
                {
                    //Checking the tbl with given user email id if its exist or not
                    loginResponse.UserData = fundooContext.UserData.Where(u => u.EmailId == userLogin.EmailId).FirstOrDefault();
                    if (loginResponse.UserData != null)
                    {
                        string decryptPass = PasswordDecrypt(loginResponse.UserData.Password);
                        if (decryptPass == userLogin.Password)
                        {
                            loginResponse.Token = GenerateSecurityToken(loginResponse.UserData.EmailId, loginResponse.UserData.UserId);
                            return loginResponse;
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Send Token To The Registered Emailid For The User Who Forgots The Password
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public string ForgetPassword(string emailId)
        {
            try
            {
                var userDetails = fundooContext.UserData.Where(u => u.EmailId == emailId).FirstOrDefault();
                if (userDetails != null)
                {
                    var token = GenerateSecurityToken(userDetails.EmailId, userDetails.UserId);
                    new Msmq().SendMessage(token, userDetails.EmailId, userDetails.FirstName);
                    return token;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Send Token To The Registered Emailid For The User Who Forgots The Password
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserTicket CreateTicketForPassword(string emailId, string token)
        {
            try
            {
                var userDetails = fundooContext.UserData.FirstOrDefault(u => u.EmailId == emailId);
                if (userDetails != null)
                {
                    UserTicket ticketResonse = new UserTicket
                    {
                        FirstName = userDetails.FirstName,
                        LastName = userDetails.LastName,
                        EmailId = emailId,
                        Token = token,
                        IssueAt = DateTime.Now
                    };
                    return ticketResonse;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Reset The Password For Autheniticated EmailId After Token AUthorization 
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public string ResetPassword(ResetPassword resetPassword, string emailId)
        {
            try
            {
                if (resetPassword.NewPassword.Equals(resetPassword.ConfirmPassword))
                {
                    var userDetails = fundooContext.UserData.Where(u => u.EmailId == emailId).FirstOrDefault();
                    userDetails.Password = PasswordEncrypt(resetPassword.ConfirmPassword);
                    fundooContext.SaveChanges();
                    return "Resetted The Password SuccessFully";
                }
                else
                {
                    return "Password Does Not Match";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Encrypt The Password To Store Into The DB
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string PasswordEncrypt(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                    return null;
                else
                {
                    password += Key;
                    var passwordBytes = Encoding.UTF8.GetBytes(password);
                    return Convert.ToBase64String(passwordBytes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Decrypt The Password From The DB
        /// </summary>
        /// <param name="encodedPassword"></param>
        /// <returns></returns>
        public static string PasswordDecrypt(string encodedPassword)
        {
            try
            {
                //Decrypting the password
                if (string.IsNullOrEmpty(encodedPassword))
                    return null;
                else
                {
                    var encodedBytes = Convert.FromBase64String(encodedPassword);
                    var res = Encoding.UTF8.GetString(encodedBytes);
                    var resPass = res.Substring(0, res.Length - Key.Length);
                    return resPass;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Generate Security Token For A User
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string GenerateSecurityToken(string emailId, long userId)
        {
            try
            {
                //Genearting A Json Web Toekn For Authorization
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(ClaimTypes.Email, emailId),
                new Claim("UserId", userId.ToString())
                };
                var token = new JwtSecurityToken(
                  issuer: configuration["Jwt:Issuer"],
                  audience: configuration["Jwt:Audience"],
                  claims,
                  expires: DateTime.Now.AddHours(6),
                  signingCredentials: credentials
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
