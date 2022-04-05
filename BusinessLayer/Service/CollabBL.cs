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

        //Method To Return Repo Layer EmailExist Method

        public bool IsEmailIdExist(string emailId, long noteId)
        {
            try
            {
                return colabRL.IsEmailIdExist(emailId, noteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Return Repo Layer AddCollaborater Method
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

        //Method To Return Repo Layer DeleteCollaborater Method
        public string DeleteCollaborator(long collabId, long userId)
        {
            try
            {
                return colabRL.DeleteCollaborator(collabId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Return Repo Layer GetCollaborater Method
        public IEnumerable<CollaboratorEntity> GetNoteCollaborators(long noteId)
        {
            try
            {
                return colabRL.GetNoteCollaborators(noteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
