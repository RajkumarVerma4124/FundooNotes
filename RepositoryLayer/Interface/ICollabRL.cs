using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The Collab Interface For Collab Repository Layer Class
    /// </summary>
    public interface ICollabRL
    {
        bool IsEmailIdExist(string emailId, long noteId);
        CollaboratorEntity AddCollaborator(NotesCollab notesCollab, long userId);
        string DeleteCollaborator(long collabId, long notesId, long userId);
        IEnumerable<CollaboratorEntity> GetNoteCollaborators(long noteId, long userId);
    }
}
