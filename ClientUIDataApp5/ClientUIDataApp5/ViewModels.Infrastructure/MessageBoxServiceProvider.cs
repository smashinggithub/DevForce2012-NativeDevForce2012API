using System;
using System.Windows;
using Intersoft.Client.Framework;
using Intersoft.Client.Framework.Input;
using Intersoft.Client.UI.Aqua.UXDesktop;
using UXDesktop = Intersoft.Client.UI.Aqua.UXDesktop;

namespace ClientUIDataApp5
{
    public class MessageBoxServiceProvider
    {
        internal static IWindow Owner
        {
            get
            {
                FrameworkElement element = ISFocusManager.GetFocusedElement() as FrameworkElement;
                IWindow owner = null;

                // check if the caller is hosted within a known container
                // if the dependency container is of IWindow type, use it as the owner of this modal window
                if (element != null)
                {
                    DependencyObject logicalContainer = ISFocusManager.GetLogicalContainerScope(element);

                    if (logicalContainer is IWindow)
                        owner = logicalContainer as IWindow;
                }

                return owner;
            }
        }

        internal static void Show(string message, Action<DialogResult> resultCallback)
        {
            UXMessageBox.Show(Owner, message, resultCallback);
        }

        internal static void Show(string message, string caption, Action<DialogResult> resultCallback)
        {
            UXMessageBox.Show(Owner, message, caption, resultCallback);
        }

        internal static void Show(string message, string caption, Intersoft.Client.UI.Aqua.UXDesktop.MessageBoxButton button, Action<DialogResult> resultCallback)
        {
            UXMessageBox.Show(Owner, message, caption, button, resultCallback);
        }

        internal static void Show(string message, string caption, Intersoft.Client.UI.Aqua.UXDesktop.MessageBoxButton button, UXDesktop.MessageBoxImage image, Action<DialogResult> resultCallback)
        {
            UXMessageBox.Show(Owner, message, caption, button, image, resultCallback);
        }
    }
}
