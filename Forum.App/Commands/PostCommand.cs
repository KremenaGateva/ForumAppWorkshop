namespace Forum.App.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Forum.App.Contracts;

    public class PostCommand : ICommand
    {
        private ISession session;
        private IPostService postService;
        private ICommandFactory commandFactory;

        public PostCommand(ISession session, IPostService postService, ICommandFactory commandFactory)
        {
            this.session = session;
            this.postService = postService;
            this.commandFactory = commandFactory;
        }

        public IMenu Execute(params string[] args)
        {
            int userId = this.session.UserId;

            string postTitle = args[0];
            string postCategory = args[1];
            string postContent = args[2];

            int postId = this.postService.AddPost(userId, postTitle, postCategory, postContent);

            ICommand command = this.commandFactory.CreateCommand("ViewPostMenu");
            IMenu menu = command.Execute(postId.ToString());

            return menu;
        }
    }
}
