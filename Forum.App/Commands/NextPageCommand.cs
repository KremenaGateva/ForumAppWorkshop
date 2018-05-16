namespace Forum.App.Commands
{
    using Forum.App.Contracts;

    public class NextPageCommand : ICommand
    {
        private ISession session;

        public NextPageCommand(ISession session)
        {
            this.session = session;
        }

        public IMenu Execute(params string[] args)
        {
            IPaginatedMenu menu =(IPaginatedMenu)this.session.CurrentMenu;

            menu.ChangePage(true);

            return menu;
        }
    }
}
