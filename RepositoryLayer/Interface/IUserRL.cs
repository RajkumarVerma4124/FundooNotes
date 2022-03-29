﻿using CommonLayer.Models;
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
        UserEntity Register(UserReg userReg);
        LoginResponse Login(UserLogin userLogin);
        string ForgetPassword(string emailId);
        string ResetPassword(ResetPassword resetPassword, string emailId);
    }
}
