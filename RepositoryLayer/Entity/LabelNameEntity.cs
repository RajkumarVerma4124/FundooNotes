using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
        public string LabelName { get; set; }
    }
}
