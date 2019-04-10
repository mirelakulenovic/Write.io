using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Write.io.Models;

namespace Write.io.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var model = db.Blogs.Select(b => b).ToList();
            return View(model);
        }

        public ActionResult Search(string Query)
        {
            var SearchQuery = Query.Split(' ').ToList();
            List<Blog> model = new List<Blog>();
            foreach (var q in SearchQuery)
            {
                model.AddRange(db.Blogs.Where(b => b.Title.Contains(q) || b.Body.Contains(q) || b.User.FirstName.Contains(q) || b.User.LastName.Contains(q) || b.User.Email.Contains(q)).Select(b => b).ToList());
            }
            return PartialView("_BlogGridPartial", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}