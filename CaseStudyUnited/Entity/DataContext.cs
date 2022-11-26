using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CaseStudyUnited.Entity
{
    public class DataContext : DbContext
    {
        public DataContext(): base("dataConnection") 
        {

        }

        public DbSet<Note> Notes { get; set; }
    }
}