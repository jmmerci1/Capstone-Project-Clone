using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
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
/// This file has all of the logic for adding new Email Groups
/// This page allows the director to add new
/// email groups without leaving the portal to use a service
/// like ConstantContact. 
/// </summary>
namespace DirectorsPortalWPF.EmailMembersAddGroupsUI
{
    /// <summary>
    /// Interaction logic for EmailMembersAddGroupsPage.xaml
    /// </summary>
    public partial class EmailMembersAddGroupsPage : Page
    {
        public EmailMembersAddGroupsPage()
        {
            InitializeComponent();
            LoadEmailGroups();
        }
        /// <summary>
        /// Pulls the list of email groups. Depending on whether
        /// Outlook or ConstantContact is used, the list will come
        /// from a different place, but will be rendered on the UI
        /// the same way. This function allows for a dynamic load
        /// of the emails so that if a new group is added, it does
        /// not have to be hard coded.
        /// </summary>
        public void LoadEmailGroups()
        {
            // Pull the email list element from the page
            object nodGroupList = FindName("EmailGroupList");
            // Ensure that the email list element is a stack panel
            if (nodGroupList is StackPanel)
            {
                // Cast email list to stack panel so it can be used
                StackPanel vspGroupList = nodGroupList as StackPanel;

                // TODO: This should be retrieved from an API from SDK team or database team
                string[] rgstrGroups = { "Gold Members", "Silver Members", "Restaurants" };

                foreach (var strGroup in rgstrGroups)
                {
                    // For every email group found in the database, create a row
                    // in the email groups list with label and an edit button
                    StackPanel hspEmailGroupRow = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };
                    Label lblEmailGroupName = new Label()
                    {
                        Content = strGroup
                    };
                    Button btnEmailGroupEditButton = new Button()
                    {
                        Content = "Edit",
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Template = (ControlTemplate)Application.Current.Resources["smallButton"],
                        Padding = new Thickness(0, 0, 35, 0),
                        Height = 15
                    };
                    hspEmailGroupRow.Children.Add(btnEmailGroupEditButton);
                    hspEmailGroupRow.Children.Add(lblEmailGroupName);
                    vspGroupList.Children.Add(hspEmailGroupRow);
                }

            }
        }

        /// <summary>
        /// Gets called on the click of the "Add" button on the edit page.
        /// Will create a new Email Group by saving the contents of the
        /// txtGroupName, txtGroupMembers, and txtNotes fields.
        /// Then will router user to the SendEmailPage
        /// </summary>
        /// <param name="sender">The Add button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Add_Group(object sender, RoutedEventArgs e)
        {
            // TODO: Still needs to be implemented
            string strGroupName = txtGroupName.Text;
            List<Business> businesses = new List<Business>();
            string strNotes = txtNotes.Text;
            using (var context = new DatabaseContext())
            {
                foreach (Business groupMember in lstGroupMembers.Items)
                {
                    Business b = context.Businesses.FirstOrDefault(x => x.Id == groupMember.Id);
                    b.BusinessName = "New Business Name";
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
                    b => b.BusinessName.ToLower().Contains(strSearchTerm.ToLower())
                ).ToList();
                foreach (Business business in queryBusinesses)
                {
                    boolExistsInGroup = false;
                    foreach (Business groupMember in lstGroupMembers.Items)
                        if (groupMember.Id == business.Id)
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
