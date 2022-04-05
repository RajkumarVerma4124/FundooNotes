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
    /// Created The Collaboratory Repository Layer Class To Implement ICollabRL Methods
    /// </summary>
    public class CollabRL: ICollabRL
    {
        //Reference Object For FundooContext
        private readonly FundooContext fundooContext;

        //Created Constructor To Initialize Fundoocontext For Each Instance
        public CollabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        //Method To Add The Collaborator Using Note Id And UserId
        public CollaboratorEntity AddCollaborator(NotesCollab notesCollab, long userId)
        {
            try
            {
                var userDetails = fundooContext.UserData.Where(u => u.EmailId == notesCollab.CollabEmail).FirstOrDefault();
                var notesDetails = fundooContext.NotesData.Where(n => n.NoteId == notesCollab.NoteId && n.UserId == userId).FirstOrDefault();
                if (userDetails != null && notesDetails != null)
                {
                    CollaboratorEntity collaboratorEntity = new CollaboratorEntity()
                    {
                        CollabEmail = notesCollab.CollabEmail,
                        NoteId = notesCollab.NoteId,
                        UserId = userId
                    };
                    fundooContext.CollaboratorData.Add(collaboratorEntity);
                    var result = fundooContext.SaveChanges();
                    return collaboratorEntity;
                }
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
