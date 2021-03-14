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
        public string GStrTitle { get; set; }
        public int GntEmailCount { get; set; } = 0;
        public int GIntNumberCount { get; set; } = 0;

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
                GVisRemovable = Visibility.Visible
            };

            SpContactEmails.Children.Add(EiEmail);
        }

        private void BtnAddNumber_Click(object sender, RoutedEventArgs e)
        {
            GIntNumberCount++;

            ContactNumberInput CniNumber = new ContactNumberInput
            {
                GStrInputName = "Number " + GIntNumberCount + ":",
                GVisRemovable = Visibility.Visible
            };

            SpContactNumbers.Children.Add(CniNumber);
        }

        private void BtnRemoveContact_Click(object sender, RoutedEventArgs e)
        {
            Content = null;
        }
    }
}
