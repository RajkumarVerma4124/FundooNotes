using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text;

namespace RepositoryLayer.Entity
{
    /// <summary>
    /// Created The Class Of LabelEntity Class To Create Multiple Labels With Multiple Notes
    /// </summary>
    public class LabelsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelId { get; set; }
        public string LabelName { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }

        [JsonIgnore]
        public virtual UserEntity User { get; set; }

        [ForeignKey("Note")]
        public long? NoteId { get; set; }

        [JsonIgnore]
        public virtual NoteEntity Note { get; set; }
    }
}
