using DirectorsPortalWPF.EmailMembersUI;
using DirectorsPortalWPF.GenerateReportsUI;
using DirectorsPortalWPF.HelpUI;
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

/// <summary>
/// 
/// File Name: MainWindow.xaml.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Benjamin J. Dore
/// 
/// Date Created: 1/20/2021
/// 
/// File Purpose:
///     This file defines the Main Window object that will contain all the screens used across the Director's Portal Application. 
///     The CCOC heading and sidebar are defined here along with a WPF Frame to contain each screen (WPF Page).
///     
/// Command Line Parameter List:
///     (NONE)
/// 
/// Environmental Returns: 
///     (NONE)
/// 
/// Sample Invocation:
///     This code is executed when the user navigates to the "Todo" screen from the Directors
///     portal main menu. 
///     
/// Global Variable List:
///     (NONE)
///     
/// Modification History:
///     1/20/2021 - BD: Inital creation
///     
/// </summary>

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
            mainFrame.Navigate(new PaymentsPage());
        }

        private void MembersPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new MembersPage());
        }

        private void EmailPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new EmailPage());
        }

        private void WebsitePreviewPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new WebsitePreviewPage());
        }

        private void TodoPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new TodoPage());
        }

        private void SettingsPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SettingsPage());
        }

        private void GenerateReportsPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GenerateReportsPage());
        }

        private void HelpScreenPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new HelpScreenPage());
        }
    }
}
