using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For Colab Business Layer Class 
    /// </summary>
    public interface ICollabBL
    {
        bool IsEmailIdExist(string emailId, long noteId);
        CollaboratorEntity AddCollaborator(NotesCollab notesCollab, long userId);
        string DeleteCollaborator(long collabId, long notesId, long userId);
        IEnumerable<CollaboratorEntity> GetNoteCollaborators(long noteId, long userId);
    }
}
