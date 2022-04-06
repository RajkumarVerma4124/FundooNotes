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

        public bool IsLabelExist(string labelName, long noteId)
        {
            try
            {
                return labelRL.IsLabelExist(labelName, noteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LabelsEntity CreateLabel(NotesLabel notesLabel, long userId)
        {
            try
            {
                return labelRL.CreateLabel(notesLabel, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
