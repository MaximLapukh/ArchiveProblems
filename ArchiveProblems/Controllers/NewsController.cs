using ArchiveProblems.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Controllers
{
    public class NewsController:Controller
    {
        private readonly ProblemsContext _db;

        public NewsController(ProblemsContext db)
        {
            _db = db;
        }
        public IActionResult All()
        {                    
            return View(_db.news.ToList());           
        }
        public IActionResult News(int? id) {
            if (id != null)
            {
                ViewBag.viewnews = "one";
                var curNews = _db.news.FirstOrDefault(n => n.Id == id);
                if (curNews != null) return View(curNews);
                else return RedirectToAction("All");
            }
            return RedirectToAction("All");
        }
    }
}
