﻿using CommonLayer.Models;
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
        bool IsLabelExist(string labelName);
        bool IsLabelInNoteExist(string labelName, long noteId);
        LabelsEntity CreateNoteLabel(NotesLabel notesLabel, long userId);
        LabelsEntity CreateNewLabel(string labelName, long userId);
        LabelsEntity AddLabelToNote(long labelId, long noteId, long userId);
        LabelsEntity EditLabel(string newLabelName, long userId, long labelId);
        string RemoveLabel(long labelId, long userId);
        string DeleteLabel(string labelName, long userId);
        IEnumerable<LabelsEntity> GetNotesLabels(long noteId, long userId);
        IEnumerable<LabelsEntity> GetLabelsList(long userId);
    }
}
