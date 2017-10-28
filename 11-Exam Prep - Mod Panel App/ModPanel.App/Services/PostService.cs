namespace ModPanel.App.Services
{
    using Data;
    using Data.Models;
    using Models.Home;
    using Models.Posts;
    using Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostService : IPostService
    {
        public void Create(string title, string content, int userId)
        {
            using (var context = new ModPanelDbContext())
            {
                var post = new Post
                {
                    Title = title,
                    Content = content,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow
                };

                context.Posts.Add(post);
                context.SaveChanges();
            }
        }

        public IEnumerable<PostListingModel> All()
        {
            using (var context = new ModPanelDbContext())
            {
                return context
                    .Posts
                    .Select(p => new PostListingModel
                    {
                        Id = p.Id,
                        Title = p.Title
                    })
                    .ToList();
            }
        }

        public IEnumerable<HomeListingModel> AllWithDetails(string search = null)
        {
            using (var context = new ModPanelDbContext())
            {
                var query = context.Posts.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Title.ToLower()
                                            .Contains(search.ToLower()));
                }

                return query
                    .OrderByDescending(p => p.Id)
                    .Select(p => new HomeListingModel
                    {
                        Title = p.Title,
                        Content = p.Content,
                        CreatedBy = p.User.Email,
                        CreatedOn = p.CreatedOn
                    })
                    .ToList();
            }
        }

        public PostModel GetById(int id)
        {
            using (var context = new ModPanelDbContext())
            {
                return context
                    .Posts
                    .Where(p => p.Id == id)
                    .Select(p => new PostModel
                    {
                        Title = p.Title,
                        Content = p.Content
                    })
                    .FirstOrDefault();
            }
        }

        public bool Delete(int id)
        {
            using (var context = new ModPanelDbContext())
            {
                // Find post
                var post = context.Posts.Find(id);
                if (post == null)
                {
                    return false;
                }

                // Remove post
                context.Remove(post);
                context.SaveChanges();

                return true;
            }
        }

        public bool Update(int id, string title, string content)
        {
            using (var context = new ModPanelDbContext())
            {
                // Find post
                var post = context.Posts.Find(id);
                if (post == null)
                {
                    return false;
                }

                // Update post
                post.Title = title;
                post.Content = content;

                context.Update(post);
                context.SaveChanges();

                return true;
            }
        }
    }
}
