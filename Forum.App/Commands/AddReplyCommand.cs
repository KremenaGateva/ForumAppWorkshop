﻿namespace Forum.App.Commands
{
    using Forum.App.Contracts;

    public class AddReplyCommand : ICommand
    {
        private IMenuFactory menuFactory;

        public AddReplyCommand(IMenuFactory menuFactory)
        {
            this.menuFactory = menuFactory;
        }


        public IMenu Execute(params string[] args)
        {
            string commandName = this.GetType().Name;
            string menuName = commandName.Substring(0, commandName.Length - "Command".Length) + "Menu";

            IMenu menu = this.menuFactory.CreateMenu(menuName);

            IIdHoldingMenu idHoldingMenu = (IIdHoldingMenu)menu;
            idHoldingMenu.SetId(int.Parse(args[0]));

            return menu;
        }
    }
}
