using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The Collaboratory Repository Layer Class To Implement ICollabRL Methods
    /// </summary>
    public class ColabRL
    {
        //Reference Object For FundooContext
        private readonly FundooContext fundooContext;

        //Created Constructor To Initialize Fundoocontext For Each Instance
        public ColabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
    }
}
