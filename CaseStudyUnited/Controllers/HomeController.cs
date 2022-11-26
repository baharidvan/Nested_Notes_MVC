using CaseStudyUnited.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CaseStudyUnited.Controllers
{
    public class HomeController : Controller
    {
        DataContext dbcontext= new DataContext();
        public ActionResult Index()
        {
            var notes = dbcontext.Notes.ToList();
            List<Note> noteList = new List<Note>();
            foreach (var note in notes)
            {
                Note _note = new Note()
                {
                    Id = note.Id,
                    ParentId = note.ParentId,
                    Content = note.Content,
                    UserName = note.UserName,
                    Date = note.Date,
                    SubNote = notes.Where(x => x.ParentId == note.Id).OrderBy(x => x.Date).ToList()
                };
                CreateNote(_note);
                noteList.Add(_note);
            }
            var resultList = SortNotes(noteList);

            return View(resultList);
        }
        [HttpPost]
        public ActionResult AddNote(Note _note, int id)
        {
            var note = new Note()
            {
                Content = _note.Content,
                UserName = "user1",
                Date = DateTime.Now,
                ParentId = id,
                Level = 0
            };
            note.Level = SetLevel(note);

            dbcontext.Notes.Add(note);
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteNote(int id)
        {
            var note = dbcontext.Notes.Find(id);
            note.SubNote = dbcontext.Notes.Where(x => x.ParentId == note.Id).OrderBy(x => x.Date).ToList();
            CreateNote(note);
            var list = GetSubNotes(note);
            list.Add(note);
            foreach (var item in list)
            {
                dbcontext.Notes.Remove(item);
            }
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }
        public void CreateNote (Note sourceNote)
        {
            foreach (var item in sourceNote.SubNote)
            {
                item.SubNote = dbcontext.Notes.Where(x => x.ParentId == item.Id).OrderBy(x => x.Date).ToList();
                CreateNote(item);
            }
        }

        public List<Note> SortNotes(List<Note> noteList)
        {
            List<Note> returnList = new List<Note>();

            foreach (var item in noteList)
            {
                if (!returnList.Any(x => x.Id == item.Id)) //Aynı notu listede birden fazla traverse etmemek için
                {
                    returnList.Add(item);
                    if ((item.SubNote != null) && (item.SubNote.Any()) && (item.SubNote.Count != 0)) //Eğer ki alt notu varsa
                    {
                        returnList.AddRange(SortNotes(item.SubNote.ToList())); //Recursive olarak hiyerarşik bir şekilde alt notları listeye ekle
                    }
                }
            }
            return returnList;
        }

        public int SetLevel(Note note)
        {
            var parentNote = dbcontext.Notes.Where(x => x.Id == note.ParentId).SingleOrDefault();
            if (parentNote != null)
                return SetLevel(parentNote) + 1;
            else
                return 0;
        }

        public List<Note> GetSubNotes(Note note)
        {
            var returnSubNotes = new List<Note>();
            foreach (var item in note.SubNote)
            {
                returnSubNotes.Add(item);
                if (item.SubNote.Count > 0)
                {
                    returnSubNotes.AddRange(GetSubNotes(item));
                }
            }
            return returnSubNotes;
        }
    }
}