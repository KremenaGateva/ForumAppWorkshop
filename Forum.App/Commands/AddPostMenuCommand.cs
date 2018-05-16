namespace Forum.App.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Forum.App.Contracts;

    public class AddPostMenuCommand : ICommand
    {
        private IMenuFactory menuFactory;

        public AddPostMenuCommand(IMenuFactory menuFactory)
        {
            this.menuFactory = menuFactory;
        }

        public IMenu Execute(params string[] args)
        {
            string commandName = this.GetType().Name;
            string menuName = commandName.Substring(0, commandName.Length - "Command".Length);
            IMenu menu = this.menuFactory.CreateMenu(menuName);

            return menu;
        }
    }
}
