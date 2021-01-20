using DirectorsPortalWPF.EmailMembersUI;
using DirectorsPortalWPF.GenerateReportsUI;
using DirectorsPortalWPF.MemberInfoUI;
using DirectorsPortalWPF.PaymentInfoUI;
using DirectorsPortalWPF.SettingsUI;
using DirectorsPortalWPF.TodoUI;
using DirectorsPortalWPF.ValidateWebsite;
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

namespace DirectorsPortalWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PaymentsPage_Navigate(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new PaymentsPage());
        }

        private void MembersPage_Navigate(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new MembersPage());
        }

        private void EmailPage_Navigate(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new EmailPage());
        }

        private void WebsitePreviewPage_Navigate(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new WebsitePreviewPage());
        }

        private void TodoPage_Navigate(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new TodoPage());
        }

        private void SettingsPage_Navigate(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new SettingsPage());
        }

        private void GenerateReportsPage_Navigate(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new GenerateReportsPage());
        }
    }
}
