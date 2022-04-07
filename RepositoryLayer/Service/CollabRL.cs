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
        public bool IsEmailIdExist(string emailId, long noteId, long userId)
        {
            try
            {
                var ownerNotesDetails = fundooContext.NotesData.FirstOrDefault(o => o.NoteId == noteId);
                var ownerCollabDetails = fundooContext.UserData.FirstOrDefault(o => o.UserId == ownerNotesDetails.UserId);
                if (ownerCollabDetails.EmailId == emailId)
                    return true;
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
                    fundooContext.CollaboratorData.Remove(collabRes);
                    var result = fundooContext.SaveChanges();
                    if (result > 0)
                        return "Collaborater Deleted Succesfully";
                    else
                        return null;
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
        public IEnumerable<CollabListResponse> GetNoteCollaborators(long noteId, long userId)
        {
            try
            {
                List<CollabListResponse> collabNotesList = new List<CollabListResponse>();
                var ownerNotesDetails = fundooContext.NotesData.FirstOrDefault(o => o.NoteId == noteId);
                var ownerCollabDetails = fundooContext.UserData.FirstOrDefault(o => o.UserId == ownerNotesDetails.UserId);
                var userDetails = fundooContext.UserData.FirstOrDefault(o => o.UserId == userId);
                var collabList = fundooContext.CollaboratorData.Where(c => c.NoteId == noteId).ToList();
                if (ownerCollabDetails != null || collabList.Count() > 0)
                {
                    //Condition For Adding CollabOwner
                    CollabListResponse collabOwner = new CollabListResponse()
                    {
                        FirstName = ownerCollabDetails.UserId == userId ? ownerCollabDetails.FirstName : null,
                        LastName = ownerCollabDetails.UserId == userId ? ownerCollabDetails.LastName : null,
                        CollabId = null,
                        CollabEmail = ownerCollabDetails.EmailId,
                        UserId = userId,
                        NoteId = noteId,
                    };
                    collabNotesList.Add(collabOwner);

                    //Condition For Adding CollabsUser
                    foreach (var collab in collabList)
                    {
                        CollabListResponse collabUsers = new CollabListResponse()
                        {
                            FirstName = ownerCollabDetails.UserId != userId && collab.UserId == userId ? userDetails.FirstName : null,
                            LastName = ownerCollabDetails.UserId != userId && collab.UserId == userId ? userDetails.LastName : null,
                            CollabId = collab.CollabId,
                            CollabEmail = collab.CollabEmail,
                            UserId = collab.UserId,
                            NoteId = collab.NoteId,
                        };
                        collabNotesList.Add(collabUsers);
                    }
                    return collabNotesList;
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
