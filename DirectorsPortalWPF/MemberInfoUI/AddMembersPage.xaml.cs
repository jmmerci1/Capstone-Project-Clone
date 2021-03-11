using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorsPortalWPF.Controls;
using DirectorsPortalWPF.MemberInfoUI.MemberInfoViewModels;
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

namespace DirectorsPortalWPF.MemberInfoUI
{
    /// <summary>
    /// Interaction logic for EditAddMembersPage.xaml
    /// </summary>
    public partial class AddMembersPage : Page
    {
        Dictionary<string, string> GDicHumanReadableDataFields = new Dictionary<string, string>();
        private int IntContactCount = 1;

        /// <summary>
        /// A method for generating the add members UI.
        /// </summary>
        public AddMembersPage()
        {
            InitializeComponent();

            GDicHumanReadableDataFields.Clear();
            GDicHumanReadableDataFields = BusinessDataModel.PopulateHumanReadableDataDic();
        }

        /// <summary>
        /// A method for canceling add business action and taking the user back to the member info page.
        /// </summary>
        /// <param name="sender">The object that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MembersPage());
        }

        /// <summary>
        /// A method for converting the view model to a new business and writing it to the database.
        /// </summary>
        /// <param name="sender">The object that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void BtnAddMember_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MembersPage());
        }

        private void BtnAddContact_Click(object sender, RoutedEventArgs e)
        {
            IntContactCount++;

            ContactInput CiContact = new ContactInput
            {
                GStrTitle = "Contact " + IntContactCount + ":"
            };

            SpContacts.Children.Add(CiContact);
        }
    }
}
