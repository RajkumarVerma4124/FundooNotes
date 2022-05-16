using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For User Business Layer Class
    /// </summary>
    public interface IUserBL
    {
        UserTicket CreateTicketForPassword(string emailId, string token);
        bool IsEmailIdExist(string emailId);
        UserEntity Register(UserReg userReg);
        LoginResponse Login(UserLogin userLogin);
        string ForgetPassword(GetForgotPassword getForgotPassword);
        string ResetPassword(ResetPassword resetPassword, string emailId);
    }
}
