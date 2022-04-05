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

        //Method to check if current email id provided by user is exist in Collab table or not;
        public bool IsEmailIdExist(string emailId, long noteId)
        {
            var emailIds = fundooContext.CollaboratorData.Where(e => e.CollabEmail == emailId && e.NoteId == noteId).Count();
            return emailIds > 0;
        }

        //Method To Fetch The Notes And Add The Collaborator Using Note Id And UserId
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

        //Method To Fetch And Delete The Collaborator Using Note Id And UserId
        public string DeleteCollaborator(long collabId, long userId)
        {
            try
            {
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(c=> c.CollabId == collabId && c.UserId == userId);
                if (collabRes != null)
                { 
                    fundooContext.CollaboratorData.Remove(collabRes);
                    var result = fundooContext.SaveChanges();
                    return "Collaborater Deleted Succesfully";
                }
                else
                    return "Collaborater Deletion Failed";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
