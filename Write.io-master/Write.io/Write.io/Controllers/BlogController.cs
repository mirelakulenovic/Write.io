using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Write.io.Models;

namespace Write.io.Controllers
{
    public class BlogController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Blog
        //This uses something called "Attribute Routing" which needs to be enabled in the RouteConfig.cs file
        [Route("b/{Nickname}/{BlogTitle}/")]
        public ActionResult Index(string Nickname, string BlogTitle)
        {
            BlogViewModel model = new BlogViewModel();
            if (model.Populate(Nickname, BlogTitle) == true)
            {
                return View(model);
            }
            else
            {
                throw new HttpException(404, "The blog could not be found.");
            }
        }

        //Overload to support searching
        [Route("b/{Nickname}/{BlogTitle}/S/{Query}")]
        public ActionResult Index(string Nickname, string BlogTitle, string Query)
        {
            BlogViewModel model = new BlogViewModel();
            if (model.Populate(Nickname, BlogTitle, Query) == true)
            {
                return View(model);
            }
            else
            {
                throw new HttpException(404, "The blog could not be found.");
            }
        }

        //Get method for the create a post dialogue
        [Route("b/{Nickname}/{BlogTitle}/CreatePost/"), HttpGet]
        public ActionResult CreatePost()
        {
            var model = new CreatePostViewModel()
            {
                Title = "",
                Body = "",
                Tags = "",
            };
            return View(model);
        }
        //Get for post editing
        [Route("b/{Nickname}/{BlogTitle}/{PostID}-{PostTitle}/Edit/"), HttpGet]
        public ActionResult CreatePost(int PostID, string PostTitle)
        {
            var model = new CreatePostViewModel();
            //If the post exists, adds the content to the model
            if (db.Posts.Any(p => p.Id == PostID))
            {
                var post = db.Posts.Where(p => p.Id == PostID).FirstOrDefault();
                model.Id = PostID;
                model.Title = post.Title;
                model.Body = post.Body;
                var Tags = "";
                foreach (var tag in post.Tags)
                {
                    Tags = Tags + tag.Name + ", ";
                }
                Tags.TrimEnd(' ');
                Tags.TrimEnd(',');
                model.Tags = Tags;
                model.Id = post.Id;
            }

            return View(model);
        }
        //Post method
        [Route("b/{Nickname}/{BlogTitle}/CreatePost"), HttpPost]
        public ActionResult CreatePost(CreatePostViewModel model, string Nickname, string BlogTitle)
        {
            var post = new Post()
            {
                Title = model.Title,
                Body = model.Body
            };
            post.BlogId = db.Blogs.Where(b => b.Title == BlogTitle && b.User.Nickname == Nickname).Select(b => b.Id).SingleOrDefault();
            var BlogUserId = db.Blogs.Where(b => b.Title == BlogTitle && b.User.Nickname == Nickname).Select(b => b.UserId).SingleOrDefault();
            //Checks if the logged on user is the owner of the blog
            if (User.Identity.GetUserId() == BlogUserId)
            {
                ViewBag.Message = Post.CreateOrUpdate(post, model.Tags, model.Id);
            }
            else
            {
                ViewBag.Message = "You can't create a post on a blog you don't own.";
            }

            return RedirectToAction("Index", new { Nickname = Nickname, BlogTitle = BlogTitle });
        }
        //Post method for updating a post
        [Route("b/{Nickname}/{BlogTitle}/{PostID}-{PostTitle}/Edit"), HttpPost]
        public ActionResult CreatePost(CreatePostViewModel model, string Nickname, string BlogTitle, int PostID, string PostTitle)
        {
            var post = new Post()
            {
                Id = PostID,
                Title = model.Title,
                Body = model.Body
            };
            post.BlogId = db.Blogs.Where(b => b.Title == BlogTitle && b.User.Nickname == Nickname).Select(b => b.Id).SingleOrDefault();
            var BlogUserId = db.Blogs.Where(b => b.Title == BlogTitle && b.User.Nickname == Nickname).Select(b => b.UserId).SingleOrDefault();
            //Checks if the logged on user is the owner of the blog
            if (User.Identity.GetUserId() == BlogUserId)
            {
                var UpdatedPost = Post.CreateOrUpdate(post, model.Tags, PostID);
                return RedirectToAction("ViewPost", new { Nickname = Nickname, BlogTitle = BlogTitle, PostID = UpdatedPost.Id, PostTitle = UpdatedPost.Title });

            }
            return RedirectToAction("Index", new { Nickname = Nickname, BlogTitle = BlogTitle });

        }
        //Views individual posts
        [Route("b/{Nickname}/{BlogTitle}/{PostID}-{PostTitle}")]
        public ActionResult ViewPost(string Nickname, string BlogTitle, int PostID, string PostTitle)
        {
            BlogPostViewModel model = new BlogPostViewModel();
            model.Populate(Nickname, BlogTitle, PostID, PostTitle);
            return PartialView("ViewPost", model);
        }

        [HttpPost]
        public JsonResult PostComment(Comment model)
        {
            if (model != null)
            {
                model.UserId = User.Identity.GetUserId();
                model.created = DateTime.Now;
                db.Comments.Add(model);
                db.SaveChanges();
                return Json("Your comment has been created.");
            }
            else
            {
                return Json("An error has occured. Dispatching codemonkeys.");
            }
        }
    }
}