using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Write.io.Models;
using Microsoft.AspNet.Identity;



namespace Write.io.Controllers
{

    [Authorize]
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        

        [Route("MyBlogs")]
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();

            var model = db.Blogs.Where(b => b.UserId.Equals(user)).ToList();
            

            return View(model);
        }


        [HttpGet]
        [Route("AddBlog")]
        public ActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        [Route("AddBlog")]
        public ActionResult AddBlog(Blog obj)
        {
            if (ModelState.IsValid)
            {
                obj.UserId = User.Identity.GetUserId(); ;
                obj.Created = DateTime.Now;

                db.Blogs.Add(obj);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public ActionResult RemoveBlog(int id)
        {
            Blog obj = db.Blogs.Where(b => b.Id.Equals(id)).SingleOrDefault();

            db.Blogs.Remove(obj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("EditBlog/{Id}")]
        public ActionResult EditBlog(int id)
        {
            Blog obj = db.Blogs.Where(b => b.Id.Equals(id)).SingleOrDefault();

            return View(obj);
        }

        [HttpPost]
        [Route("EditBlog/{Id}")]
        public ActionResult EditBlog(Blog obj, string Title)
        {
            var blog = db.Blogs.Where(b => b.Id.Equals(obj.Id)).SingleOrDefault();

            if (ModelState.IsValid)
            {
                blog.Title = obj.Title;
                blog.BlogHeaderURL = obj.BlogHeaderURL;
                blog.Body = obj.Body;
                blog.Template = obj.Template;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}