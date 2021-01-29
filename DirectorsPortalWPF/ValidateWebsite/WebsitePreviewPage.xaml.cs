using System;
using System.IO;
using System.Windows.Controls;

namespace DirectorsPortalWPF.ValidateWebsite
{
    /// <summary>
    /// Interaction logic for WebsitePreviewPage.xaml
    /// </summary>
    public partial class WebsitePreviewPage : Page
    {
        public WebsitePreviewPage()
        {
            InitializeComponent();

            // Template code
            frmValidateWebpage.Source = new Uri(GetTemplateLocation());
        }

        public string GetTemplateLocation()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var pagesFolder = Directory.GetParent(exePath).Parent.Parent;
            string templateFullPath = pagesFolder.FullName + "\\Resources\\MembershipTemplate.html";
            return templateFullPath;
        }
    }
}
