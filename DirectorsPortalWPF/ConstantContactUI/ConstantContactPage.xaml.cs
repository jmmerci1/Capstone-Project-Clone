using DirectorsPortalConstantContact;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        // The object containing all data for the user of a Constant Contact account. 
        private ConstantContact gObjConstContact;
        
        /// <summary>
        /// Initialization of the Constant Contact screen. Renders the Side menus for Contact Lists, 
        /// Email Campaigns and a preview of a selected Email Campaign Activity.
        /// </summary>
        public ConstantContactPage()
        {
            InitializeComponent();
            
            //Constant Contact Dev Account
            //Username: edwalk@svsu.edu
            //password: ayC&Aybab6sC422
            //
            // yes this is intentional, this is an accoutn we can all use for dev


            //ConstantContact CC = new ConstantContact();
            //CC.Authenticate();

            //MessageBox.Show(CC.gdctEmailCampaigns.ElementAt(i).Value.Activities.First().permalink_url);

            //LoadEmailCampaigns(CC);
            //LoadEmailGroups(CC);

            /*Update Contact
            Contact c = CC.FindContactByEmail("aamodt@example.com");
            c.company_name = "walmart";
            Console.WriteLine(c.contact_id);
            CC.Update(c);
            */

            /*update list 
            ContactList cl = CC.FindListByName("usable");
            cl.name = "Usable List";
            CC.Update(cl);
            */

            //add campaign
            //CC.AddCampaign();
            
        }

        /// <summary>
        /// Pulls the list of email groups.This function allows for a dynamic load
        /// of the emails groups to the 'email groups' sidepane in the Constant Contact screen.
        /// </summary>
        public void LoadEmailGroups(ConstantContact ccHelper)
        {
            // Pull the email list element from the page
            object nodGroupList = FindName("EmailGroupList");
            // Ensure that the email list element is a stack panel
            if (nodGroupList is StackPanel)
            {

                // Cast email list to stack panel so it can be used
                StackPanel vspGroupList = nodGroupList as StackPanel;
                vspGroupList.Children.Clear();
                // TODO: This should be retrieved from an API from SDK team or database team
                //string[] rgstrGroups = { "Gold Members", "Silver Members", "Restaurants" };

                foreach (var ccEmailGroup in ccHelper.gdctContactLists)
                {
                    // For every email group found in the database, create a row
                    // in the email groups list with label and an edit button
                    StackPanel hspEmailGroupRow = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };

                    Label lblEmailGroupName;
                    if (ccEmailGroup.Value.name.Length > 12)
                    {
                        lblEmailGroupName = new Label()
                        {
                            Content = ccEmailGroup.Value.name.Substring(0, 12) + "..."
                        };
                    }
                    else
                    {
                        lblEmailGroupName = new Label()
                        {
                            Content = ccEmailGroup.Value.name
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
                    btnEmailGroupSelectButton.Click += (sender, e) => AddEmailGroupToMessage(sender, e, ccEmailGroup.Value.name);
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
        public void LoadEmailCampaigns(ConstantContact ccHelper)
        {
            // Pull the email campaign element from the page
            object nodGroupList = FindName("EmailCampaignList");
            // Ensure that the email campaign element is a stack panel
            if (nodGroupList is StackPanel)
            {
                // Cast Email Campaign list to stack panel so it can be used
                StackPanel vspGroupList = nodGroupList as StackPanel;
                vspGroupList.Children.Clear();
                foreach (var dctCampaigns in ccHelper.gdctEmailCampaigns)
                {
                    if (!dctCampaigns.Value.current_status.Equals("DRAFT"))
                    {
                        foreach (var rgActivities in dctCampaigns.Value.Activities)
                        {
                            if (!rgActivities.current_status.Equals("DRAFT"))
                            {
                                // For every Email Campaign found in the API, create a row
                                // in the Email Campaign list with label and an edit button
                                StackPanel hspEmailGroupRow = new StackPanel()
                                {
                                    Orientation = Orientation.Horizontal
                                };

                                Label lblEmailGroupName;
                                if ((dctCampaigns.Value.name + ": " + rgActivities.subject).Length > 18)
                                {
                                    lblEmailGroupName = new Label()
                                    {
                                        Content = (dctCampaigns.Value.name + ": " + rgActivities.subject).Substring(0, 18) + "..."
                                    };
                                }
                                else
                                {
                                    lblEmailGroupName = new Label()
                                    {
                                        Content = dctCampaigns.Value.name + ": " + rgActivities.subject
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

                                btnEmailGroupSelectButton.Click += (sender, e) => PreviewCampaignActivity(sender, e, rgActivities.permalink_url);

                                hspEmailGroupRow.Children.Add(btnEmailGroupSelectButton);
                                hspEmailGroupRow.Children.Add(lblEmailGroupName);
                                vspGroupList.Children.Add(hspEmailGroupRow);

                            }
                            break;  // Constant Contact has two of each activity and I can't see a way to pull only one that is relevant to what we need.
                                    // Will change this once we figure out more on this.
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the Email Campaign preview to the Activity selected by the user on the 
        /// 'Email Campaign' sidebar
        /// </summary>
        /// <param name="sender">The 'Select' button for a particular Email Campaign</param>
        /// <param name="e">The click event</param>
        /// <param name="permalink_url">The permanent link for the selected Activity for an Email Campaign as a String</param>
        private void PreviewCampaignActivity(object sender, RoutedEventArgs e, string permalink_url)
        {
            frmCampaignTemplate.Source = new Uri(permalink_url);
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

        /// <summary>
        /// Creates a background worker on a separate thread (Upon clicking the 'Refresh' button) that does all the constant contact
        /// authentication. The Background worker is necessary to preventing the user interface from locking up
        /// while authentication is occuring. Once Constant Contact authentication is complete, the user interface is updated with data
        /// from the Constant Contact API.
        /// </summary>
        /// <param name="sender">The 'Refresh' button on the Constant Contact screen</param>
        /// <param name="e">The click event</param>
        private void RefreshConstantContact_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bWrk = new BackgroundWorker();

            btnRefreshConstantContact.Content = "Refreshing...";
            btnRefreshConstantContact.Width = 100;

            bWrk.DoWork += AuthenticateConstantContact;
            bWrk.RunWorkerCompleted += LoadConstantContactData;

            bWrk.RunWorkerAsync();

        }

        /// <summary>
        /// To be used by the background worker when refreshing the Constant Contact page. Loads the data from the
        /// Constant Contact API into the UI
        /// </summary>
        /// <param name="sender">The background worker.</param>
        /// <param name="e">The arguments for when the worker completes work</param>
        private void LoadConstantContactData(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadEmailGroups(gObjConstContact);
            LoadEmailCampaigns(gObjConstContact);

            btnRefreshConstantContact.Content = "Refresh";
            btnRefreshConstantContact.Width = 60;
        }

        /// <summary>
        /// To be used by the background worker when refreshing the Constant Contact page. Authenticates the user to
        /// Constant Contact.
        /// </summary>
        /// <param name="sender">The background worker</param>
        /// <param name="e">The arguments for the 'DoWork' event</param>
        private void AuthenticateConstantContact(object sender, DoWorkEventArgs e)
        {
            gObjConstContact = new ConstantContact();
            gObjConstContact.Authenticate();

        }
    }
}
