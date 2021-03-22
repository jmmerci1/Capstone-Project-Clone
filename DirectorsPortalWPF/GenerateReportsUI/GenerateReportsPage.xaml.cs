using System;
using System.Windows.Controls;
using ExcelDataReader;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DirectorPortalDatabase.Utility;
using ClosedXML.Excel;

namespace DirectorsPortalWPF.GenerateReportsUI
{
    /// <summary>
    /// Interaction logic for GenerateReportsPage.xaml
    /// </summary>
    public partial class GenerateReportsPage : Page
    {

        private ClsMetadataHelper.ClsModelInfo GUdtSelectedReportType { get; set; }
        private ComboBoxItem[] GRGReportTypeItems { get; set; }
        private List<string[]> GRGCurrentReport { get; set; }
        private List<ReportTemplate> GRGReportTemplates { get; set; }

        private int intKeyForExport = 0;



        public void ReportTypeSelectedHandler(object sender, EventArgs e)
        {
            // Gets the selected report type name from the combo box.
            ComboBoxItem cbiSelectedReportTypeItem = (ComboBoxItem)cboReportType.SelectedItem;
            // Extracts the model information from the ComboBoxItem.
            GUdtSelectedReportType = (ClsMetadataHelper.ClsModelInfo)cbiSelectedReportTypeItem.Tag;
            if (GUdtSelectedReportType != null)
            {
                int intNumberOfFields = GUdtSelectedReportType.UdtTableMetaData.IntNumberOfFields;
                ListBoxItem[] rgFieldItems = new ListBoxItem[intNumberOfFields];

                // Iterates over the fields of the selected table.
                for (int i = 0; i < intNumberOfFields; i++)
                {
                    // Stores the i-th field's information in a new ListBoxItem.
                    ClsMetadataHelper.ClsTableField udtField = GUdtSelectedReportType.UdtTableMetaData.GetField(i);
                    ListBoxItem lbiFieldItem = new ListBoxItem();
                    lbiFieldItem.Content = udtField.StrHumanReadableName;
                    lbiFieldItem.Tag = udtField;

                    // Stores the new ListBoxItem in the array.
                    rgFieldItems[i] = lbiFieldItem;
                }
                // Displays the table's fields in the listbox.
                lstReportFields.ItemsSource = rgFieldItems;
            }
        }

        private List<ClsMetadataHelper.ClsTableField> GetSelectedFields()
        {
            List<ClsMetadataHelper.ClsTableField> rgSelectedFields = new List<ClsMetadataHelper.ClsTableField>();
            // Iterates over the selected ListBoxItem objects and extracts their ClsTableField objects.
            foreach (ListBoxItem lbiSelectedFieldItem in lstReportFields.SelectedItems)
            {
                ClsMetadataHelper.ClsTableField udtField = (ClsMetadataHelper.ClsTableField)lbiSelectedFieldItem.Tag;
                rgSelectedFields.Add(udtField);
            }
            return rgSelectedFields;
        }

        private void RenderReport()
        {
            // Clears the existing grid content.
            grdReportContent.Children.Clear();
            grdReportContent.RowDefinitions.Clear();
            grdReportContent.ColumnDefinitions.Clear();

            int intRowCount = GRGCurrentReport.Count;

            if (intRowCount > 0)
            {
                int intColCount = GRGCurrentReport[0].Length;

                for (int i = 0; i < intColCount; i++)
                {
                    grdReportContent.ColumnDefinitions.Add(new ColumnDefinition());
                }
                for (int i = 0; i < intRowCount; i++)
                {
                    grdReportContent.RowDefinitions.Add(new RowDefinition());
                }
                for (int i = 0; i < intRowCount; i++)
                {
                    for (int j = 0; j < intColCount; j++)
                    {
                        // Stores the report cell data in a TextBox, which is inserted into the Grid.
                        TextBox txtReportCell = new TextBox();
                        txtReportCell.IsReadOnly = true;
                        txtReportCell.Text = GRGCurrentReport[i][j];
                        txtReportCell.SetValue(Grid.RowProperty, i);
                        txtReportCell.SetValue(Grid.ColumnProperty, j);
                        grdReportContent.Children.Add(txtReportCell);
                    }
                }
            }

        }

