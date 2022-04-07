using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class Of CollabListResponse Class To Get List Of Collabs With Multiple Emails
    /// </summary>
    public class CollabListResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FirstName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LastName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? CollabId { get; set; }
        public string CollabEmail { get; set; }
        public long UserId { get; set; }
        public long NoteId { get; set; }

    }
}
