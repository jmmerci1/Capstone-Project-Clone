using System.Windows;
using System.Windows.Controls;

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
                        Padding = new Thickness(0,0,35,0),
                        Height = 15
                    };
                    hspEmailGroupRow.Children.Add(btnEmailGroupEditButton);
                    hspEmailGroupRow.Children.Add(lblEmailGroupName);
                    vspGroupList.Children.Add(hspEmailGroupRow);
                }

            }
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
