using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorsPortalWPF.MemberInfoUI.MemberInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace DirectorsPortalWPF.MemberInfoUI
{
    /// <summary>
    /// Interaction logic for EditMembersPage.xaml
    /// </summary>
    public partial class EditMembersPage : Page
    {
        Dictionary<string, string> GDicHumanReadableDataFields = new Dictionary<string, string>();
        int GIntBusinessToEditId = 0;

        /// <summary>
        /// A method for building the Edit members UI.
        /// </summary>
        /// <param name="rowViewModel">A data model representing the business the user wants to edit.</param>
        public EditMembersPage(Business rowViewModel, [Optional] Dictionary<string, string> dictPdfImport)
        {
            InitializeComponent();

            BusinessDataModel dataModel = ConvertBusinessToDataModel(rowViewModel);

            GDicHumanReadableDataFields.Clear();
            GDicHumanReadableDataFields = BusinessDataModel.PopulateHumanReadableDataDic();

            int intRowCounter = 0;

            /* Create the entry fields. */
            Type typeDataViewModel = typeof(BusinessDataModel);
            foreach (var property in typeDataViewModel.GetProperties())
            {
                if (property.Name.Equals("StrMailingAddress") ||
                    property.Name.Equals("StrLocationAddress") ||
                    property.Name.Equals("StrContactPerson"))
                {
                    /* These properties define a new section, so make a heading for them. */
                    RowDefinition rdefSectionHeaderRow = new RowDefinition();
                    rdefSectionHeaderRow.Height = GridLength.Auto;

                    grdForm.RowDefinitions.Add(rdefSectionHeaderRow);

                    TextBlock txtHeader = new TextBlock();
                    txtHeader.TextDecorations = TextDecorations.Underline;
                    txtHeader.FontWeight = FontWeights.Bold;
                    txtHeader.Text = GDicHumanReadableDataFields[property.Name];

                    Label lblHeaderName = new Label();
                    lblHeaderName.Content = txtHeader;
                    lblHeaderName.Margin = new Thickness(0, 0, 0, 5);

                    Grid.SetColumn(lblHeaderName, 0);
                    Grid.SetRow(lblHeaderName, intRowCounter);
                    grdForm.Children.Add(lblHeaderName);

                    intRowCounter++;
                }

                RowDefinition rdefFieldRow = new RowDefinition();
                rdefFieldRow.Height = GridLength.Auto;

                grdForm.RowDefinitions.Add(rdefFieldRow);

                Label lblFieldName = new Label();
                lblFieldName.Content = GDicHumanReadableDataFields[property.Name] + ":";
                lblFieldName.Margin = new Thickness(0, 0, 0, 5);

                Grid.SetColumn(lblFieldName, 0);
                Grid.SetRow(lblFieldName, intRowCounter);
                grdForm.Children.Add(lblFieldName);

                TextBox txtFieldEntry = new TextBox();
                txtFieldEntry.Margin = new Thickness(0, 0, 0, 5);
                txtFieldEntry.Padding = new Thickness(3);

                /* If membership level, convert the enum to a readable string. */
                string strFieldText = dataModel.GetType().GetProperty(property.Name).GetValue(dataModel)?.ToString();
                if (property.Name == "StrLevel")
                {
                    
                    // Fill with whats from the Modify PDF as priority, otherwise fill from the dataModel.
                    if (!dictPdfImport[GDicHumanReadableDataFields[property.Name]].Equals(""))
                    {
                        txtFieldEntry.Text = dictPdfImport[GDicHumanReadableDataFields[property.Name]];
                        txtFieldEntry.BorderBrush = Brushes.Green;
                    }
                    else
                    {
                        txtFieldEntry.Text = Business.GetMebershipLevelString((MembershipLevel)int.Parse(strFieldText));
                    }

                }
                else
                {
                    // Fill with whats from the Modify PDF as priority, otherwise fill from the dataModel.
                    if (!dictPdfImport[GDicHumanReadableDataFields[property.Name]].Equals(""))
                    {
                        txtFieldEntry.Text = dictPdfImport[GDicHumanReadableDataFields[property.Name]];
                        if (!GDicHumanReadableDataFields[property.Name].Equals("Business Name"))
                            txtFieldEntry.BorderBrush = Brushes.Green;
                    }
                    else
                    {
                        txtFieldEntry.Text = strFieldText;
                    }


                }

                RegisterName(property.Name, txtFieldEntry);

                Grid.SetColumn(txtFieldEntry, 1);
                Grid.SetRow(txtFieldEntry, intRowCounter);
                grdForm.Children.Add(txtFieldEntry);

                intRowCounter++;
            }
        }

        /// <summary>
        /// A method for canceling the edit to a business and taking the user back to the member info page.
        /// </summary>
        /// <param name="sender">The object that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MembersPage());
        }

        /// <summary>
        /// A method for updating the business with the information entered by the user.
        /// </summary>
        /// <param name="sender">The object that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void BtnUpdateMember_Click(object sender, RoutedEventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext()) 
            {
                Business businessToUpdate = context.Businesses.FirstOrDefault(x => x.GIntId == GIntBusinessToEditId);

                /* Update the mailing address. */
                Address updatedMailingAddress = context.Addresses.FirstOrDefault(x => x.GIntId == businessToUpdate.GIntMailingAddressId);

                object txtInputBox = FindName("StrMailingAddress");
                updatedMailingAddress.GStrAddress = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrMailCity");
                updatedMailingAddress.GStrCity = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrMailState");
                updatedMailingAddress.GStrState = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntMailZipCode");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    updatedMailingAddress.GIntZipCode = int.Parse(((TextBox)txtInputBox).Text);
                }

                /* Update the location address */
                Address updatedLocationAddress = context.Addresses.FirstOrDefault(x => x.GIntId == businessToUpdate.GIntPhysicalAddressId);

                txtInputBox = FindName("StrLocationAddress");
                updatedLocationAddress.GStrAddress = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrLocCity");
                updatedLocationAddress.GStrCity = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrLocState");
                updatedLocationAddress.GStrState = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntLocZipCode");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    updatedLocationAddress.GIntZipCode = int.Parse(((TextBox)txtInputBox).Text);
                }

                /* Update the business info. */
                txtInputBox = FindName("StrBuisnessName");
                businessToUpdate.GStrBusinessName = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntEstablishedYear");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    businessToUpdate.GIntYearEstablished = int.Parse(((TextBox)txtInputBox).Text);
                }

                txtInputBox = FindName("StrLevel");
                businessToUpdate.GEnumMembershipLevel = Business.GetMemberShipEnum(((TextBox)txtInputBox).Text);

                txtInputBox = FindName("StrWebsite");
                businessToUpdate.GStrWebsite = ((TextBox)txtInputBox).Text;

                /* Update the contact person. */
                BusinessRep businessRep = context.BusinessReps.FirstOrDefault(x => x.GIntBusinessId == businessToUpdate.GIntId);
                ContactPerson updatedContactPerson = context.ContactPeople.FirstOrDefault(x => x.GIntId == businessRep.GIntContactPersonId);

                txtInputBox = FindName("StrContactPerson");
                updatedContactPerson.GStrName = ((TextBox)txtInputBox).Text;

                PhoneNumber updatedPhoneNumber = context.PhoneNumbers
                    .FirstOrDefault(x => x.GIntContactPersonId == updatedContactPerson.GIntId && x.GEnumPhoneType != PhoneType.Fax);

                txtInputBox = FindName("StrPhoneNumber");
                updatedPhoneNumber.GStrPhoneNumber = ((TextBox)txtInputBox).Text;

                PhoneNumber updatedFaxNumber = context.PhoneNumbers
                    .FirstOrDefault(x => x.GIntContactPersonId == updatedContactPerson.GIntId && x.GEnumPhoneType == PhoneType.Fax);

                txtInputBox = FindName("StrFaxNumber");
                updatedFaxNumber.GStrPhoneNumber = ((TextBox)txtInputBox).Text;

                Email updatedEmail = context.Emails.FirstOrDefault(x => x.GIntContactPersonId == updatedContactPerson.GIntId);

                txtInputBox = FindName("StrEmailAddress");
                updatedEmail.GStrEmailAddress = ((TextBox)txtInputBox).Text;

                context.SaveChanges();
            }

            NavigationService.Navigate(new MembersPage());
        }

        /// <summary>
        /// A method for converting a business to a business display model. This makes it possible to
        /// populate the UI with the selected business to edit.
        /// </summary>
        /// <param name="businessToConvert"></param>
        /// <returns></returns>
        private BusinessDataModel ConvertBusinessToDataModel(Business businessToConvert) 
        {
            using (DatabaseContext context = new DatabaseContext()) 
            {
                GIntBusinessToEditId = businessToConvert.GIntId;

                BusinessDataModel dataModel = new BusinessDataModel();
                dataModel.StrBuisnessName = businessToConvert.GStrBusinessName;
                dataModel.StrWebsite = businessToConvert.GStrWebsite;
                dataModel.StrLevel = businessToConvert.GEnumMembershipLevel.ToString("D");
                dataModel.IntEstablishedYear = businessToConvert.GIntYearEstablished;

                Address mailingAddress = context.Addresses.FirstOrDefault(x => x.GIntId == businessToConvert.GIntMailingAddressId);
                dataModel.StrMailingAddress = mailingAddress.GStrAddress;
                dataModel.StrMailCity = mailingAddress.GStrCity;
                dataModel.StrMailState = mailingAddress.GStrState;
                dataModel.IntMailZipCode = mailingAddress.GIntZipCode;

                Address locationAddresss = context.Addresses.FirstOrDefault(x => x.GIntId == businessToConvert.GIntPhysicalAddressId);
                dataModel.StrLocationAddress = locationAddresss.GStrAddress;
                dataModel.StrLocCity = locationAddresss.GStrCity;
                dataModel.StrLocState = locationAddresss.GStrState;
                dataModel.IntLocZipCode = locationAddresss.GIntZipCode;

                BusinessRep businessRep = context.BusinessReps.FirstOrDefault(x => x.GIntBusinessId == businessToConvert.GIntId);
                ContactPerson contactPerson = context.ContactPeople.FirstOrDefault(x => x.GIntId == businessRep.GIntContactPersonId);
                dataModel.StrContactPerson = contactPerson.GStrName;

                PhoneNumber phoneNumber = context.PhoneNumbers
                    .FirstOrDefault(x => x.GIntContactPersonId == contactPerson.GIntId && x.GEnumPhoneType != PhoneType.Fax);
                dataModel.StrPhoneNumber = phoneNumber.GStrPhoneNumber;

                PhoneNumber faxNumber = context.PhoneNumbers
                    .FirstOrDefault(x => x.GIntContactPersonId == contactPerson.GIntId && x.GEnumPhoneType == PhoneType.Fax);
                dataModel.StrFaxNumber = faxNumber.GStrPhoneNumber;

                Email email = context.Emails.FirstOrDefault(x => x.GIntContactPersonId == contactPerson.GIntId);
                dataModel.StrEmailAddress = email.GStrEmailAddress;

                return dataModel;
            }
        }
    }
}
