namespace Forum.App.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Forum.App.Contracts;
    using Forum.App.ViewModels;
    using Forum.Data;
    using Forum.DataModels;

    public class PostService : IPostService
    {
        private ForumData forumData;
        private ISession session;
        private IUserService userService;

        public PostService(ForumData forumData, ISession session, IUserService userService)
        {
            this.forumData = forumData;
            this.session = session;
            this.userService = userService;
        }


        public int AddPost(int userId, string postTitle, string postCategory, string postContent)
        {
            bool emptyTitle = string.IsNullOrWhiteSpace(postTitle);
            bool emptyCategory = string.IsNullOrWhiteSpace(postCategory);
            bool emptyContent = string.IsNullOrWhiteSpace(postContent);

            if (emptyTitle || emptyCategory || emptyContent)
            {
                throw new ArgumentException("All fields must be filled!");
            }

            Category category = this.EnsureCategory(postCategory);

            int postId = this.forumData.Posts.LastOrDefault()?.Id + 1 ?? 1;
            User author = this.userService.GetUserById(userId);

            Post post = new Post(postId, postTitle, postContent, category.Id, author.Id, new List<int>());

            this.forumData.Posts.Add(post);
            author.Posts.Add(postId);
            category.Posts.Add(postId);
            this.forumData.SaveChanges();

            return post.Id;
        }

        private Category EnsureCategory(string categoryName)
        {
            Category category = this.forumData.Categories.FirstOrDefault(c => c.Name == categoryName);

            if (category == null)
            {
                int categoryId = this.forumData.Categories.LastOrDefault()?.Id + 1 ?? 1;
                category = new Category(categoryId, categoryName, new List<int>());
            }

            return category;
        }

        public void AddReplyToPost(int postId, string replyContents, int userId)
        {
            Post post = this.forumData.Posts.Find(p => p.Id == postId);
            User author = this.userService.GetUserById(userId);

            int id = this.forumData.Replies.LastOrDefault()?.Id + 1 ?? 1;
            Reply reply = new Reply(id, replyContents, userId, postId);

            this.forumData.Replies.Add(reply);
            post.Replies.Add(id);

            this.forumData.SaveChanges();
        }

        public IEnumerable<ICategoryInfoViewModel> GetAllCategories()
        {
            IEnumerable<ICategoryInfoViewModel> categories = this.forumData.Categories
                .Select(c => new CategoryInfoViewModel(c.Id, c.Name, c.Posts.Count));

            return categories;
        }

        public string GetCategoryName(int categoryId)
        {
            string categoryName = this.forumData.Categories.First(c => c.Id == categoryId)?.Name;
            if (categoryName == null)
            {
                throw new ArgumentException($"Category with id {categoryId} not found!");
            }

            return categoryName;
        }

        public IEnumerable<IPostInfoViewModel> GetCategoryPostsInfo(int categoryId)
        {
            ICollection<int> postsIds = this.forumData.Categories
                .First(c => c.Id == categoryId)?.Posts;

            if (postsIds == null)
            {
                throw new ArgumentException($"Category with id {categoryId} not found!");
            }

            IEnumerable<IPostInfoViewModel> posts = this.forumData.Posts.Where(p => postsIds.Contains(p.Id))
                .Select(p => new PostInfoViewModel(p.Id, p.Title, p.Replies.Count));

            return posts;
        }

        public IPostViewModel GetPostViewModel(int postId)
        {
            Post post = this.forumData.Posts.First(p => p.Id == postId);
            IPostViewModel postView = new PostViewModel(post.Title, this.userService.GetUserName(post.AuthorId),
                post.Content, this.GetPostReplies(post.Id));

            return postView;
        }

        private IEnumerable<IReplyViewModel> GetPostReplies(int postId)
        {
            IEnumerable<IReplyViewModel> replies = this.forumData.Replies
                .Where(r => r.PostId == postId)
                .Select(r => new ReplyViewModel(this.userService.GetUserName(r.AuthorId), r.Content));

            return replies;
        }
    }
}
