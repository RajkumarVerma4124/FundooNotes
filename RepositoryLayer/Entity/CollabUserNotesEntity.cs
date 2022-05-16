using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    /// <summary>
    /// Creating The Model class For Other Collaboarated Users Notes Operations
    /// </summary>
    public class CollabUserNotesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        [JsonIgnore]
        public virtual UserEntity User { get; set; }

        [ForeignKey("Collab")]
        public long CollabId { get; set; }
        [JsonIgnore]
        public virtual CollaboratorEntity Collab { get; set; }

        [DefaultValue("false")]
        public bool IsPinned { get; set; }
        [DefaultValue("false")]
        public bool IsArchive { get; set; }
        [DefaultValue("white")]
        public string NoteColor { get; set; }
        [DefaultValue("2022-05-20 12:12:55.389Z")]
        public DateTime? Reminder { get; set; }
    }
}
