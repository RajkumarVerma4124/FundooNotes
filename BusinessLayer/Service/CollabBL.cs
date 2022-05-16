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
    /// Created The Collab Business Layer Class To Implement IColabBL Methods
    /// </summary>
    public class CollabBL : ICollabBL
    {
        /// <summary>
        /// Reference Object For Interface IUserRL
        /// </summary>
        private readonly ICollabRL colabRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IUSerRL
        /// </summary>
        /// <param name="colabRL"></param>
        public CollabBL(ICollabRL colabRL)
        {
            this.colabRL = colabRL;
        }

        /// <summary>
        /// Method To Return Repo Layer EmailExist Method
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public bool IsEmailIdExist(string emailId, long noteId, long userId)
        {
            try
            {
                return colabRL.IsEmailIdExist(emailId, noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Return Repo Layer AddCollaborater Method
        /// </summary>
        /// <param name="notesCollab"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer DeleteCollaborater Method
        /// </summary>
        /// <param name="collabId"></param>
        /// <param name="notesId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string DeleteCollaborator(long collabId, long notesId, long userId)
        {
            try
            {
                return colabRL.DeleteCollaborator(collabId, notesId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method To Return Repo Layer GetCollaborater Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<CollabListResponse> GetNoteCollaborators(long noteId, long userId)
        {
            try
            {
                return colabRL.GetNoteCollaborators(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CollabListResponse> GetAllNotesCollaborators()
        {
            try
            {
                return colabRL.GetAllNotesCollaborators();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
