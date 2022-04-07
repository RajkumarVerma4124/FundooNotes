using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    /// <summary>
    /// Created The Class Of LabelNameEntity Class To Create Multiple Labels With Multiple Notes
    /// </summary>
    public class LabelNameEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelNameId { get; set; }
        [JsonIgnore]
        public long UserId { get; set; }
        public string LabelName { get; set; }
    }
}
