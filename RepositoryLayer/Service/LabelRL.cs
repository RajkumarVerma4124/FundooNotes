using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Created The Label Repository Layer Class To Implement ILabelRL Methods
    /// </summary>
    public class LabelRL : ILabelRL
    {
        //Reference Object For FundooContext
        private readonly FundooContext fundooContext;

        //Created Constructor To Initialize Fundoocontext For Each Instance
        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        //Method to check if current Label Name With Note Is Present Or Not;
        public bool IsLabelExist(string labelName, long noteId)
        {
            try
            {
                //Condition For Checking Lable Name If Its Exist Or Not
                var LabelCount = fundooContext.LabelsData.Where(l => l.LabelName == labelName && l.NoteId == noteId).Count();
                return LabelCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Notes And Add The Label Using Note Id And UserId
        public LabelsEntity CreateLabel(NotesLabel notesLabel, long userId)
        {
            try
            {
                var notesDetails = fundooContext.NotesData.Where(n => n.NoteId == notesLabel.NoteId && n.UserId == userId).FirstOrDefault();
                if (notesDetails != null)
                {
                    LabelsEntity labels = new LabelsEntity()
                    {
                        LabelName = notesLabel.LabelName,
                        NoteId = notesLabel.NoteId,
                        UserId = notesDetails.UserId
                    };
                    fundooContext.LabelsData.Add(labels);
                    var result = fundooContext.SaveChanges();
                    return labels;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
