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
/// File Name: EmailPage.xaml.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Josh Bacon
/// 
/// Date Created: 1/25/2021
/// 
/// File Purpose:
///     This file has all of the logic for handling the email page.
///     This page allows the director to send emails to all of the
///     different groups without leaving the portal to use a service
///     like ConstantContact. 
/// </summary>
namespace DirectorsPortalWPF.EmailMembersUI
{
    /// <summary>
    /// Interaction logic for EmailPage.xaml
    /// </summary>
    public partial class EmailPage : Page
    {
        /// <summary>
        /// Test list for setting up the UI. This variable should not be used in
        /// the final implmentation.
        /// </summary>
        List<Group> GroupList = new List<Group>();

        /// <summary>
        /// Initialize the email page. Automatically gets run
        /// upon creating the page in WPF. Will call the
        /// <see cref="EmailPage.LoadEmailGroups()"/> function
        /// to load in a list of the email groups to the UI.
        /// </summary>
        /// <remarks>
        /// Original Author: Josh Bacon
        /// Date Created: 1/27/2021
        /// </remarks>

        public EmailPage()
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
        /// <remarks>
        /// Original Author: Josh Bacon
        /// Date Created: 1/27/2021
        /// </remarks>
        public void LoadEmailGroups()
        {
            // Pull the email list element from the page
            object nodGroupList = FindName("EmailGroupList");
            // Ensure that the email list element is a stack panel
            if (nodGroupList is StackPanel)
            {
                // Cast email list to stack panel so it can be used
                StackPanel vspGroupList = nodGroupList as StackPanel;

                // TODO: GroupList should be retrieved from an API from SDK team or database team. Values added for test purposes
                GroupList.Add(new Group("Silver",new string[] { "Tom", "John" } , "Test Note"));
                GroupList.Add(new Group("Gold", new string[] { "Jane", "Bill" }, "Test Note2"));

                foreach (Group group in GroupList)
                {
                    // For every email group found in the database, create a row
                    // in the email groups list with label and an edit button
                    StackPanel hspEmailGroupRow = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };
                    Label lblEmailGroupName = new Label()
                    {
                        Content = group.Name
                    };
                    Button btnEmailGroupEditButton = new Button()
                    {
                        Content = "Edit",
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Template = (ControlTemplate)Application.Current.Resources["smallButton"],
                        Padding = new Thickness(0,0,35,0),
                        Height = 15,
                    };

                    btnEmailGroupEditButton.Click += (s, e) => 
                    {
                        /// <summary>
                        /// Navigates to the EditGroups screen and passes the corresponding group name
                        /// </summary>
                        /// <param name="sender">The 'Edit' Button</param>
                        /// <param name="e">The Click Event</param>
                        emailFrame.Navigate(new EmailMembersEditGroupsUI.EmailMembersEditGroupsPage(group.Name)); 
                    };
                        hspEmailGroupRow.Children.Add(btnEmailGroupEditButton);
                    hspEmailGroupRow.Children.Add(lblEmailGroupName);
                    vspGroupList.Children.Add(hspEmailGroupRow);
                }

            }
        }
        /// <summary>
        /// Navigates to the AddGroups screen.
        /// </summary>
        /// <param name="sender">The 'Add' Button</param>
        /// <param name="e">The Click Event</param>
        private void AddGroupsPage_Navigate(object sender, RoutedEventArgs e)
        {
            emailFrame.Navigate(new EmailMembersAddGroupsUI.EmailMembersAddGroupsPage());
        }
    }

}

/// <summary>
/// A test class that defines the properties of a Group.
/// </summary>
public class Group
{
    public string Name { get; set; }
    public string[] Members { get; set; }
    public string Note { get; set; }

    public Group(string name, string[] members, string note)
    {
        Name = name;
        Members = members;
        Note = note;
    }
}
