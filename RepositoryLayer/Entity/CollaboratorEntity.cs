using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    /// <summary>
    /// Created The Class To CollaboratorEntity Class To Create Collabd With Multiple Emails
    /// </summary>
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollabId { get; set; }
        public string CollabEmail { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        [JsonIgnore]
        public virtual UserEntity User { get; set; }

        [ForeignKey("Note")]
        public long NoteId { get; set; }

        [JsonIgnore]
        public virtual NoteEntity Note { get; set; }
        
    }
}
