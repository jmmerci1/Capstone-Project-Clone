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
        /// </summary>
        /// 
        /// <remarks>
        /// Original Author: Josh Bacon
        /// Date Created: 1/27/2021
        /// </remarks>
        /// 
        /// <param name="sender">The Send button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Send_Email(object sender, RoutedEventArgs e)
        {
            // TODO: Still needs to be implemented
            // Need API to call from the SDK team
        }
    }
}
