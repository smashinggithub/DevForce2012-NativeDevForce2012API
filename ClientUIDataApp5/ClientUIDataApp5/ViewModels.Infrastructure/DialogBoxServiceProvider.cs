using System;
using System.Windows;
using Intersoft.Client.Framework;
using Intersoft.Client.Framework.Input;

namespace ClientUIDataApp5.ViewModels
{
    public class DialogBoxServiceProvider
    {
        public static IModalWindow GetInstance(string viewType)
        {
            return UXShell.Current.GetApplication(App.ApplicationID).CreateInstance(viewType) as IModalWindow;
        }

        public static void GetInstanceAsync(string viewType, Action<IModalWindow> resultCallback)
        {
            UXShell.Current.GetApplication(App.ApplicationID).CreateInstanceAsync((result) =>
                    {
                        InstanceLoaderAsyncResult asyncResult = (InstanceLoaderAsyncResult)result;

                        if (asyncResult.IsCompleted)
                        {
                            if (asyncResult.Error == null)
                            {
                                resultCallback((IModalWindow)asyncResult.Instance);
                            }
                            else
                            {
                                throw new TypeLoadException("Unable to create the specified type", asyncResult.Error);
                            }
                        }
                    }
                    , viewType, null);
        }

        public static void Show(IModalWindow modalWindow, Action<DialogResult> resultCallback)
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

            modalWindow.ShowDialog(owner, resultCallback);
        }
    }
}
