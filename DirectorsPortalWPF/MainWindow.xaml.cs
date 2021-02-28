﻿using DirectorPortalDatabase;
using DirectorsPortalWPF.ConstantContactUI;
using DirectorsPortalWPF.EmailMembersUI;
using DirectorsPortalWPF.GenerateReportsUI;
using DirectorsPortalWPF.HelpUI;
using DirectorsPortalWPF.MemberInfoUI;
using DirectorsPortalWPF.PaymentInfoUI;
using DirectorsPortalWPF.SettingsUI;
using DirectorsPortalWPF.TodoUI;
using DirectorsPortalWPF.ValidateWebsite;
using System.Windows;
using System.Windows.Media;

/// <summary>
/// File Purpose:
///     This file defines the Main Window object that will contain all the screens used across the Director's Portal Application. 
///     The CCOC heading and sidebar are defined here along with a WPF Frame to contain each screen (WPF Page).
///     
/// </summary>

namespace DirectorsPortalWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Launches the Window containing the application.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));       // Appears selected
                                                                                                                    // Create the database if it doesn't exist
            DatabaseContext dbContextIntialStartup = new DatabaseContext();
            dbContextIntialStartup.Database.EnsureCreated();                     // Ensures the database is created upon application startup. If the database is not created, then the context will create the database.
        }


        /// <summary>
        /// Navigates to the Payments screen. Sets the Payment button to gray so as to appear selected. All other
        /// buttons appear deselected.
        /// </summary>
        /// <param name="sender">The 'Payment Info' Button</param>
        /// <param name="e">The Click Event</param>
        private void PaymentsPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new PaymentsPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));       
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));      // Appears selected
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        /// <summary>
        /// Navigates to the Members screen. Sets the Member Info button to gray so as to appear selected. All other
        /// buttons appear deselected.
        /// </summary>
        /// <param name="sender">The 'Member Info' Button</param>
        /// <param name="e">The Click Event</param>
        private void MembersPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new MembersPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));       // Appears selected
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        /// <summary>
        /// Navigates to the Email Members screen. Sets the Email Members button to gray so as to appear selected. All other
        /// buttons appear deselected.
        /// </summary>
        /// <param name="sender">The 'Email Members' Button</param>
        /// <param name="e">The Click Event</param>
        private void EmailPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new EmailPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));        // Appears selected
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }


        /// <summary>
        /// Navigates to the Validate Website screen. Sets the Validate Website button to gray so as to appear selected. All other
        /// buttons appear deselected.
        /// </summary>
        /// <param name="sender">The 'Validate Website' Button</param>
        /// <param name="e">The Click Event</param>
        private void WebsitePreviewPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new WebsitePreviewPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));       // Appears selected
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }


        /// <summary>
        /// Navigates to the Todos screen. Sets the Todos button to gray so as to appear selected. All other
        /// buttons appear deselected.
        /// </summary>
        /// <param name="sender">The 'TODOs' Button</param>
        /// <param name="e">The Click Event</param>
        private void TodoPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new TodoPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));         // Appears selected
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        /// <summary>
        /// Navigates to the Settings screen. Sets the Settings button to gray so as to appear selected. All other
        /// buttons appear deselected.
        /// </summary>
        /// <param name="sender">The 'Settings' Button</param>
        /// <param name="e">The Click Event</param>
        private void SettingsPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SettingsPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));     // Appears selected
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        /// <summary>
        /// Navigates to the Generate Reports screen. Sets the Generate Reports button to gray so as to appear selected. All other
        /// buttons appear deselected.
        /// </summary>
        /// <param name="sender">The 'Generate Reports' Button</param>
        /// <param name="e">The Click Event</param>
        private void GenerateReportsPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GenerateReportsPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));     
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));    // Appears selected
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
        }

        /// <summary>
        /// Navigates to the Help screen. Sets the Help button to gray so as to appear selected. All other
        /// buttons appear deselected.
        /// </summary>
        /// <param name="sender">The 'Help' Button</param>
        /// <param name="e">The Click Event</param>
        private void HelpScreenPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new HelpScreenPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));         // Appears selected
        }

        private void ConstantContactPage_Navigate(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new ConstantContactPage());

            btnSettings.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnEmail.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnConstantContact.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8D8D8"));  // Appears selected
            btnGenReport.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnMember.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnPayment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnTodo.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnValWeb.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));
            btnHelp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F2F7"));         
        }
    }
}
