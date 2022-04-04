using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class ImageEntity
    {
        public long Id { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        [ForeignKey("Note")]
        public long NoteId { get; set; }

        [JsonIgnore]
        public virtual NoteEntity Note { get; set; }
    }
}
