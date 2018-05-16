namespace Forum.App.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Forum.App.Contracts;

    public class PreviousPageCommand : ICommand
    {
        private ISession session;

        public PreviousPageCommand(ISession session)
        {
            this.session = session;
        }

        public IMenu Execute(params string[] args)
        {

            IPaginatedMenu menu = (IPaginatedMenu)this.session.CurrentMenu;

            menu.ChangePage(false);

            return menu;
        }
    }
}
