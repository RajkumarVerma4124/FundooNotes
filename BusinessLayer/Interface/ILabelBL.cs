using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Created The Interface For Label Business Layer Class 
    /// </summary>
    public interface ILabelBL
    {
        bool IsLabelExist(string labelName, long noteId);
        LabelsEntity CreateLabel(NotesLabel notesLabel, long userId);
    }
}