        private void RenderReportTemplateList()
        {
            // Clears any existing grid content on the second page.
            grdReportTemplateList.Children.Clear();
            grdReportTemplateList.RowDefinitions.Clear();
            //grdReportTemplateList.ColumnDefinitions.Clear();

            int intRowCount = GRGReportTemplates.Count;

            for (int i = 0; i < intRowCount; i++)
            {
                // Defines a new row for this report template.
                grdReportTemplateList.RowDefinitions.Add(new RowDefinition());

                // Creates a text block to display the name of the template.
                TextBlock txtTemplateName = new TextBlock();
                txtTemplateName.Text = GRGReportTemplates[i].GStrReportTemplateName;
                txtTemplateName.SetValue(Grid.RowProperty, i);
                txtTemplateName.SetValue(Grid.ColumnProperty, 0);
                grdReportTemplateList.Children.Add(txtTemplateName);

                // Creates a button to view the report.
                Button btnViewReport = new Button();
                btnViewReport.Content = "View Report";
                btnViewReport.Tag = GRGReportTemplates[i];
                btnViewReport.Click += ViewReportButtonHandler;
                btnViewReport.SetValue(Grid.RowProperty, i);
                btnViewReport.SetValue(Grid.ColumnProperty, 1);
                grdReportTemplateList.Children.Add(btnViewReport);

                // Creates a button to export the report to Excel.
                Button btnExportToExcel = new Button();
                btnExportToExcel.Content = "Export to Excel";
                btnExportToExcel.Tag = GRGReportTemplates[i];
                btnExportToExcel.Click += ExportToExcelButtonHandler;
                btnExportToExcel.SetValue(Grid.RowProperty, i);
                btnExportToExcel.SetValue(Grid.ColumnProperty, 2);
                grdReportTemplateList.Children.Add(btnExportToExcel);

                // Creates a button to delete the report template.
                Button btnDeleteReport = new Button();
                btnDeleteReport.Content = "Delete Report";
                btnDeleteReport.Tag = GRGReportTemplates[i];
                btnDeleteReport.Click += DeleteReportButtonHandler;
                btnDeleteReport.SetValue(Grid.RowProperty, i);
                btnDeleteReport.SetValue(Grid.ColumnProperty, 3);
                grdReportTemplateList.Children.Add(btnDeleteReport);
            }
        }

        private void GetReportTemplateList()
        {
            using (DirectorPortalDatabase.DatabaseContext context = new DirectorPortalDatabase.DatabaseContext())
            {
                GRGReportTemplates = context.ReportTemplates.ToList();
            }
            RenderReportTemplateList();
        }

        /// <summary>
        /// Queries the database for all ReportField records belonging to the specified ReportTemplate.
        /// </summary>
        /// <param name="udtTemplate"></param>
        private List<ReportField> GetReportTemplateFields(ReportTemplate udtTemplate)
        {
            List<ReportField> rgReportFields;

            using (DirectorPortalDatabase.DatabaseContext context = new DirectorPortalDatabase.DatabaseContext())
            {
                rgReportFields = context.ReportFields.Where(x => x.GIntReportTemplateId == udtTemplate.GIntId).ToList();
            }

            return rgReportFields;
        }

        public void GenerateReportButtonHandler(object sender, EventArgs e)
        {
            if (GUdtSelectedReportType != null)
            {
                // The report is a list of rows, each in the form of a string array.
                List<string[]> rgReport = new List<string[]>();

                List<ClsMetadataHelper.ClsTableField> rgSelectedFields = GetSelectedFields();

                string[] rgFieldNames = rgSelectedFields.Select(x => x.StrHumanReadableName).ToArray();

                // The first row consists of the names of all included fields.
                rgReport.Add(rgFieldNames);

                using (DirectorPortalDatabase.DatabaseContext context = new DirectorPortalDatabase.DatabaseContext())
                {
                    // Queries for all instances of the specified model.
                    object[] udtModels = GUdtSelectedReportType.GetContextDbSet(context).Cast<object>().ToArray();

                    foreach (object objRecord in udtModels)
                    {
                        string[] rgReportRow = new string[rgSelectedFields.Count];

                        for (int i = 0; i < rgSelectedFields.Count; i++)
                        {
                            // Gets the value of the selected field in the current record.
                            ClsMetadataHelper.ClsTableField udtSelectedField = rgSelectedFields[i];
                            object objFieldValue = udtSelectedField.GetValue(objRecord);

                            // Stores a string in the report row representing the value just obtained.
                            string strReportCell = objFieldValue?.ToString() ?? "";
                            rgReportRow[i] = strReportCell;
                        }

                        rgReport.Add(rgReportRow);
                    }
                }

                GRGCurrentReport = rgReport;

                //Checks if integer is set to allow the Report to be Rendered.
                if (intKeyForExport == 0)
                {
                    RenderReport();
                }



            }
        }
        /// <summary>
        /// Opens a pop-up window that displays the current frames help information. 
        /// </summary>
        /// <param name="sender">Help button</param>
        /// <param name="e">The Click event</param>
        public void HelpButtonHandler(object sender, EventArgs e)
        {
            HelpUI.HelpScreenWindow helpWindow = new HelpUI.HelpScreenWindow();
            helpWindow.Show();
            helpWindow.tabs.SelectedIndex = 3;

        }

