using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseStudyUnited.Entity
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
        public List<Note> SubNote { get; set; }
    }
}