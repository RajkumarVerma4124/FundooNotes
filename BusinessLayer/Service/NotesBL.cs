﻿using BusinessLayer.Interface;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Created The Notes Business Layer Class To Implement INotesBL Methods
    /// </summary>
    public class NotesBL : INotesBL
    {
        //Reference Object For Interface IUserRL
        private readonly INotesRL notesRL;

        //Created Constructor With Dependency Injection For IUSerRL
        public NotesBL(INotesRL notesRL)
        {
            this.notesRL = notesRL;
        }

        //Method To Return Created Notes Data
        public NoteEntity CreateNote(UserNotes userNotes, long userId)
        {
            try
            {
                return notesRL.CreateNote(userNotes, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<GetNotes> GetAllNotes()
        {
            try
            {
                return notesRL.GetAllNotes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<GetNotes> GetAllNotesByUserId(long userId)
        {
            try
            {
                return notesRL.GetAllNotesByUserId(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetNotes GetNote(long noteId, long userId)
        {
            try
            {
                return notesRL.GetNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity UpdateNote(NoteUpdate noteUpdate, long noteId, long userId)
        {
            try
            {
                return notesRL.UpdateNote(noteUpdate, noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteNote(long noteId, long userId)
        {
            try
            {
                return notesRL.DeleteNote(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity CheckIsArchieveOrNot(long noteId, long userId)
        {
            try
            {
                return notesRL.CheckIsArchieveOrNot(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity CheckIsPinnedOrNot(long noteId, long userId)
        {
            try
            {
                return notesRL.CheckIsPinnedOrNot(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity CheckIsTrashOrNot(long noteId, long userId)
        {
            try
            {
                return notesRL.CheckIsTrashOrNot(noteId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
