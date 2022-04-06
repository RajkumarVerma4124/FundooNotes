﻿using CommonLayer.Models;
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

        //Method to check if current Label Name Exist Or Not;
        public bool IsLabelExist(string labelName)
        {
            try
            {
                //Condition For Checking Lable Name If Its Exist Or Not
                var LabelCount = fundooContext.LabelsData.Where(l => l.LabelName == labelName).Count();
                return LabelCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method to check if current Label Name In Note Exist Or Not;
        public bool IsLabelInNoteExist(string labelName, long noteId)
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

        //Method To Create New Labels Without Adding To Notes 
        public LabelsEntity CreateNewLabel(string labelName, long userId)
        {
            try
            {
                var notesDetails = fundooContext.NotesData.Where(n => n.UserId == userId).FirstOrDefault();
                if (notesDetails != null)
                {
                    LabelsEntity labels = new LabelsEntity()
                    {
                        LabelName = labelName,
                        UserId = userId,
                        NoteId = null,
                    };
                    fundooContext.LabelsData.Add(labels);
                    var result = fundooContext.SaveChanges();
                    if (result > 0)
                        return labels;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Create New Labels In Notes If It Doesnt Have Any Existing Labels
        public LabelsEntity CreateNoteLabel(NotesLabel notesLabel, long userId)
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
                    if (result > 0)
                        return labels;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Notes And Add The Label Using Note Id And UserId
        public LabelsEntity AddLabelToNote(long labelId, long noteId, long userId)
        {
            try
            {
                var noteDetails = fundooContext.NotesData.Where(n => n.NoteId == noteId && n.UserId == userId).FirstOrDefault();
                var labelDetails = fundooContext.LabelsData.Where(n => n.LabelId == labelId).FirstOrDefault();
                if (labelDetails != null && noteDetails != null)
                {
                    labelDetails.NoteId = noteId;
                    fundooContext.LabelsData.Update(labelDetails);
                    var result = fundooContext.SaveChanges();
                    if (result > 0)
                        return labelDetails;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Notes And Edit The Label Using Note Id And UserId
        public LabelsEntity EditLabel(string newLabelName, long userId, long labelId)
        {
            try
            {
                var labelDetails = fundooContext.LabelsData.Where(l => l.LabelId == labelId && l.UserId == userId).FirstOrDefault();
                if (labelDetails != null)
                {
                    labelDetails.LabelName = newLabelName;
                    var result = fundooContext.SaveChanges();
                    if (result > 0)
                        return labelDetails;
                    else
                        return null;
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