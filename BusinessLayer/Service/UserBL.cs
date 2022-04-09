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
        //Reference Object For Interface IUserRL
        private readonly IUserRL userRL;

        //Created Constructor With Dependency Injection For IUSerRL
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        //Method To Return Registered User Data
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

        //Method To Return Login User Data
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

        //Method To Call And Return Forget Password 
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

        //Method To Call And Return Reset Password For Authorized User 
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

        //Method To Call And Return IsEmailIdExist Result 
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
