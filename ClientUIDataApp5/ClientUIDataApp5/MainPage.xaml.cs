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
using Intersoft.Client.Framework.Input;
using Intersoft.Client.Framework;
using Intersoft.Client.UI.Controls;
using System.Windows.Browser;

namespace ClientUIDataApp5
{
    public partial class MainPage : UXPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.ContentFrame.Navigated += new NavigationEventHandler(ContentFrame_Navigated);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        // set the browser's page title using branded format string
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            string title = string.Empty;
            object content = e.Content;

            if (content == null)
                content = ContentFrame.Content;

            if (content is Page)
                title = ((Page)content).Title;

            if (CrossPlatform.IsHtmlPageEnabled())
                CrossPlatform.SetDocumentTitle("Intersoft ClientUI Application | " + title);
        }
    }
}
