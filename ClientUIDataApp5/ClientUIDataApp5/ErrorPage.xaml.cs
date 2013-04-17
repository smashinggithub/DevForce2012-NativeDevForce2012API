using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Intersoft.Client.UI.Navigation;

namespace ClientUIDataApp5
{
    public partial class ErrorPage : UXPage
    {
        public ErrorPage()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.ExtraData == null)
            {
                // user inadvertently landed to the error page, redirect to the home page
                this.GetNavigator().Navigate(new Uri("/Home", UriKind.Relative));
                return;
            }

            ApplicationUnhandledExceptionEventArgs errorArgs = e.ExtraData as ApplicationUnhandledExceptionEventArgs;
            errorDetails.Text = errorArgs.ExceptionObject.Message + "\n" + errorArgs.ExceptionObject.StackTrace;
        }

        private void UXButton_Click(object sender, RoutedEventArgs e)
        {
            // todo: send the error details through web service or email.

            // transition the content
            contentTransition.SetContent(thanksPanel);
        }
    }
}
