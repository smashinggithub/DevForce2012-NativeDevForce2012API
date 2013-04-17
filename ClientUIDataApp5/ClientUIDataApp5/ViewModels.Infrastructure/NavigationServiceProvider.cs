using System;
using System.Windows;
using Intersoft.Client.Framework;
using Intersoft.Client.Framework.Input;

namespace ClientUIDataApp5.ViewModels
{
    public class NavigationServiceProvider
    {
        public static void Navigate(Uri uri)
        {
            NavigationCommands.Navigate.Execute(uri, (UIElement)ISFocusManager.GetFocusedElement());
        }

        public static void Navigate(Uri uri, object targetElement)
        {
            NavigationCommands.Navigate.Execute(uri, (UIElement)targetElement);
        }
    }
}
