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
            try
            {
                //Condition For Checking Collaborator Email If Its Exist Or Not
                var emailIdCount = fundooContext.CollaboratorData.Where(e => e.CollabEmail == emailId && e.NoteId == noteId).Count();
                return emailIdCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            } 
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
                        UserId = userDetails.UserId
                    };
                    fundooContext.CollaboratorData.Add(collaboratorEntity);
                    var result = fundooContext.SaveChanges();
                    if(result > 0)
                        return collaboratorEntity;
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

        //Method To Fetch And Delete The Collaborator Using Note Id And UserId
        public string DeleteCollaborator(long collabId, long notesId, long userId)
        {
            try
            {
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(c => c.CollabId == collabId && c.NoteId == notesId);
                if (collabRes != null)
                {
                    var collabOwner = fundooContext.NotesData.FirstOrDefault(c => c.NoteId == notesId);
                    if(collabOwner.UserId == collabRes.UserId && collabOwner.NoteId == collabRes.NoteId)
                    {
                        return "You Cant Remove The Owner Of Collaborator";
                    }
                    else if(collabOwner.UserId != collabRes.UserId)
                    {
                        fundooContext.CollaboratorData.Remove(collabRes);
                        var result = fundooContext.SaveChanges();
                        if (result > 0)
                            return "Collaborater Deleted Succesfully";
                        else
                            return null;
                    }    
                    else
                        return "You Cant Remove The Owner Of This Note";
                }
                else
                    return "Collaborater Deletion Failed";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Note Collaborators Lists
        public IEnumerable<CollaboratorEntity> GetNoteCollaborators(long noteId, long userId)
        {
            try
            {
                var iscollabExist = fundooContext.CollaboratorData.Where(c => c.NoteId == noteId && c.UserId == userId).ToList();
                if (iscollabExist.Count() > 0)
                {
                    var collabRes = fundooContext.CollaboratorData.Where(c => c.NoteId == noteId).ToList();
                    if (collabRes.Count() > 0)
                        return collabRes;
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
    }
}
