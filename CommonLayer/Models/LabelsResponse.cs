using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class Of LabelEntity Class To Create Multiple Labels With Multiple Notes
    /// </summary>
    public class LabelsResponse
    {
        public long LabelId { get; set; }
        public long LabelNameId { get; set; }
        public string LabelName { get; set; }
        public long UserId { get; set; }
        public long NoteId { get; set; }
    }
}
