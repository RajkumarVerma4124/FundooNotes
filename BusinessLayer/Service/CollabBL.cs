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
    /// Created The Notes Business Layer Class To Implement IColabBL Methods
    /// </summary>
    public class CollabBL: ICollabBL
    {
        //Reference Object For Interface IUserRL
        private readonly ICollabRL colabRL;

        //Created Constructor With Dependency Injection For IUSerRL
        public CollabBL(ICollabRL colabRL)
        {
            this.colabRL = colabRL;
        }

        public CollaboratorEntity AddCollaborator(NotesCollab notesCollab, long userId)
        {
            try
            {
                return colabRL.AddCollaborator(notesCollab, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
