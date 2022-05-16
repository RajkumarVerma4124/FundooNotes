using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The User Interface For User Repository Layer Class
    /// </summary>
    public interface IUserRL
    {
        UserTicket CreateTicketForPassword(string emailId, string token);
        bool IsEmailIdExist(string emailId);
        UserEntity Register(UserReg userReg);
        LoginResponse Login(UserLogin userLogin);
        string ForgetPassword(GetForgotPassword getForgotPasswordd);
        string ResetPassword(ResetPassword resetPassword, string emailId);
    }
}
