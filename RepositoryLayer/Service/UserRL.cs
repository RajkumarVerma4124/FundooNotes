using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The User Repository Layer Class To Implement IUserRL Methods
    /// </summary>
    public class UserRL : IUserRL
    {
        //Reference Object For FundooContext
        private readonly FundooContext fundooContext;

        //Created Constructor To Initialize Fundoocontext For Each Instance
        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        //Method to register user with new user data into the db table
        public UserEntity Register(UserReg userReg)
        {
            try
            {
                //Check the user email before if emailid is already exits in database or not 
                var userDataResult = fundooContext.UserData.FirstOrDefault(eu => eu.EmailId == userReg.EmailId);
                if (userDataResult == null)
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
                else
                    return null;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method to login user by checking existing db table with user credentials
        public UserEntity Login(UserLogin userLogin)
        {
            try
            {
                //Checking If The EmailId Or PassWord Is Null
                if (string.IsNullOrEmpty(userLogin.EmailId) || string.IsNullOrEmpty(userLogin.Password))
                    return null;
                else {
                    //Checking the tb with given user email id and password if its exist or not
                    var userData = fundooContext.UserData.Where(x => x.EmailId == userLogin.EmailId && x.Password == userLogin.Password).FirstOrDefault();
                    return userData;
                }      
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
