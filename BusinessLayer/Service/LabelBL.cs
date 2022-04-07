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

        //Method To Return Repo Layer IsLabelExist Method
        public bool IsLabelExist(string labelName, long userId)
        {
            try
            {
                return labelRL.IsLabelExist(labelName, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Return Repo Layer CreateNewLabel Method
        public LabelNameEntity CreateNewLabel(string labelName, long userId)
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

        //Method To Return Repo Layer AddNoteLabel Method
        public LabelsEntity AddNoteLabel(NotesLabel notesLabel, long userId)
        {
            try
            {
                return labelRL.AddNoteLabel(notesLabel, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Return Repo Layer EditLabel Method
        public LabelNameEntity EditLabel(string newLabelName, long userId, long labelId)
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



        //Method To Return Repo Layer RemoveLabel Method
        public string RemoveLabel(long labelId, long noteId, long userId)
        {
            try
            {
                return labelRL.RemoveLabel(labelId, noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Return Repo Layer DeleteLabel Method
        public string DeleteLabel(long labelNameId, long userId)
        {
            try
            {
                return labelRL.DeleteLabel(labelNameId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Return Repo Layer GetNotesLabels Method
        public IEnumerable<LabelsResponse> GetNotesLabels(long noteId, long userId)
        {
            try
            {
                return labelRL.GetNotesLabels(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            };
        }

        //Method To Return Repo Layer GetUsersLabelsList Method
        public IEnumerable<LabelsResponse> GetUsersLabelsList(long userId)
        {
            try
            {
                return labelRL.GetUsersLabelsList(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Return Repo Layer GetUsersLabelsNameList Method
        public IEnumerable<LabelNameEntity> GetUsersLabelNamesList(long userId)
        {
            try
            {
                return labelRL.GetUsersLabelNamesList(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
