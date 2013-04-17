using System;
using System.Windows;
using Intersoft.Client.Framework;
using Intersoft.Client.UI.Navigation;
using ClientUIDataApp5.ModelServices;

namespace ClientUIDataApp5
{
    public partial class App : Application
    {
        // define the ApplicationID used to reference this application from UXShell or other containers
        public static string ApplicationID = "ClientUIDataApp5";

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
            InitializeShell();

            // Load DevForce assemblies at startup to speed up performance at page level
            RepositoryManager.Create();
        }

        private void InitializeShell()
        {
            UXShell shell = new UXShell();
            shell.RootApplication = UXShell.CreateApplicationFromType(typeof(App), ApplicationID, ApplicationID);
            this.ApplicationLifetimeObjects.Add(shell);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                e.Handled = true;

                // Displays a user-friendly error page to the primary navigation (UXFrame) of the application
                Deployment.Current.Dispatcher.BeginInvoke(delegate { DisplayErrorToNavigationFrame(e); });
            }
        }

        private void DisplayErrorToNavigationFrame(ApplicationUnhandledExceptionEventArgs e)
        {
            UXFrame frame = UXFrame.GetPrimaryNavigator(this.RootVisual);

            if (frame != null)
            {
                frame.Navigate(new Uri("/Error", UriKind.Relative), e);
            }
            else
            {
                ReportErrorToDOM(e);
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