        public void SaveReportTypeButtonHandler(object sender, EventArgs e)
        {
            if (GUdtSelectedReportType != null)
            {
                // Creates a model instance to represent the report template.
                ReportTemplate udtTemplate = new ReportTemplate
                {
                    GStrModelName = GUdtSelectedReportType.TypeModelType.Name,
                    GStrReportTemplateName = $"Template {DateTime.Now.Ticks}"
                };

                using (DirectorPortalDatabase.DatabaseContext context = new DirectorPortalDatabase.DatabaseContext())
                {
                    // Inserts the template record into the database.
                    context.ReportTemplates.Add(udtTemplate);
                    context.SaveChanges();

                    // Creates a model instance for each field in the template.
                    List<ClsMetadataHelper.ClsTableField> rgSelectedFields = GetSelectedFields();
                    foreach (ClsMetadataHelper.ClsTableField udtField in rgSelectedFields)
                    {
                        ReportField udtReportField = new ReportField
                        {
                            // This ID links to the template record.
                            GIntReportTemplateId = udtTemplate.GIntId,
                            GStrModelPropertyName = udtField.StrPropertyName
                        };

                        context.ReportFields.Add(udtReportField);
                    }
                    context.SaveChanges();
                }

                // Gets the updated report template list.
                GetReportTemplateList();
            }
        }

        public void ViewReportButtonHandler(object sender, EventArgs e)
        {
            // Gets the report template instance from the button.
            Button btnSender = (Button)sender;
            ReportTemplate udtReportTemplate = (ReportTemplate)btnSender.Tag;

            GUdtSelectedReportType = null;

            // Searches for the combo box item with a matching table model.
            for (int i = 0; i < GRGReportTypeItems.Length; i++)
            {
                ComboBoxItem cbiReportTypeItem = GRGReportTypeItems[i];

                // Gets the model information from the ComboBoxItem.
                ClsMetadataHelper.ClsModelInfo udtModelInfo = (ClsMetadataHelper.ClsModelInfo)cbiReportTypeItem.Tag;

                // Checks for matching model names.
                if (udtModelInfo.TypeModelType.Name == udtReportTemplate.GStrModelName)
                {

                    // Sets this ComboBoxItem as active.
                    cbiReportTypeItem.IsSelected = true;
                    cboReportType.SelectedIndex = i;
                    cboReportType.SelectedItem = cbiReportTypeItem;

                    // Manually calls the ComboBox event handler to fill the ListBox with the appropriate fields.
                    ReportTypeSelectedHandler(null, null);

                    // Gets the fields belonging to the selected report template.
                    List<ReportField> rgReportTemplateFields = GetReportTemplateFields(udtReportTemplate);

                    // Iterates over the fields in the ListBox.
                    foreach (ListBoxItem lbiFieldItem in lstReportFields.Items)
                    {
                        // Iterates over the fields in the report template.
                        foreach (ReportField udtReportTemplateField in rgReportTemplateFields)
                        {

                            ClsMetadataHelper.ClsTableField udtTableField
                                = (ClsMetadataHelper.ClsTableField)lbiFieldItem.Tag;

                            // Checks for a match.
                            if (udtReportTemplateField.GStrModelPropertyName == udtTableField.StrPropertyName)
                            {
                                // Selects the ListBoxItem.
                                lbiFieldItem.IsSelected = true;
                                lstReportFields.SelectedItems.Add(lbiFieldItem);
                                break;
                            }
                        }
                    }

                    // Manually calls the event handler to generate the report.
                    GenerateReportButtonHandler(null, null);

                    tbiGenerateReports.IsSelected = true;

                    break;
                }
            }

            //for (int i = 0; i < ClsMetadataHelper.IntNumberOfModels; i++)
            //{
            //    Type typeModelType = ClsMetadataHelper.GetModelTypeByIndex(i);

            //    if (typeModelType.Name == udtReportTemplate.GStrModelName)
            //    {
            //        GUdtSelectedReportType = ClsMetadataHelper.GetModelInfo(typeModelType);
            //        break;
            //    }
            //}

            //if (GUdtSelectedReportType != null)
            //{
            //    cboReportType.SelectedItem = cboReportType.Items
            //}
        }

