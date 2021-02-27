using DirectorsPortal;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

/// <summary>
/// This file has all of the logic for handling the email page.
/// This page allows the director to send emails to all of the
/// different groups without leaving the portal to use a service
/// like ConstantContact. 
/// </summary>
namespace DirectorsPortalWPF.EmailMembersSendEmailUI
{
    /// <summary>
    /// Interaction logic for EmailMembersSendEmailPage.xaml
    /// </summary>
    public partial class EmailMembersSendEmailPage : Page
    {
        private string gStrAttachedFilePath;

        public EmailMembersSendEmailPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Gets called on the click of the "Send" button on the email page.
        /// Will pull the email list, subject, and body, then send it to the
        /// email service to be sent to the appropriate people.
        /// email list delimited by ;, send array of recipients body and subject to API 
        /// when send is pressed the user wil login with microsoft pop up authenticator
        /// then the emial will be sent from the user
        /// </summary>
        /// <param name="sender">The Send button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private async void SendEmail(object sender, RoutedEventArgs e)
        {
            String strSubject = txtSubject.Text;
            String strRecipient = txtToField.Text;
            String[] rgRecipient = strRecipient.Split(';');

            String strContent = GetRichTextDocumentHtmlContent();

            await GraphApiClient.SendMail(strSubject, rgRecipient, strContent);

            txtToField.Clear();
            txtSubject.Clear();
            rtbEmailBody.Document.Blocks.Clear();

            if (rgRecipient.Length > 1)
                MessageBox.Show($"Message sent to {rgRecipient[0]} and {rgRecipient.Length - 1} others.", "Message Sent");
            else
                MessageBox.Show($"Message sent to {rgRecipient[0]}.", "Message Sent");
        }
        /// <summary>
        /// empty method for use later 
        /// will take folder name or ID and return email objects that can be printed
        /// </summary>
        private async void GetFolders()
        {
            String strFolderName = "";

            await GraphApiClient.GetFolder(strFolderName);
        }

        /// <summary>
        /// empty method for user later
        /// will take folder emial ID and pull back a single email object which can be printed 
        /// </summary>
        private async void GetEmail()
        {
            // TODO: Still needs to be implemented
            // Need API to call from the SDK team
            String strID = "";
            await GraphApiClient.GetEmail(strID);

        }

        /// <summary>
        /// Get the FlowDocument content of the RichTextBox field containing the body of the email and convert
        /// it to an HTML document that can be read by the GraphApiClient
        /// </summary>
        /// <returns>A string containing the HTML for the RichTextField content</returns>
        private string GetRichTextDocumentHtmlContent()
        {
            FlowDocument fdRichTextBoxBody = rtbEmailBody.Document;
            XmlDocument xDocFlowDocToXml = new XmlDocument();

            xDocFlowDocToXml.LoadXml(XamlWriter.Save(fdRichTextBoxBody));

            StringWriter swString = new StringWriter();
            XmlTextWriter xwXml = new XmlTextWriter(swString);
            xDocFlowDocToXml.WriteTo(xwXml);

            return HtmlFromXamlConverter.ConvertXamlToHtml(swString.ToString());
        }

        /// <summary>
        /// Button intended to attach files to an email message. Assigns filepath of the attachment to a global
        /// string.
        /// </summary>
        /// <param name="sender">The 'Attach' button</param>
        /// <param name="e">The Click event</param>
        private void AttachFile_Click(object sender, RoutedEventArgs e)
        {
            gStrAttachedFilePath = OpenFile();
            MessageBox.Show(gStrAttachedFilePath);

        }

        /// <summary>
        /// Allows the user to select a file using a FileDialog window. Gets the selected
        /// file's path.
        /// </summary>
        /// <returns>A file path as a string</returns>
        private string OpenFile()
        {
            string strFilePath = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                strFilePath = openFileDialog.FileName;

            return strFilePath;
        }
    }
}
