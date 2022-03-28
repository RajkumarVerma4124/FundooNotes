using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
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
    }
}
