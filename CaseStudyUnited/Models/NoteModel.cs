using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseStudyUnited.Models
{
    public class NoteModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public int ParentId { get; set; }
    }
}