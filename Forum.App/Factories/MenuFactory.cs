namespace Forum.App.Factories
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Forum.App.Contracts;
    using Forum.App.Menus;

    public class MenuFactory : IMenuFactory
    {
        private IServiceProvider serviceProvider;

        public MenuFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IMenu CreateMenu(string menuName)
        {
            Type menuType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(m => m.Name == menuName);

            if (menuType == null)
            {
                throw new InvalidOperationException("Menu not found!");
            }
            if (!typeof(IMenu).IsAssignableFrom(menuType))
            {
                throw new InvalidOperationException($"{menuType} is not a menu!");
            }

            ParameterInfo[] ctorParameters = menuType.GetConstructors().First().GetParameters();
            object[] args = new object[ctorParameters.Length];

            for (int i = 0; i < ctorParameters.Length; i++)
            {
                args[i] = this.serviceProvider.GetService(ctorParameters[i].ParameterType);
            }

            IMenu menu = (IMenu)Activator.CreateInstance(menuType, args);

            return menu;
        }
    }
}
