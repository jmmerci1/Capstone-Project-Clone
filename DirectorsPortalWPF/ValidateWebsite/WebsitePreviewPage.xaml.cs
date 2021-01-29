using System;
using System.IO;
using System.Reflection;
using System.Windows;
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

        private void BtnCopyContent_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader reader = new StreamReader(GetTemplateLocation()))
            {
                string strHTML = reader.ReadToEnd();

                Console.WriteLine(reader.ReadToEnd());
                Clipboard.SetText(strHTML);

            }

            MessageBox.Show("Updated Webpage Content Copied to Clipboard!",
                "Copied to Clipboard",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
