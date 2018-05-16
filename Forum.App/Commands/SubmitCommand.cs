namespace Forum.App.Commands
{
    using System;
    using Forum.App.Contracts;

    public class SubmitCommand : ICommand
    {
        private ISession session;
        private IPostService postService;
        
        public SubmitCommand(ISession session, IPostService postService)
        {
            this.session = session;
            this.postService = postService;
        }

        public IMenu Execute(params string[] args)
        {
            string replyText = args[0];
            int postId = int.Parse(args[1]);
            int userId = this.session.UserId;

            if (string.IsNullOrWhiteSpace(replyText))
            {
                throw new ArgumentException("Cannot add empty reply!");
            }

            this.postService.AddReplyToPost(postId, replyText, userId);

            return this.session.Back();
        }
    }
}
