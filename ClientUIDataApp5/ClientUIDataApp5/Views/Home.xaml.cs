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
    public partial class Home : UXPage
    {
        public Home()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void UXHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            // Uncomment the following code to try friendly error page
            // Note: Start the project without debugging (Ctrl+F5) to see the error page.

            // throw new Exception("Not Implemented");
        }
    }
}
