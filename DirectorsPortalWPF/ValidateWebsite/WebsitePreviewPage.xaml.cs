using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 
/// File Name: WebsitePreviewPage.xaml.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Benjamin J. Dore
/// 
/// File Purpose:
///     This file is responsible for displaying a preview of the Membership page that will be 
///     uploaded to the Chesaning Chamber of Commerce website. Responsibilities of the file include:
///         - Generating the HTML preview
///         - Refreshing the HTML preview
///         - Copying HTML content to the Clipboard
///         
/// </summary>
namespace DirectorsPortalWPF.ValidateWebsite
{
    /// <summary>
    /// Interaction logic for WebsitePreviewPage.xaml
    /// </summary>
    public partial class WebsitePreviewPage : Page
    {
        // Preview Generator for the HTML template, responsible for loading the 
        // HTML template with latest membership data
        private readonly HtmlPreviewGenerator GHpgPreview;

        /// <summary>
        /// Initialization of the Validate Webpage screen. Preview of the HTML content is generated with latest
        /// data from the database and is displayed on screen.
        /// </summary>
        public WebsitePreviewPage()
        {
            InitializeComponent();

            GHpgPreview = new HtmlPreviewGenerator();
            GHpgPreview.GeneratePreview();

            // Template code
            frmValidateWebpage.Source = new Uri(GHpgPreview.GetTemplateLocation());
        }

        /// <summary>
        /// When the 'Copy to Clipboard' button is selected. The HTML template is read and all HTML 
        /// content is copied to the clipboard. From there the user can paste the HTML into Weebly using an
        /// 'Embed Code' object.
        /// </summary>
        /// <param name="sender">The 'Copy to Clipboard' button</param>
        /// <param name="e">The Click Event</param>
        private void BtnCopyContent_Click(object sender, RoutedEventArgs e)
        {
            // StreamReader for reading the HTML File
            using (StreamReader strReader = new StreamReader(GHpgPreview.GetTemplateLocation()))
            {
                string strHTML = strReader.ReadToEnd();

                Console.WriteLine(strReader.ReadToEnd());
                Clipboard.SetText(strHTML);             // Save HTML content to Clipboard

            }

            // Alert the user! Let them know the copy has been completed.
            MessageBox.Show("Updated Webpage Content Copied to Clipboard!",
                "Copied to Clipboard",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// When the 'Refresh Preview' button is selected. The HTML preview is refreshed with the latest data
        /// from the database.
        /// </summary>
        /// <param name="sender">The 'Refresh Preview' button</param>
        /// <param name="e">The Click Event</param>
        private void BtnRefreshPreview_Click(object sender, RoutedEventArgs e)
        {
            GHpgPreview.GeneratePreview();
            frmValidateWebpage.Source = new Uri(GHpgPreview.GetTemplateLocation());
            frmValidateWebpage.Refresh();
        }

        /// <summary>
        /// When the 'Preview in Web Browser' button is selected. The HTML preview will open in the
        /// default browser set by the operating system. This is useful since the Frame built into the app cannot render all
        /// HTML styling and some JavasScript. The Web Browser preview is a true display of content that is viewed.
        /// </summary>
        /// <param name="sender">The 'Preview in Web Browser' button</param>
        /// <param name="e">The Click Event</param>
        private void BtnViewInWeb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(GHpgPreview.GetTemplateLocation());        // Open preview in default web browser...
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);                         // In case no web browser is installed...
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);                                 // Any other error...
            }           
        }
    }
}
