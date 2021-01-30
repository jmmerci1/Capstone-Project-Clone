using DirectorsPortalWPF.EmailMembersUI;
using DirectorsPortalWPF.GenerateReportsUI;
using DirectorsPortalWPF.MemberInfoUI;
using DirectorsPortalWPF.PaymentInfoUI;
using DirectorsPortalWPF.SettingsUI;
using DirectorsPortalWPF.TodoUI;
using DirectorsPortalWPF.ValidateWebsite;
using System.Windows;
using System.Windows.Media;

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

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));       
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));      // Appears selected
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        private void MembersPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new MembersPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));        
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));       // Appears selected
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        private void EmailPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new EmailPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));        // Appears selected
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        private void WebsitePreviewPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new WebsitePreviewPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));       // Appears selected
        }

        private void TodoPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new TodoPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));         // Appears selected
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        private void SettingsPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SettingsPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));     // Appears selected
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        private void GenerateReportsPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GenerateReportsPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));     
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));    // Appears selected
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }
    }
}
