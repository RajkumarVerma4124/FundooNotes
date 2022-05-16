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
        /// <summary>
        /// Reference Object For FundooContext
        /// </summary>
        private readonly FundooContext fundooContext;

        /// <summary>
        /// Created Constructor To Initialize Fundoocontext For Each Instance
        /// </summary>
        /// <param name="fundooContext"></param>
        public CollabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        /// <summary>
        /// Method to check if current email id provided by user is exist in Collab table or not
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Fetch The Notes And Add The Collaborator Using Note Id And UserId
        /// </summary>
        /// <param name="notesCollab"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                    CollabUserNotesEntity collabUser = new CollabUserNotesEntity()
                    {
                        UserId = collaboratorEntity.UserId,
                        CollabId = collaboratorEntity.CollabId,
                        IsPinned = false,
                        IsArchive = false,
                        Reminder = DateTime.Now.AddDays(30),
                        NoteColor = "#ffffff"
                    };
                    fundooContext.CollabUserNotesData.Add(collabUser);
                    var collabResult = fundooContext.SaveChanges();
                    if (result > 0)
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

        /// <summary>
        /// Method To Fetch And Delete The Collaborator Using Note Id And UserId
        /// </summary>
        /// <param name="collabId"></param>
        /// <param name="notesId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string DeleteCollaborator(long collabId, long notesId, long userId)
        {
            try
            {
                var collabRes = fundooContext.CollaboratorData.FirstOrDefault(c => c.CollabId == collabId && c.NoteId == notesId);
                //var collabUserNotesRes = fundooContext.CollabUserNotesData.FirstOrDefault(c => c.CollabId == collabId && c.UserId == collabRes.UserId);
                if (collabRes != null && collabRes != null)
                {
                    fundooContext.CollaboratorData.Remove(collabRes);
                    var result = fundooContext.SaveChanges();
                    if (result > 0 )
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

        /// <summary>
        /// Method To Fetch The Note Collaborators Lists
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                        FirstName = ownerCollabDetails.FirstName,
                        LastName = ownerCollabDetails.LastName,
                        CollabId = null,
                        CollabEmail = ownerCollabDetails.EmailId,
                        UserId = userId,
                        NoteId = noteId,
                    };
                    collabNotesList.Add(collabOwner);

                    //Condition For Adding CollabsUser
                    foreach (var collab in collabList)
                    {
                        var collabUser = fundooContext.UserData.FirstOrDefault(o => o.UserId == collab.UserId);
                        CollabListResponse collabUsers = new CollabListResponse()
                        {
                            FirstName = collabUser.FirstName,
                            LastName = collabUser.LastName,
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

        public IEnumerable<CollabListResponse> GetAllNotesCollaborators()
        {
            try
            {
                List<CollabListResponse> collabNotesList = new List<CollabListResponse>();
                var collabList = fundooContext.CollaboratorData.ToList();
                if ( collabList.Count() > 0)
                {
                    //Condition For Adding CollabsUser
                    foreach (var collab in collabList)
                    {
                        var collabUser = fundooContext.UserData.FirstOrDefault(o => o.UserId == collab.UserId);
                        CollabListResponse collabUsers = new CollabListResponse()
                        {
                            FirstName = collabUser.FirstName,
                            LastName = collabUser.LastName,
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
