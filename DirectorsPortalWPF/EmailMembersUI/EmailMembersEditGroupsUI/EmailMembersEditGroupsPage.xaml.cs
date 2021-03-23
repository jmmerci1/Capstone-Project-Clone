using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;

/// <summary>
/// This file has all of the logic for editing existing Email Groups
/// This page allows the director to edit all of the
/// different email groups without leaving the portal to use a service
/// like ConstantContact. 
/// </summary>
namespace DirectorsPortalWPF.EmailMembersEditGroupsUI
{
    /// <summary>
    /// Interaction logic for EmailMembersEditGroupsUI.xaml
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
        /// <param name="sender">The Save button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Save_Group(object sender, RoutedEventArgs e)
        {
            string strGroupName = txtGroupName.Text;
            List<Business> businesses = new List<Business>();
            string strNotes = txtNotes.Text;
            using (var context = new DatabaseContext())
            {
                foreach (Business groupMember in lstGroupMembers.Items)
                {
                    Business b = context.Businesses.FirstOrDefault(x => x.GIntId == groupMember.GIntId);
                    b.GStrBusinessName = "New Business Name";
                    context.SaveChanges();

                }
            }

            // TODO: Link with database once implemented
            this.NavigationService.Navigate(new EmailMembersSendEmailUI.EmailMembersSendEmailPage());
        }

        /// <summary>
        /// Gets called on the click of the "Remove Member" button on the edit page.
        /// Will remove member from listbox
        /// </summary>
        /// <param name="sender">The Save button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Remove_Member(object sender, RoutedEventArgs e)
        {
            lstGroupMembers.Items.Remove(lstGroupMembers.SelectedItem);
        }

        /// <summary>
        /// Gets called on the click of the "Cancel" button on the edit page.
        /// Will return the user to the SendEmailPage
        /// </summary>
        /// <param name="sender">The Cancel button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EmailMembersSendEmailUI.EmailMembersSendEmailPage());
        }

        /// <summary>
        /// Gets called when user types in Add Member To Group textbox.
        /// Querys the database for data matching entered text
        /// </summary>
        /// <param name="sender">The Add Member to Group textbox object that has called the function.</param>
        /// <param name="e">The text changed event</param>
        private void Search_Database(object sender, TextChangedEventArgs e)
        {
            lstPopup.Items.Clear();
            string strSearchTerm = txtAddGroupMembers.Text;
            Boolean boolExistsInGroup = false;

            popSearch.IsOpen = true;
            using (var context = new DatabaseContext())
            {
                List<Business> queryBusinesses = context.Businesses.Where(
                    b => b.GStrBusinessName.ToLower().Contains(strSearchTerm.ToLower())
                ).ToList();
                foreach (Business business in queryBusinesses)
                {
                    boolExistsInGroup = false;
                    foreach (Business groupMember in lstGroupMembers.Items)
                        if (groupMember.GIntId == business.GIntId)
                        {
                            boolExistsInGroup = true;
                            break;
                        }
                    if (!boolExistsInGroup)
                        lstPopup.Items.Add(business);
                }
            }
        }

        /// <summary>
        /// Gets called when user clicks away from Add Member To Group textbox.
        /// Hides the popup used to display search results
        /// </summary>
        /// <param name="sender">The Add Member to Group textbox object that has called the function.</param>
        /// <param name="e">The lost focus event</param>
        private void Hide_Search(object sender, RoutedEventArgs e)
        {
            popSearch.IsOpen = false;
        }

        /// <summary>
        /// Gets called when user selects an item from the popup.
        /// Displays the item in the group members list box
        /// </summary>
        /// <param name="sender">The popup object that has called the function.</param>
        /// <param name="e">The selection changed event</param>
        private void Add_Member_To_Group(object sender, SelectionChangedEventArgs e)
        {
            if (lstPopup.SelectedIndex >= 0)
            {
                lstGroupMembers.Items.Add(lstPopup.SelectedItem);
                txtAddGroupMembers.Clear();
                popSearch.IsOpen = false;
            }
        }
    }
}
