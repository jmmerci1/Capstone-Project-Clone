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
/// File Name: EmailMemebersEditGroupsPage.xaml.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Daniel Heyse
/// 
/// Date Created: 2/11/2021
/// 
/// File Purpose:
///     This file has all of the logic for editing existing Email Groups
///     This page allows the director to edit all of the
///     different email groups without leaving the portal to use a service
///     like ConstantContact. 
/// </summary>

namespace DirectorsPortalWPF.EmailMembersEditGroupsUI
{
    /// <summary>
    /// Interaction logic for EmailPage.xaml
    /// </summary>
    public partial class EmailMembersEditGroupsPage : Page
    {
        public EmailMembersEditGroupsPage(String strGroup)
        {
            InitializeComponent();
            LoadGroupData(strGroup);
        }
        /// <summary>
        /// Pulls the group name, list of members, and  notes
        /// of the selected group. Then displays them in the txtNotes,
        /// txtGroupsNames, and txtGroupMembers text boxes.
        /// </summary>
        /// 
        /// <remarks>
        /// Original Author: Daniel Heyse
        /// Date Created: 2/11/2021
        /// </remarks>
        public void LoadGroupData(String strGroup)
        {
            txtGroupName.Text = strGroup;
            //TODO: Need to integrate with Database Team
        }

        /// <summary>
        /// Gets called on the click of the "Save" button on the edit page.
        /// Will save changes made to an existing Group.
        /// Then return user to the SendEmailPage
        /// </summary>
        /// 
        /// <remarks>
        /// Original Author: Daniel Heyse
        /// Date Created: 2/11/2021
        /// </remarks>
        /// 
        /// <param name="sender">The Save button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Save_Group(object sender, RoutedEventArgs e)
        {
            // TODO: Still needs to be implemented
            this.NavigationService.Navigate(new EmailMembersSendEmailUI.EmailMembersSendEmailPage());
        }

        /// <summary>
        /// Gets called on the click of the "Cancel" button on the edit page.
        /// Will return the user to the SendEmailPage
        /// </summary>
        /// 
        /// <remarks>
        /// Original Author: Daniel Heyse
        /// Date Created: 2/11/2021
        /// </remarks>
        /// 
        /// <param name="sender">The Cancel button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EmailMembersSendEmailUI.EmailMembersSendEmailPage());
        }
    }
}
