using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using DirectorPortalDatabase.Models;
using Microsoft.EntityFrameworkCore;
using DirectorPortalDatabase.Utility;

namespace DirectorsPortalWPF.GenerateReportsUI
{
    /// <summary>
    /// Interaction logic for GenerateReportsPage.xaml
    /// </summary>
    public partial class GenerateReportsPage : Page
    {

        private ClsMetadataHelper.ClsModelInfo GUdtSelectedReportType { get; set; }
        private List<string[]> GRGCurrentReport { get; set; }

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
                RenderReport();

            }
        }

        public GenerateReportsPage()
        {
            InitializeComponent();

            GRGCurrentReport = new List<string[]>();
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

            cboReportType.ItemsSource = rgReportTypeItems;
        }


    }
}