        /// <summary>
        /// Gathers the selected data from the database, then allows the user to select where and what to name the 
        /// excel file. 
        /// </summary>
        /// <param name="sender">The 'Exoirt to Excel' Button</param>
        /// <param name="e">The Click Event</param>
        public void ExportToExcelButtonHandler(object sender, EventArgs e)
        {
            //Variables to create an excel workbook.
            var wbWorkbook = new XLWorkbook();
            wbWorkbook.AddWorksheet("sheetName");
            var wsSheet = wbWorkbook.Worksheet("sheetName");

            //String for save file path
            String strfilepath = " ";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            // Gets the report template instance from the button.
            Button btnSender = (Button)sender;
            ReportTemplate udtReportTemplate = (ReportTemplate)btnSender.Tag;

            GUdtSelectedReportType = null;

            // Searches for the combo box item with a matching table model.
            for (int i = 0; i < GRGReportTypeItems.Length; i++)
            {
                ComboBoxItem cbiReportTypeItem = GRGReportTypeItems[i];

                // Gets the model information from the ComboBoxItem.
                ClsMetadataHelper.ClsModelInfo udtModelInfo = (ClsMetadataHelper.ClsModelInfo)cbiReportTypeItem.Tag;

                // Checks for matching model names.
                if (udtModelInfo.TypeModelType.Name == udtReportTemplate.GStrModelName)
                {

                    // Sets this ComboBoxItem as active.
                    cbiReportTypeItem.IsSelected = true;
                    cboReportType.SelectedIndex = i;
                    cboReportType.SelectedItem = cbiReportTypeItem;

                    // Manually calls the ComboBox event handler to fill the ListBox with the appropriate fields.
                    ReportTypeSelectedHandler(null, null);

                    // Gets the fields belonging to the selected report template.
                    List<ReportField> rgReportTemplateFields = GetReportTemplateFields(udtReportTemplate);

                    // Iterates over the fields in the ListBox.
                    foreach (ListBoxItem lbiFieldItem in lstReportFields.Items)
                    {
                        // Iterates over the fields in the report template.
                        foreach (ReportField udtReportTemplateField in rgReportTemplateFields)
                        {

                            ClsMetadataHelper.ClsTableField udtTableField
                                = (ClsMetadataHelper.ClsTableField)lbiFieldItem.Tag;

                            // Checks for a match.
                            if (udtReportTemplateField.GStrModelPropertyName == udtTableField.StrPropertyName)
                            {
                                // Selects the ListBoxItem.
                                lbiFieldItem.IsSelected = true;
                                lstReportFields.SelectedItems.Add(lbiFieldItem);
                                break;
                            }
                        }
                    }

                    //Key to turn off Render report
                    intKeyForExport = 1;

                    // Manually calls the event handler to generate the report.
                    GenerateReportButtonHandler(null, null);



                    break;
                }


            }

            //Loop to insert data into excel file.
            for (int i = 0; i < GRGCurrentReport.Count; i++)
            {
                for (int j = 0; j < GRGCurrentReport[i].Length; j++)
                {
                    wsSheet.Cell(i + 1, j + 1).Value = GRGCurrentReport[i].GetValue(j).ToString();
                }
            }

            //Open save file dialog for saving data
            if (saveFileDialog.ShowDialog() == true)
                strfilepath = saveFileDialog.FileName;

            //Saving the workbook in the selected path
            wbWorkbook.SaveAs(strfilepath + ".xlsx");

            //Returns key to activate RenderReport Method
            intKeyForExport = 0;

        }

        public void DeleteReportButtonHandler(object sender, EventArgs e)
        {
            // Gets the report template from the button.
            Button btnSender = (Button)sender;
            ReportTemplate udtReportTemplate = (ReportTemplate)btnSender.Tag;

            using (DirectorPortalDatabase.DatabaseContext context = new DirectorPortalDatabase.DatabaseContext())
            {
                // Deletes all ReportFields belonging to the specified ReportType.
                ReportField[] rgReportFields = context.ReportFields.Where(x => x.GIntReportTemplateId == udtReportTemplate.GIntId).ToArray();
                context.RemoveRange(rgReportFields);

                // Deletes the ReportTemplate itself.
                context.Remove(udtReportTemplate);
                context.SaveChanges();
            }

            // Updates the local report template list to account for the deletion that occurred.
            GetReportTemplateList();
        }

        public GenerateReportsPage()
        {
            InitializeComponent();

            GRGCurrentReport = new List<string[]>();
            GRGReportTypeItems = new ComboBoxItem[ClsMetadataHelper.IntNumberOfModels];

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
                GRGReportTypeItems[i] = cbiModelItem;
            }

            cboReportType.ItemsSource = GRGReportTypeItems;

            GetReportTemplateList();
        }



    }

}







