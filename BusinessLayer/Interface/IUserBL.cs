using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For Business Layer Class
    /// </summary>
    public interface IUserBL
    {
        UserEntity Register(UserReg userReg);
        UserEntity Login(UserLogin userLogin);
    }
}
