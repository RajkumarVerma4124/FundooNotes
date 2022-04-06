using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    /// <summary>
    /// Created The Label Interface For Label Repository Layer Class
    /// </summary>
    public interface ILabelRL
    {
        bool IsLabelExist(string labelName, long noteId);
        LabelsEntity CreateLabel(NotesLabel notesLabel, long userId);
    }
}
