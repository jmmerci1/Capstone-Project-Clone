using DirectorsPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            String[] arrRecipient = strRecipient.Split(';');

            String strContent = txtBody.Text;

            await GraphApiClient.SendMail(strSubject, arrRecipient, strContent);
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
            String strID = "";

            await GraphApiClient.GetEmail(strID);
        }
    }
}
