using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    /// <summary>
    /// Created The Context Class For Migration Of DIfferent Tables Into Db
    /// </summary>
    public class FundooContext: DbContext
    {
        /// Initializes the constructor for fundoocontext class
        public FundooContext(DbContextOptions options) : base(options)
        {
        }

        /// Property For UserData Table To Create Table Using UserEntity Properties
        public DbSet<UserEntity> UserData { get; set; }
        public DbSet<NoteEntity> NotesData { get; set; }
        public DbSet<ImageEntity> ImagesData { get; set; }
        public DbSet<CollaboratorEntity> CollaboratorData { get; set; }
        public DbSet<LabelsEntity> LabelsData { get; set; }
    }
}
