namespace Forum.App.Commands
{
    using Forum.App.Contracts;

    public class ViewPostMenuCommand : ICommand
    { 
        private IMenuFactory menuFactory;

        public ViewPostMenuCommand(IMenuFactory menuFactory)
        {
            this.menuFactory = menuFactory;
        }

        public IMenu Execute(params string[] args)
        {
            string commandName = this.GetType().Name;
            string menuName = commandName.Substring(0, commandName.Length - "Command".Length);

            IMenu menu = this.menuFactory.CreateMenu(menuName);

            IIdHoldingMenu idHoldingMenu = (IIdHoldingMenu)menu;
            idHoldingMenu.SetId(int.Parse(args[0]));

            return menu;
        }
    }
}
