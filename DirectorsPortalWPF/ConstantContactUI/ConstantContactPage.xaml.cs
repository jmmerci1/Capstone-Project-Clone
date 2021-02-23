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
/// File Purpose:
///     This file defines the logic for the Constant Contact screen. Functionality includes displaying
///     email group and email campaings as well as interfacing with the Constant Contact service.
///         
/// </summary>
namespace DirectorsPortalWPF.ConstantContactUI
{
    /// <summary>
    /// Interaction logic for ConstantContactPage.xaml
    /// </summary>
    public partial class ConstantContactPage : Page
    {
        public ConstantContactPage()
        {
            InitializeComponent();
            LoadEmailGroups();
            LoadEmailCampaigns();
        }

        /// <summary>
        /// Pulls the list of email groups.This function allows for a dynamic load
        /// of the emails groups to the 'email groups' sidepane in the Constant Contact screen.
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

                    Label lblEmailGroupName;
                    if (strGroup.Length > 12)
                    {
                        lblEmailGroupName = new Label()
                        {
                            Content = strGroup.Substring(0, 12) + "..."
                        };
                    }
                    else
                    {
                        lblEmailGroupName = new Label()
                        {
                            Content = strGroup
                        };
                    }

                    Button btnEmailGroupSelectButton = new Button()
                    {
                        Content = "Select",
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Template = (ControlTemplate)Application.Current.Resources["smallButton"],
                        Padding = new Thickness(0, 0, 42, 0),
                        Height = 15
                    };
                    Button btnEmailGroupEditButton = new Button()
                    {
                        Content = "Edit",
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Template = (ControlTemplate)Application.Current.Resources["smallButton"],
                        Padding = new Thickness(0, 0, 35, 0),
                        Margin = new Thickness(5, 0, 0, 0),
                        Height = 15
                    };
                    btnEmailGroupSelectButton.Click += (sender, e) => AddEmailGroupToMessage(sender, e, strGroup);
                    hspEmailGroupRow.Children.Add(btnEmailGroupSelectButton);
                    hspEmailGroupRow.Children.Add(btnEmailGroupEditButton);
                    hspEmailGroupRow.Children.Add(lblEmailGroupName);
                    vspGroupList.Children.Add(hspEmailGroupRow);
                }

            }
        }

        /// <summary>
        /// Pulls the list of email campaigns. This function allows for a dynamic load
        /// of the email campaigns to the 'email campaigns' sidepane on the Constant Contact page.
        /// </summary>
        public void LoadEmailCampaigns()
        {
            // Pull the email list element from the page
            object nodGroupList = FindName("EmailCampaignList");
            // Ensure that the email list element is a stack panel
            if (nodGroupList is StackPanel)
            {
                // Cast email list to stack panel so it can be used
                StackPanel vspGroupList = nodGroupList as StackPanel;

                // TODO: This should be retrieved from an API from SDK team or database team
                string[] rgstrGroups = { "Retail Sales", "Spring 2021", "Valenties Day", "Easter Sales Week", "Weekly Update 3/10/2021",
                "Monthly Newsletter", "New COVID State Regulations", "Outdoor Events in Chesaning", "Tourism Information", "Yearly Payment Reminder"};

                foreach (var strGroup in rgstrGroups)
                {
                    // For every email group found in the database, create a row
                    // in the email groups list with label and an edit button
                    StackPanel hspEmailGroupRow = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };

                    Label lblEmailGroupName;
                    if (strGroup.Length > 18)
                    {
                        lblEmailGroupName = new Label()
                        {
                            Content = strGroup.Substring(0,18) + "..."
                        };
                    } else
                    {
                        lblEmailGroupName = new Label()
                        {
                            Content = strGroup
                        };
                    }

                    Button btnEmailGroupSelectButton = new Button()
                    {
                        Content = "Select",
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Template = (ControlTemplate)Application.Current.Resources["smallButton"],
                        Padding = new Thickness(0, 0, 35, 0),
                        Height = 15
                    };
                    hspEmailGroupRow.Children.Add(btnEmailGroupSelectButton);
                    hspEmailGroupRow.Children.Add(lblEmailGroupName);
                    vspGroupList.Children.Add(hspEmailGroupRow);
                }

            }
        }

        /// <summary>
        /// Adds an email group to the 'To:' field, delimited by a semicolon (;)
        /// </summary>
        /// <param name="sender">The 'Select' button for the selected email group</param>
        /// <param name="e">The click event</param>
        /// <param name="emailGroup">The email group name being selected</param>
        private void AddEmailGroupToMessage(object sender, RoutedEventArgs e, string emailGroup)
        {
            // This is just template code for now. In the future this method should be able to read a emailGroup object
            // and load the group name to the 'To' field.

            Button btnSelection = (Button)sender;

            // Only add the email group if it hasn't been selected, otherwise remove the group.
            if (txtEmailGroups.Text.Contains(emailGroup))
            {
                txtEmailGroups.Text = txtEmailGroups.Text.Replace($"{emailGroup}; ", "");
                btnSelection.Content = "Select";
            }
            else
            {
                txtEmailGroups.Text += $"{emailGroup}; ";
                btnSelection.Content = "Remove";
            }
        }

        /// <summary>
        /// Gets called on the click of the "Send" button on the email page.
        /// Will pull the email list, subject, and body, then send it to the
        /// email service to be sent to the appropriate people.
        /// </summary>
        /// <param name="sender">The Send button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Send_Email(object sender, RoutedEventArgs e)
        {
            // TODO: Still needs to be implemented
            // Need API to call from the SDK team
        }

    }
}
