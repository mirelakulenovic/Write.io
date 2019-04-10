using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Write.io.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BlogHeaderURL { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public string Template { get; set; }
        public virtual ApplicationUser User {get; set; }
        public virtual ICollection<Post> Posts {get; set; }
    }
    public class BlogViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUser User { get; set; }
        public Blog Blog { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<int> PostArchive {get; set; }

        public bool Populate(string Nickname, string BlogTitle, string Query = null)
        {
            if (db.Blogs.Any(b => b.Title == BlogTitle) && db.Users.Any(u => u.Nickname == Nickname))
            {
                this.Blog = db.Blogs.Where(b => b.User.Nickname == Nickname && b.Title == BlogTitle).Select(b => b).FirstOrDefault();
                if (Query == null)
                {
                    this.Posts = db.Posts.Where(p => p.BlogId == this.Blog.Id).OrderByDescending(p => p.Created).Select(p => p).ToList();
                } else
                {
                    int Year = 0;
                    Int32.TryParse(Query, out Year);
                    var SearchQuery = Query.Split(' ').ToList();
                    foreach (var q in SearchQuery)
                    {
                        this.Posts.AddRange(db.Posts.Where(p => p.Title.Contains(q) || p.Body.Contains(q) || p.Tags.Any(t => t.Name.Contains(q)) || p.Created.Year == Year && p.BlogId == this.Blog.Id).Select(p => p).ToList());
                    }
                }                
                this.User = this.Blog.User;
                this.PostArchive = db.Posts.Where (p => p.BlogId == this.Blog.Id).DistinctBy(p => p.Created.Year).Select(p => p.Created.Year).ToList();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class BlogPostViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUser User { get; set; }
        public Blog Blog { get; set; }
        public PostViewModel PostViewModel { get; set; } = new PostViewModel();
        public List<int> PostArchive { get; set; }

        //Populates the viewmodel based on input parameters.
        //Returns true if model is successfully populated
        //If the post couldn't be found, the function will return false. If the blog couldn't be found, the function will also return false.
        public bool Populate(string Nickname, string BlogTitle, int PostID, string PostTitle)
        {
            if (db.Blogs.Any(b => b.Title == BlogTitle) && db.Users.Any(u => u.Nickname == Nickname))
            {
                this.Blog = db.Blogs.Where(b => b.User.Nickname == Nickname && b.Title == BlogTitle).Select(b => b).FirstOrDefault();
                this.User = this.Blog.User;
                this.PostArchive = db.Posts.Where(p => p.BlogId == this.Blog.Id).DistinctBy(p => p.Created.Year).Select(p => p.Created.Year).ToList();
                if (db.Posts.Any(p => p.Title == PostTitle && p.Id == PostID && p.BlogId == this.Blog.Id))
                {
                    this.PostViewModel.Post = db.Posts.Where(p => p.Id == PostID && p.Title == PostTitle && p.BlogId == this.Blog.Id).Select(p => p).FirstOrDefault();
                    //Increments views by one
                    this.PostViewModel.Post.Views++;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}