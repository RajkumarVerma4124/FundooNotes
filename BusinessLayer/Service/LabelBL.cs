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
        /// <summary>
        /// Reference Object For Interface IUserRL
        /// </summary>
        private readonly ILabelRL labelRL;

        /// <summary>
        /// Created Constructor With Dependency Injection For IUSerRL
        /// </summary>
        /// <param name="labelRL"></param>
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        /// <summary>
        /// Method To Return Repo Layer IsLabelExist Method
        /// </summary>
        /// <param name="labelName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer CreateNewLabel Method
        /// </summary>
        /// <param name="labelName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer AddNoteLabel Method
        /// </summary>
        /// <param name="notesLabel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer EditLabel Method
        /// </summary>
        /// <param name="newLabelName"></param>
        /// <param name="userId"></param>
        /// <param name="labelId"></param>
        /// <returns></returns>
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



        /// <summary>
        /// Method To Return Repo Layer RemoveLabel Method
        /// </summary>
        /// <param name="labelId"></param>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer DeleteLabel Method
        /// </summary>
        /// <param name="labelNameId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer GetNotesLabels Method
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer GetUsersLabelsList Method
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method To Return Repo Layer GetUsersLabelsNameList Method
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
