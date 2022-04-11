using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The User Business Layer Class To Implement IUserBL Methods
    /// </summary>
    public class UserBL : IUserBL
    {
        /// <summary>
        /// Reference Object For Interface IUserRL
        /// </summary>
        private readonly IUserRL userRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IUSerRL
        /// </summary>
        /// <param name="userRL"></param>
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        /// <summary>
        /// Method To Return Repo Layer Registered User Data Method
        /// </summary>
        /// <param name="userReg"></param>
        /// <returns></returns>
        public UserEntity Register(UserReg userReg)
        {
            try
            {
                return userRL.Register(userReg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method To Return Rep Layer Login User Data Method
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public LoginResponse Login(UserLogin userLogin)
        {
            try
            {
                return this.userRL.Login(userLogin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Call And Return Forget Password Method Of Repo Layers
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public string ForgetPassword(string emailId)
        {
            try
            {
                return this.userRL.ForgetPassword(emailId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Call And Return Reset Password For Authorized User 
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public string ResetPassword(ResetPassword resetPassword, string emailId)
        {
            try
            {
                return this.userRL.ResetPassword(resetPassword, emailId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Call And Return IsEmailIdExist Result 
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public bool IsEmailIdExist(string emailId)
        {
            try
            {
                return this.userRL.IsEmailIdExist(emailId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Call And Return IsEmailIdExist Result 
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserTicket CreateTicketForPassword(string emailId, string token)
        {
            try
            {
                return this.userRL.CreateTicketForPassword(emailId, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
