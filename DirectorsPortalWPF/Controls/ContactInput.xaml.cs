using DirectorsPortalWPF.MemberInfoUI;
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

namespace DirectorsPortalWPF.Controls
{
    /// <summary>
    /// Interaction logic for ContactInput.xaml
    /// </summary>
    public partial class ContactInput : UserControl
    {
        public string GStrTitle { get; set; } = string.Empty;
        public int GIntContactId { get; set; } = -1;
        public int GntEmailCount { get; set; } = 0;
        public int GIntNumberCount { get; set; } = 0;
        public List<int> GIntEmailsToRemove { get; set; } = new List<int>();
        public List<int> GIntNumbersToRemove { get; set; } = new List<int>();

        public ContactInput()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void BtnAddEmail_Click(object sender, RoutedEventArgs e)
        {
            GntEmailCount++;

            EmailInput EiEmail = new EmailInput
            {
                GStrInputName = "Email " + GntEmailCount + ":",
            };

            SpContactEmails.Children.Add(EiEmail);
        }

        private void BtnAddNumber_Click(object sender, RoutedEventArgs e)
        {
            GIntNumberCount++;

            ContactNumberInput CniNumber = new ContactNumberInput
            {
                GStrInputName = "Number " + GIntNumberCount + ":",
            };

            SpContactNumbers.Children.Add(CniNumber);
        }

        private void BtnRemoveContact_Click(object sender, RoutedEventArgs e)
        {
            if (GIntContactId != -1) 
            {
                ModifyMembersPage page = NavigationService.GetNavigationService(this).Content as ModifyMembersPage;
                page.GIntContactsToRemove.Add(GIntContactId);
            }

            Content = null;
        }
    }
}
