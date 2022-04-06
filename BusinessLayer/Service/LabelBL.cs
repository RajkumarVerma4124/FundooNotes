using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The Label Business Layer Class To Implement IColabBL Methods
    /// </summary>
    public class LabelBL : ILabelBL
    {
        //Reference Object For Interface IUserRL
        private readonly ILabelRL labelRL;

        //Created Constructor With Dependency Injection For IUSerRL
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public bool IsLabelExist(string labelName)
        {
            try
            {
                return labelRL.IsLabelExist(labelName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsLabelInNoteExist(string labelName, long noteId)
        {
            try
            {
                return labelRL.IsLabelInNoteExist(labelName, noteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LabelsEntity CreateNoteLabel(NotesLabel notesLabel, long userId)
        {
            try
            {
                return labelRL.CreateNoteLabel(notesLabel, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LabelsEntity AddLabelToNote(long labelId, long noteId, long userId)

        {
            try
            {
                return labelRL.AddLabelToNote(labelId, noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LabelsEntity EditLabel(string newLabelName, long userId, long labelId)
        {
            try
            {
                return labelRL.EditLabel(newLabelName, userId, labelId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LabelsEntity CreateNewLabel(string labelName, long userId)
        {
            try
            {
                return labelRL.CreateNewLabel(labelName, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RemoveLabel(long labelId, long userId)
        {
            try
            {
                return labelRL.RemoveLabel(labelId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteLabel(string labelName, long userId)
        {
            try
            {
                return labelRL.DeleteLabel(labelName, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
