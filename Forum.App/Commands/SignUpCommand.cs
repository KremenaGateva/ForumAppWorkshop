namespace Forum.App.Commands
{
    using System;
    using Forum.App.Contracts;

    public class SignUpCommand : ICommand
    {
        private IUserService userService;
        private IMenuFactory menuFactory;

        public SignUpCommand(IUserService userService, IMenuFactory menuFactory)
        {
            this.userService = userService;
            this.menuFactory = menuFactory;
        }

        public IMenu Execute(params string[] args)
        {
            string username = args[0];
            string password = args[1];

            bool canSignUp = this.userService.TrySignUpUser(username, password);

            if (!canSignUp)
            {
                throw new InvalidOperationException("Invalid singup!");
            }

            IMenu menu = this.menuFactory.CreateMenu("MainMenu");

            return menu;
        }
    }
}
