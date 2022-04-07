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

        //Method to check if current Label Name Exist Or Not;
        public bool IsLabelExist(string labelName, long userId)
        {
            try
            {
                //Condition For Checking Lable Name If Its Exist Or Not
                var LabelCount = fundooContext.LabelsNameData.Where(l => l.LabelName == labelName && l.UserId == userId).Count();
                return LabelCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Create New Labels 
        public LabelNameEntity CreateNewLabel(string labelName, long userId)
        {
            try
            {
                var notesDetails = fundooContext.NotesData.Where(n => n.UserId == userId).FirstOrDefault();
                if (notesDetails != null)
                {
                    LabelNameEntity labels = new LabelNameEntity()
                    {
                        LabelName = labelName,
                        UserId = userId,
                    };
                    fundooContext.LabelsNameData.Add(labels);
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
        public LabelsEntity AddNoteLabel(NotesLabel notesLabel, long userId)
        {
            try
            {
                var notesDetails = fundooContext.NotesData.Where(n => n.NoteId == notesLabel.NoteId && n.UserId == userId).FirstOrDefault();
                var labelsName = fundooContext.LabelsNameData.Where(l => l.LabelName == notesLabel.LabelName).FirstOrDefault(); 
                if (notesDetails != null && labelsName != null)
                {
                    LabelsEntity labels = new LabelsEntity()
                    {
                        LabelNameId = labelsName.LabelNameId,
                        NoteId = notesLabel.NoteId,
                        UserId = userId
                    };
                    fundooContext.LabelsData.Add(labels);
                    var result = fundooContext.SaveChanges();
                    if (result > 0)
                        return labels;
                    else
                        return null;
                }
                else if (notesDetails != null && labelsName == null)
                {
                    var resultLabel = CreateNewLabel(notesLabel.LabelName, userId);
                    LabelsEntity labels = new LabelsEntity()
                    {
                        LabelNameId = resultLabel.LabelNameId,
                        NoteId = notesLabel.NoteId,
                        UserId = userId
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

        //Method To Fetch The Labels And Edit The Label Using label Id And UserId
        public LabelNameEntity EditLabel(string newLabelName, long userId, long labelNameId)
        {
            try
            {
                var labelNameDetails = fundooContext.LabelsNameData.Where(l => l.LabelNameId == labelNameId && l.UserId == userId).FirstOrDefault();
                if (labelNameDetails != null)
                {
                    labelNameDetails.LabelName = newLabelName;
                    var result = fundooContext.SaveChanges();
                    if (result > 0)
                        return labelNameDetails;
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

        //Method To Fetch And Remove The Labels From Notes Using Label Id And UserId
        public string RemoveLabel (long labelId, long noteId, long userId)
        {
            try
            {
                var labelDetails = fundooContext.LabelsData.Where(l => l.LabelId == labelId && l.UserId == userId && l.NoteId == noteId).FirstOrDefault();
                if (labelDetails != null)
                {
                    fundooContext.LabelsData.Remove(labelDetails);
                    var result = fundooContext.SaveChanges();
                    if (result > 0)
                        return "Label Removed Succesfully";
                    else
                        return "Removal Of Label Failed";
                }
                else
                    return "Label Not Found";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch And Delete The Labels Using Label Name And UserId
        public string DeleteLabel(long labelNameId, long userId)
        {
            try
            {
                var labelDetails = fundooContext.LabelsNameData.Where(l => l.LabelNameId == labelNameId && l.UserId == userId).FirstOrDefault();
                if (labelDetails != null)
                {
                    fundooContext.LabelsNameData.Remove(labelDetails);
                    var result = fundooContext.SaveChanges();
                    if (result > 0)
                        return "Label Deleted Succesfully";
                    else
                        return "Label Deletion Failed";
                }
                else
                    return "Label Not Found";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Lables Lists
        public IEnumerable<LabelsResponse> GetNotesLabels(long noteId, long userId)
        {
            try
            {
                IList<LabelsResponse> noteLabelsList = new List<LabelsResponse>();
                var labelsNameList = fundooContext.LabelsNameData.ToList();
                var labelsList = fundooContext.LabelsData.Where(c => c.NoteId == noteId && c.UserId == userId).ToList();
                if(labelsList.Count() > 0)
                {
                    foreach (var label in labelsList)
                    {
                        foreach (var labelName in labelsNameList)
                        {
                            if (labelName.LabelNameId == label.LabelNameId)
                            {
                                LabelsResponse getNoteLabels = new LabelsResponse()
                                {
                                    LabelId = label.LabelId,
                                    LabelNameId = labelName.LabelNameId,
                                    LabelName = labelName.LabelName,
                                    UserId = label.UserId,
                                    NoteId = label.NoteId
                                };
                                noteLabelsList.Add(getNoteLabels);
                            }
                        }
                    }
                    return noteLabelsList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Users Lables Lists
        public IEnumerable<LabelsResponse> GetUsersLabelsList(long userId)
        {
            try
            {
                IList<LabelsResponse> noteLabelsList = new List<LabelsResponse>();
                var labelsNameList = fundooContext.LabelsNameData.ToList();
                var notesList = fundooContext.NotesData.Where(u => u.UserId == userId).ToList();
                var labelsList = fundooContext.LabelsData.Where(l => l.UserId == userId).ToList();
                if(labelsList.Count > 0)
                {
                    foreach (var label in labelsList)
                    {
                        foreach (var labelName in labelsNameList)
                        {
                            if (labelName.LabelNameId == label.LabelNameId)
                            {
                                LabelsResponse getNoteLabels = new LabelsResponse()
                                {
                                    LabelId = label.LabelId,
                                    LabelNameId = labelName.LabelNameId,
                                    LabelName = labelName.LabelName,
                                    UserId = label.UserId,
                                    NoteId = label.NoteId
                                };
                                noteLabelsList.Add(getNoteLabels);
                            }
                        }
                    }
                    return noteLabelsList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method To Fetch The Lables Lists With Notes
        public IEnumerable<LabelNameEntity> GetUsersLabelNamesList(long userId)
        {
            try
            {
                var labelsNameList = fundooContext.LabelsNameData.Where(l => l.UserId == userId).ToList();
                if (labelsNameList.Count() > 0)
                    return labelsNameList;
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
