using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;
        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public UserEntity Register(UserReg userReg)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userReg.FirstName;
                userEntity.LastName = userReg.LastName;
                userEntity.EmailId = userReg.EmailId;
                userEntity.Password = userReg.Password;
                fundooContext.Add(userEntity);
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
    }
}
