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
        bool IsLabelExist(string labelName, long userId);
        LabelNameEntity CreateNewLabel(string labelName, long userId);
        LabelsEntity AddNoteLabel(NotesLabel notesLabel, long userId);
        LabelNameEntity EditLabel(string newLabelName, long userId, long labelNameId);
        string RemoveLabel(long labelId, long noteId, long userId);
        string DeleteLabel(long labelNameId, long userId);
        IEnumerable<LabelsResponse> GetNotesLabels(long noteId, long userId);
        IEnumerable<LabelsResponse> GetUsersLabelsList(long userId);
        IEnumerable<LabelNameEntity> GetUsersLabelNamesList(long userId);
    }
}
