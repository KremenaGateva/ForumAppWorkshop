namespace Forum.App.Commands
{
    using System;
    using Forum.App.Contracts;

    public class LogInCommand : ICommand
    {
        private ISession session;
        private IMenuFactory menuFactory;
        private IUserService userService;

        public LogInCommand(ISession session, IMenuFactory menuFactory, IUserService userService)
        {
            this.session = session;
            this.menuFactory = menuFactory;
            this.userService = userService;
        }

        public IMenu Execute(params string[] args)
        {
            string username = args[0];
            string password = args[1];

            bool canLogIn = this.userService.TryLogInUser(username, password);
            if (!canLogIn)
            {
                throw new ArgumentException("Invalid login!");
            }

            IMenu menu = this.menuFactory.CreateMenu("MainMenu");
            return menu;
        }
    }
}
