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
        private readonly ModPanelDbContext context;

        public PostService(ModPanelDbContext context)
        {
            this.context = context;
        }

        public void Create(string title, string content, int userId)
        {
            var post = new Post
            {
                Title = title,
                Content = content,
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            this.context.Posts.Add(post);
            this.context.SaveChanges();
        }

        public IEnumerable<PostListingModel> All()
        {
            return this.context
                .Posts
                .Select(p => new PostListingModel
                {
                    Id = p.Id,
                    Title = p.Title
                })
                .ToList();
        }

        public IEnumerable<HomeListingModel> AllWithDetails(string search = null)
        {
            var query = this.context.Posts.AsQueryable();

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

        public PostModel GetById(int id)
        {
            return this.context
                .Posts
                .Where(p => p.Id == id)
                .Select(p => new PostModel
                {
                    Title = p.Title,
                    Content = p.Content
                })
                .FirstOrDefault();
        }

        public bool Delete(int id)
        {
            // Find post
            var post = this.context.Posts.Find(id);
            if (post == null)
            {
                return false;
            }

            // Remove post
            this.context.Remove(post);
            this.context.SaveChanges();

            return true;
        }

        public bool Update(int id, string title, string content)
        {
            // Find post
            var post = this.context.Posts.Find(id);
            if (post == null)
            {
                return false;
            }

            // Update post
            post.Title = title;
            post.Content = content;

            this.context.Update(post);
            this.context.SaveChanges();

            return true;
        }
    }
}
