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
        CollaboratorEntity AddCollaborator(NotesCollab notesCollab, long userId);
    }
}
