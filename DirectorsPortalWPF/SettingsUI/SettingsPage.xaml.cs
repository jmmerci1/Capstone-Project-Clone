using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
/// File Name: SettingsPage.xaml.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Benjamin J. Dore
/// 
/// Date Created: 1/20/2021
/// 
/// File Purpose:
///     This file defines the logic for the 'Settings' screen in the Directors Portal application. The 
///     'Settings' screen consists of tabs including:
///         - Backup and Restore
///         - Edit Fields
///         - OTHERS TO BE DETERMINED
///     
/// Command Line Parameter List:
///     (NONE)
/// 
/// Environmental Returns: 
///     (NONE)
/// 
/// Sample Invocation:
///     This code is executed when the user navigates to the "Settings" screen from the Directors
///     portal main menu. 
///     
/// Global Variable List:
///     (NONE)
///     
/// Modification History:
///     1/20/2021 - BD: Inital creation
///     
/// </summary>

namespace DirectorsPortalWPF.SettingsUI
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {

        /// <summary>
        /// Intializes the Page and content within the Page
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            cmbNotificationFrequency.ItemsSource = new List<string> { "None", "Daily", "Weekly", "Monthly" };
            cmbNotificationTime.ItemsSource = GenerateDropdownTimeList();

            for (int i = 0; i < 10; i++)
            {
                StackPanel sPanelTxtBoxAndBtn = CreateStackPanel(Orientation.Horizontal);

                Button btnEdit = CreateButton("Edit");
                Button btnDelete = CreateButton("Delete");
                btnDelete.Visibility = Visibility.Hidden;

                TextBox txtBoxFieldEdit = CreateTextBox(false);
                txtBoxFieldEdit.Text = $"Field {i + 1}";

                btnEdit.Click += (sender, e) => SetTextField(sender, e, txtBoxFieldEdit, btnDelete);
                btnDelete.Click += (sender, e) => DeleteTextField(sender, e, sPanelTxtBoxAndBtn);

                sPanelTxtBoxAndBtn.Children.Add(txtBoxFieldEdit);
                sPanelTxtBoxAndBtn.Children.Add(btnEdit);
                sPanelTxtBoxAndBtn.Children.Add(btnDelete);

                sPanelFields.Children.Add(sPanelTxtBoxAndBtn);
            }    

        }

        /// <summary>
        ///
        /// Creates a list of times to be used in the dropdown list in the Backup and Restore section 
        /// of Settings.
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/26/2021
        /// 
        /// Modification History:
        ///     1/26/2021 - BD: Initial creation
        ///
        /// </summary>
        /// <returns></returns>
        private List<string> GenerateDropdownTimeList()
        {
            return new List<string>
            {
                "",
                "12:00am",
                "1:00am",
                "2:00am",
                "3:00am",
                "4:00am",
                "5:00am",
                "6:00am",
                "7:00am",
                "8:00am",
                "9:00am",
                "10:00am",
                "11:00am",
                "12:00pm",
                "1:00pm",
                "2:00pm",
                "3:00pm",
                "4:00pm",
                "5.00pm",
                "6:00pm",
                "7:00pm",
                "8:00pm",
                "9:00pm",
                "10:00pm",
                "11:00pm"
            };
        }

        /// <summary>
        /// 
        /// Deletes a Field from the list (will need to be converted for DB use). Will also display a message confirming the delete operation.
        /// 
        /// TODO: Modify the DeleteTextField method in 'Settings' to work with the DB
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/23/2021
        /// 
        /// Modification History:
        ///     1/23/2020 - BD: Intial creation
        ///     
        /// </summary>
        /// <param name="sender">The Delete Button</param>
        /// <param name="e">The Click Event</param>
        /// <param name="stackPanel">The Stack Panel containing the Field to be deleted.</param>
        private void DeleteTextField(object sender, RoutedEventArgs e, StackPanel stackPanel)
        {

            TextBox txtBoxFieldEdit = (TextBox)stackPanel.Children[0];
            MessageBoxResult confirmDelete = MessageBox.Show(
                $"Are you sure you want to delete \'{txtBoxFieldEdit.Text}\'?", 
                "Warning!", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);

            switch(confirmDelete)
            {
                case MessageBoxResult.Yes:
                    sPanelFields.Children.Remove(stackPanel);
                    stackPanel.Children.Clear();

                    GC.Collect();               // Initiate garbage collection so rogue stack panel children isn't floating around in heap.
                    break;
                case MessageBoxResult.No:
                    return;
            }
        }

        /// <summary>
        /// 
        /// Sets the TextBox that contains the name of the Field from the database. Currently just
        /// changes the TextField text that is there. This method can be modified to work with a database.
        /// 
        /// TODO: Modify the SetTextField method in the 'Settings' to work with the DB
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/23/2021
        /// 
        ///  Modification History:
        ///     1/23/2020 - BD: Intial creation
        ///     
        /// </summary>
        /// <param name="sender">The button that is calling the Click event</param>
        /// <param name="e">The event itself</param>
        /// <param name="txtBox">The TextBox that needs to be edited</param>
        private void SetTextField(object sender, RoutedEventArgs e, TextBox txtBox, Button btnDelete)
        {

            Button btnEditText = (Button)sender;       // Sender should be converted to a button.

            if (txtBox.IsEnabled)
            {   // Disable the TextBox and Save, change the button lable back to "Edit"
                txtBox.IsEnabled = false;
                txtBox.Text = txtBox.Text;
                btnEditText.Content = "Edit";
                btnDelete.Visibility = Visibility.Hidden;
            } else
            {   // Enable the TextBox and change the button label to "Save"
                txtBox.IsEnabled = true;
                btnEditText.Content = "Save";
                btnDelete.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// 
        /// Creates a new Text Box for the 'Edit Fields' tab on the Settings screen.
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/23/2021
        /// 
        ///  Modification History:
        ///     1/23/2020 - BD: Intial creation
        ///     
        /// </summary>
        /// <param name="isEnabled">Set the Text Field to either be enabled or disabled</param>
        /// <returns>Returns a generated TextBox object with pre-set formatting</returns>
        private TextBox CreateTextBox(bool isEnabled)
        {
            TextBox newTextBox = new TextBox
            {
                Height = 30,
                Width = 300,
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                IsEnabled = isEnabled
            };

            return newTextBox;
        }

        /// <summary>
        /// 
        /// Created a new Stack Panel to be used by the 'Edit Fields' tab on the Settings screen.
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/23/2021
        /// 
        ///  Modification History:
        ///     1/23/2020 - BD: Intial creation
        ///     
        /// </summary>
        /// <param name="desiredOrientation">Set the orientation of the Stack Panel, either Vertical or Horizontal
        /// (NOTE: use Orientation.[Vertial | Horizontal]</param>
        /// <returns>Returns a generated StackPanel object with pre-set formatting</returns>
        private StackPanel CreateStackPanel(Orientation desiredOrientation)
        {
            StackPanel newStackPanel = new StackPanel
            {
                Orientation = desiredOrientation,
                Height = 40,
                VerticalAlignment = VerticalAlignment.Center
            };

            return newStackPanel;
        }

        /// <summary>
        /// 
        /// Creates a new Button to be used by the 'Edit Fields' tab on the Settings screen.
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/23/2021
        /// 
        ///  Modification History:
        ///     1/23/2020 - BD: Intial creation
        ///     
        /// </summary>
        /// <returns>Returns a generated Button object with pre-set formatting</returns>
        private Button CreateButton(string buttonText)
        {
            Button newButtton = new Button
            {
                Content = buttonText,
                Template = (ControlTemplate)Application.Current.Resources["xtraSmallButton"],
                Margin = new Thickness(5)
            };

            return newButtton;
        }

        /// <summary>
        /// 
        /// Defines the logic for when the 'Open File' button is clicked. Intended to be used to 
        /// select a backup filepath for the application.
        /// 
        /// TODO: The BtnOpenFile_Click method doesn't list a Filepath properly, pursuit a fix.
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/23/2021
        /// 
        ///  Modification History:
        ///     1/23/2020 - BD: Intial creation
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            BackupUtility backupUtility = new BackupUtility();
            
            txtBoxFileBackup.Text = backupUtility.CreateBackup();
           
        }

        /// <summary>
        /// 
        /// Hides and unhides the UI elements for adding a new field to the database.
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/23/2021
        /// 
        ///  Modification History:
        ///     1/23/2020 - BD: Intial creation
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddField_Click(object sender, RoutedEventArgs e)
        {
            if (gridAddField.Visibility == Visibility.Hidden)
            {
                gridAddField.Visibility = Visibility.Visible;
                btnAddField.Content = "Cancel";
            }
            else
            {
                gridAddField.Visibility = Visibility.Hidden;
                btnAddField.Content = "Add Field";
            }
        }

        /// <summary>
        /// 
        /// Saves a new Field to the list when the SaveField button is clicked.
        /// 
        /// TODO: Modify the BtnSaveField_Click to add a new field to the DB.
        /// 
        /// Original Author: Benjamin J. Dore
        /// Date Created: 1/23/2021
        /// 
        ///  Modification History:
        ///     1/23/2020 - BD: Intial creation
        ///     
        /// </summary>
        /// <param name="sender">The button for the Save Field event</param>
        /// <param name="e">The click event</param>
        private void BtnSaveField_Click(object sender, RoutedEventArgs e)
        {
            StackPanel sPanelTxtBoxAndBtn = CreateStackPanel(Orientation.Horizontal);

            TextBox newField = CreateTextBox(false);
            newField.Text = txtBoxFieldName.Text;

            txtBoxFieldName.Text = "";
            gridAddField.Visibility = Visibility.Hidden;

            Button newButtonEdit = CreateButton("Edit");
            Button newButtonDelete = CreateButton("Delete");
            newButtonDelete.Visibility = Visibility.Hidden;

            newButtonEdit.Click += (next_Sender, next_e) => SetTextField(next_Sender, next_e, newField, newButtonDelete);
            newButtonDelete.Click += (next_Sender, next_e) => DeleteTextField(next_Sender, next_e, sPanelTxtBoxAndBtn);

            sPanelTxtBoxAndBtn.Children.Add(newField);
            sPanelTxtBoxAndBtn.Children.Add(newButtonEdit);
            sPanelTxtBoxAndBtn.Children.Add(newButtonDelete);

            sPanelFields.Children.Add(sPanelTxtBoxAndBtn);

            btnAddField.Content = "Add Field";
        }

        private void btnRestoreFromBackup_Click(object sender, RoutedEventArgs e)
        {
            BackupUtility backupUtility = new BackupUtility();
            backupUtility.RestoreFromBackup();
        }

        private void btnCreateBackupNow_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
