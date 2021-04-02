using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorsPortal;
using DirectorsPortalWPF.EmailMembersUI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Path = System.IO.Path;

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
    /// 
    public partial class EmailMembersSendEmailPage : Page
    {
        List<EmailGroup> emailGroups;
        List<string> gStrAttachedFilePath = new List<string>();
        List<string> gStrFileExtension = new List<string>();
        List<string> gStrFileName = new List<string>();
        DatabaseContext dbContext = new DatabaseContext();
        EmailPage objEmailPage;

        public EmailMembersSendEmailPage(List<EmailGroup> emailGroups, EmailPage emailPage)
        {
            InitializeComponent();
            this.emailGroups = emailGroups;
            objEmailPage = emailPage;
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
            if (ValidateGroupsEmails())
            {
                String[] rgRecipient = GetEmails().ToArray();

                String strContent = GetRichTextDocumentHtmlContent();

                if (gStrAttachedFilePath != null)
                    await GraphApiClient.SendMail(strSubject, rgRecipient, strContent, gStrAttachedFilePath, gStrFileExtension, gStrFileName);
                else
                    await GraphApiClient.SendMail(strSubject, rgRecipient, strContent);

                objEmailPage.LoadEmailGroups();
                LblAttachmentCount.Content = "";
                emailGroups.Clear();
                txtToField.Clear();
                txtSubject.Clear();
                rtbEmailBody.Document.Blocks.Clear();
                gStrAttachedFilePath.Clear();
                gStrFileExtension.Clear();
                gStrFileName.Clear();

                if (rgRecipient.Length > 1)
                    MessageBox.Show($"Message sent to {rgRecipient[0]} and {rgRecipient.Length - 1} others.", "Message Sent");
                else
                    MessageBox.Show($"Message sent to {rgRecipient[0]}.", "Message Sent");
            }
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

            OpenFileDialog FileDialog = new OpenFileDialog();

            FileDialog.ShowDialog();

            String strFilePath = FileDialog.FileName.ToString();

            gStrAttachedFilePath.Add(strFilePath);

            gStrFileExtension.Add(Path.GetExtension(FileDialog.FileName));

            gStrFileName.Add(Path.GetFileNameWithoutExtension(FileDialog.FileName));

            MessageBox.Show(strFilePath, "Attached File Name",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            LblAttachmentCount.Content = $"{gStrAttachedFilePath.Count()} Attachments";

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

        /// <summary>
        /// Pulls all text in the txtToField
        /// Returns a linked list containing all emails within the to Field
        /// </summary>
        /// <returns>A list of emails</returns>
        private List<string> GetEmails()
        {
            List<string> strEmailList = new List<string>();
            List<Email> rgEmails = new List<Email>();
            List<EmailGroupMember> rgEmailGroupMembers;

            string strToField = txtToField.Text;
            // TODO : Repleace Test Group List with list of groups pulled from DB
            // String[] strGroupList = { "Silver", "Gold", "Bronze" };

            foreach(EmailGroup currentEmailGroup in this.emailGroups)
            {
                rgEmailGroupMembers = dbContext.EmailGroupMembers.Where(x => currentEmailGroup.Id == x.GroupId).ToList();
                foreach (EmailGroupMember emailGroupMembers in rgEmailGroupMembers)
                {
                    rgEmails.Add(dbContext.Emails.Where(x => x.Id == emailGroupMembers.EmailId).FirstOrDefault());
                }
            }

            List<string> strGroupEmails = new List<string>();

            foreach (Email currentEmail in rgEmails)
                strGroupEmails.Add(currentEmail.EmailAddress);

            List<string> strEmails = strToField.Trim().Split(';').ToList();
            strEmails.AddRange(strGroupEmails);

            while (strEmails.Contains(""))
                strEmails.Remove("");

            for (int i = 0; i < strEmails.Count(); i++)
                strEmails[i] = strEmails[i].Trim();

            foreach (EmailGroup currentEmailGroup in this.emailGroups)
            {
                if (strEmails.Contains(currentEmailGroup.GroupName.Trim()))
                    strEmails.Remove(currentEmailGroup.GroupName);
            }

            //Removes last Element if it is an empty string
            //There to remove empty string generated by Split() after the final ';'
            if (strEmails.Last().Equals(""))
            {
                strEmails.RemoveAt(strEmails.Count-1);
            }
            foreach (string strEmail in strEmails)
            {
                strEmailList.Add(strEmail);
            }
            return strEmailList;
        }
        /// <summary>
        /// Validates the txtToField
        /// Returns true if all contents are valid email addresses or groups
        /// Returns false if empty or any element is not a group or valid email
        /// </summary>
        /// <returns>A boolean</returns>
        private Boolean ValidateGroupsEmails()
        {
            Boolean blnAllValid = true;
            if (!txtToField.Text.Equals(""))
            {
                string strToField = txtToField.Text;
                string strEmailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
             @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
             @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                Regex re = new Regex(strEmailPattern);
                String strDisplayInvalidEntries = "";
                // TODO : Repleace Test Group List with list of groups pulled from DB
                // string[] strGroupList = { "Silver", "Gold", "Bronze" };
                List<string> strEmailGroups = new List<string>();

                foreach (EmailGroup currentEmailGroup in this.emailGroups)
                    strEmailGroups.Add(currentEmailGroup.GroupName);


                List<string> strEmails = strToField.Trim().Split(';').ToList();

                while (strEmails.Contains(""))
                    strEmails.Remove("");

                //Removes last Element if it is an empty string
                //There to remove empty string generated by Split() after the final ';'
                if (strEmails.Last().Equals(""))
                {
                    strEmails.RemoveAt(strEmails.Count - 1);
                }

                for (int i = 0; i < strEmails.Count(); i++)
                    strEmails[i] = strEmails[i].Trim();

                foreach (string strEmail in strEmails)
                {
                    if (!re.IsMatch(strEmail) && !strEmailGroups.Contains(strEmail))
                    {
                        blnAllValid = false;
                        strDisplayInvalidEntries += strEmail + Environment.NewLine;
                    }
                }

                if (strEmails.Count().Equals(0))
                {
                    MessageBox.Show("Input Emails or Groups in the To Field");
                    blnAllValid = false;
                }
                else if (!blnAllValid)
                {
                    MessageBox.Show($"Invalid entry in To: field : {Environment.NewLine} {strDisplayInvalidEntries}");
                }
            }
            else
            {
                blnAllValid = false;
                MessageBox.Show("Please enter an email address or group to send a message to.");
            }

            return blnAllValid;
        }
    }
}
