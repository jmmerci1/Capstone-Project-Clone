using DirectorPortalDatabase.Utility;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// 
/// File Name: SettingsPage.xaml.cs
/// 
/// Part of Project: DirectorsPortal
/// 
/// Original Author: Benjamin J. Dore
/// 
/// File Purpose:
///     This file defines the logic for the 'Settings' screen in the Directors Portal application. The 
///     'Settings' screen consists of tabs including:
///         - Backup and Restore
///         - Edit Fields
///         - OTHERS TO BE DETERMINED
///         
/// </summary>

namespace DirectorsPortalWPF.SettingsUI
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private ClsMetadataHelper.ClsModelInfo GUdtSelectedReportType { get; set; }

        /// <summary>
        /// Intializes the Page and content within the Page
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            cmbNotificationFrequency.ItemsSource = new List<string> { "None", "Daily", "Weekly", "Monthly" };
            cmbNotificationTime.ItemsSource = GenerateDropdownTimeList();

            ComboBoxItem[] rgReportTypeItems = new ComboBoxItem[ClsMetadataHelper.IntNumberOfModels];

            for (int i = 0; i < ClsMetadataHelper.IntNumberOfModels; i++)
            {
                // Gets info on the i-th database model.
                Type typeModelType = ClsMetadataHelper.GetModelTypeByIndex(i);
                ClsMetadataHelper.ClsModelInfo udtModelInfo = ClsMetadataHelper.GetModelInfo(typeModelType);

                // Stores model information in a new ComboBoxItem.
                ComboBoxItem cbiModelItem = new ComboBoxItem();
                cbiModelItem.Content = udtModelInfo.StrHumanReadableName;
                cbiModelItem.Tag = udtModelInfo;

                // Adds the ComboBoxItem to the array.
                rgReportTypeItems[i] = cbiModelItem;
            }

            cmbEntity.ItemsSource = rgReportTypeItems;


        }

        /// <summary>
        /// Creates a list of times to be used in the dropdown list in the Backup and Restore section 
        /// of Settings.
        /// </summary>
        /// <returns>A list of all times in a day</returns>
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
        /// Deletes a Field from the list (will need to be converted for DB use). Will also display a message confirming the delete operation.
        /// 
        /// TODO: Modify the DeleteTextField method in 'Settings' to work with the DB
        /// </summary>
        /// <param name="sender">The Delete Button</param>
        /// <param name="e">The Click Event</param>
        /// <param name="sPanelToDelete">The Stack Panel containing the Field to be deleted.</param>
        private void DeleteTextField(object sender, RoutedEventArgs e, StackPanel sPanelToDelete)
        {

            TextBox txtBoxFieldEdit = (TextBox)sPanelToDelete.Children[0];
            MessageBoxResult confirmDelete = MessageBox.Show(
                $"Are you sure you want to delete \'{txtBoxFieldEdit.Text}\'?",
                "Warning!",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            switch (confirmDelete)
            {
                case MessageBoxResult.Yes:
                    sPanelFields.Children.Remove(sPanelToDelete);
                    sPanelToDelete.Children.Clear();

                    GC.Collect();               // Initiate garbage collection so rogue stack panel children isn't floating around in heap.
                    break;
                case MessageBoxResult.No:
                    return;
            }
        }

        /// <summary>
        /// Sets the TextBox that contains the name of the Field from the database. Currently just
        /// changes the TextField text that is there. This method can be modified to work with a database.
        /// 
        /// TODO: Modify the SetTextField method in the 'Settings' to work with the DB
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
            }
            else
            {   // Enable the TextBox and change the button label to "Save"
                txtBox.IsEnabled = true;
                btnEditText.Content = "Save";
                btnDelete.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// Creates a new Text Box for the 'Edit Fields' tab on the Settings screen.
        /// </summary>
        /// <param name="blnIsEnabled">Set the Text Field to either be enabled or disabled</param>
        /// <returns>Returns a generated TextBox object with pre-set formatting</returns>
        private TextBox CreateTextBox(bool blnIsEnabled)
        {
            TextBox txtBoxNewTextBox = new TextBox
            {
                Height = 30,
                Width = 300,
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                IsEnabled = blnIsEnabled
            };

            return txtBoxNewTextBox;
        }

        /// <summary>
        /// Created a new Stack Panel to be used by the 'Edit Fields' tab on the Settings screen.
        /// </summary>
        /// <param name="OriDesiredOrientation">Set the orientation of the Stack Panel, either Vertical or Horizontal
        /// (NOTE: use Orientation.[Vertial | Horizontal]</param>
        /// <returns>Returns a generated StackPanel object with pre-set formatting</returns>
        private StackPanel CreateStackPanel(Orientation OriDesiredOrientation)
        {
            StackPanel sPanelNewStackPanel = new StackPanel
            {
                Orientation = OriDesiredOrientation,
                Height = 40,
                VerticalAlignment = VerticalAlignment.Center
            };

            return sPanelNewStackPanel;
        }

        /// <summary>
        /// Creates a new Button to be used by the 'Edit Fields' tab on the Settings screen.
        /// </summary>
        /// <returns>Returns a generated Button object with pre-set formatting</returns>
        private Button CreateButton(string strButtonText)
        {
            Button btnNewButtton = new Button
            {
                Content = strButtonText,
                Template = (ControlTemplate)Application.Current.Resources["xtraSmallButton"],
                Margin = new Thickness(5)
            };

            return btnNewButtton;
        }

        /// <summary>
        /// Defines the logic for when the 'Open File' button is clicked. Intended to be used to 
        /// select a backup filepath for the application.
        /// 
        /// TODO: The BtnOpenFile_Click method doesn't list a Filepath properly, pursuit a fix.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtBoxFileBackup.Text = File.ReadAllText(openFileDialog.FileName);
        }

        /// <summary>
        /// Hides and unhides the UI elements for adding a new field to the database.
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
        /// Saves a new Field to the list when the SaveField button is clicked.
        /// 
        /// TODO: Modify the BtnSaveField_Click to add a new field to the DB.
        /// </summary>
        /// <param name="sender">The button for the Save Field event</param>
        /// <param name="e">The click event</param>
        private void BtnSaveField_Click(object sender, RoutedEventArgs e)
        {
            StackPanel sPanelTxtBoxAndBtn = CreateStackPanel(Orientation.Horizontal);

            TextBox txtBoxNewField = CreateTextBox(false);
            txtBoxNewField.Text = txtBoxFieldName.Text;

            txtBoxFieldName.Text = "";
            gridAddField.Visibility = Visibility.Hidden;

            Button btnNewButtonEdit = CreateButton("Edit");
            Button btnNewButtonDelete = CreateButton("Delete");
            btnNewButtonDelete.Visibility = Visibility.Hidden;

            btnNewButtonEdit.Click += (next_Sender, next_e) => SetTextField(next_Sender, next_e, txtBoxNewField, btnNewButtonDelete);
            btnNewButtonDelete.Click += (next_Sender, next_e) => DeleteTextField(next_Sender, next_e, sPanelTxtBoxAndBtn);

            sPanelTxtBoxAndBtn.Children.Add(txtBoxNewField);
            sPanelTxtBoxAndBtn.Children.Add(btnNewButtonEdit);
            sPanelTxtBoxAndBtn.Children.Add(btnNewButtonDelete);

            sPanelFields.Children.Add(sPanelTxtBoxAndBtn);

            btnAddField.Content = "Add Field";
        }

        private void BtnImportExcel_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add import to excel logic here
        }

        /// <summary>
        /// After selecting a table from the 'Entity' combo box, a list of all fields will appear in that
        /// selected table. Users will have the ability to edit/delete the field names in the selected table.
        /// </summary>
        /// <param name="sender">The Selected Table from the Combo Box</param>
        /// <param name="e">The SelectionChange event</param>
        private void CmbEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Gets the selected report type name from the combo box.
            ComboBoxItem cbiSelectedReportTypeItem = (ComboBoxItem)cmbEntity.SelectedItem;
            // Extracts the model information from the ComboBoxItem.
            GUdtSelectedReportType = (ClsMetadataHelper.ClsModelInfo)cbiSelectedReportTypeItem.Tag;
            // Clear the fields populated from the last table selection.
            sPanelFields.Children.Clear();

            if (GUdtSelectedReportType != null)
            {
                int intNumberOfFields = GUdtSelectedReportType.UdtTableMetaData.IntNumberOfFields;

                for (int i = 0; i < intNumberOfFields; i++)
                {
                    ClsMetadataHelper.ClsTableField udtField = GUdtSelectedReportType.UdtTableMetaData.GetField(i);

                    StackPanel sPanelTxtBoxAndBtn = CreateStackPanel(Orientation.Horizontal);

                    Button btnEdit = CreateButton("Edit");
                    Button btnDelete = CreateButton("Delete");
                    btnDelete.Visibility = Visibility.Hidden;

                    TextBox txtBoxFieldEdit = CreateTextBox(false);
                    txtBoxFieldEdit.Text = $"{udtField.StrHumanReadableName}";

                    btnEdit.Click += (next_sender, next_e) => SetTextField(next_sender, next_e, txtBoxFieldEdit, btnDelete);
                    btnDelete.Click += (next_sender, next_e) => DeleteTextField(next_sender, next_e, sPanelTxtBoxAndBtn);

                    sPanelTxtBoxAndBtn.Children.Add(txtBoxFieldEdit);
                    sPanelTxtBoxAndBtn.Children.Add(btnEdit);
                    sPanelTxtBoxAndBtn.Children.Add(btnDelete);

                    sPanelFields.Children.Add(sPanelTxtBoxAndBtn);
                }
            }
        }
    }
}
