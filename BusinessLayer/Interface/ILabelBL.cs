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
        bool IsLabelExist(string labelName);
        bool IsLabelInNoteExist(string labelName, long noteId);
        LabelsEntity CreateNoteLabel(NotesLabel notesLabel, long userId);
        LabelsEntity CreateNewLabel(string labelName, long userId);
        LabelsEntity AddLabelToNote(long labelId, long noteId, long userId);
        LabelsEntity EditLabel(string newLabelName, long userId, long labelId);
        string RemoveLabel(long labelId, long userId);
        string DeleteLabel(string labelName, long userId);
    }
}
