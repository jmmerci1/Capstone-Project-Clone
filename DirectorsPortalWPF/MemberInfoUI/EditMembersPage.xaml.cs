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
                    if ((dictPdfImport != null) && (!dictPdfImport[GDicHumanReadableDataFields[property.Name]].Equals("")))
                    {
                        txtFieldEntry.Text = dictPdfImport[GDicHumanReadableDataFields[property.Name]];
                        txtFieldEntry.BorderBrush = Brushes.Green;
                        txtFieldEntry.Background = Brushes.Green;
                        txtFieldEntry.FontWeight = FontWeights.Bold;
                        txtFieldEntry.Foreground = Brushes.White;
                    }
                    else
                    {
                        txtFieldEntry.Text = Business.GetMebershipLevelString((MembershipLevel)int.Parse(strFieldText));
                    }

                }
                else
                {
                    // Fill with whats from the Modify PDF as priority, otherwise fill from the dataModel.
                    if ((dictPdfImport != null) && (!dictPdfImport[GDicHumanReadableDataFields[property.Name]].Equals("")))
                    {
                        txtFieldEntry.Text = dictPdfImport[GDicHumanReadableDataFields[property.Name]];
                        if (!GDicHumanReadableDataFields[property.Name].Equals("Business Name"))
                        {
                            txtFieldEntry.BorderBrush = Brushes.Green;
                            txtFieldEntry.Background = Brushes.Green;
                            txtFieldEntry.FontWeight = FontWeights.Bold;
                            txtFieldEntry.Foreground = Brushes.White;
                        }
                           
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
                Business businessToUpdate = context.Businesses.FirstOrDefault(x => x.Id == GIntBusinessToEditId);

                /* Update the mailing address. */
                Address updatedMailingAddress = context.Addresses.FirstOrDefault(x => x.Id == businessToUpdate.MailingAddressId);

                object txtInputBox = FindName("StrMailingAddress");
                updatedMailingAddress.StreetAddress = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrMailCity");
                updatedMailingAddress.City = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrMailState");
                updatedMailingAddress.State = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntMailZipCode");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    updatedMailingAddress.ZipCode = int.Parse(((TextBox)txtInputBox).Text);
                }

                /* Update the location address */
                Address updatedLocationAddress = context.Addresses.FirstOrDefault(x => x.Id == businessToUpdate.PhysicalAddressId);

                txtInputBox = FindName("StrLocationAddress");
                updatedLocationAddress.StreetAddress = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrLocCity");
                updatedLocationAddress.City = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrLocState");
                updatedLocationAddress.State = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntLocZipCode");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    updatedLocationAddress.ZipCode = int.Parse(((TextBox)txtInputBox).Text);
                }

                /* Update the business info. */
                txtInputBox = FindName("StrBuisnessName");
                businessToUpdate.BusinessName = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntEstablishedYear");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    businessToUpdate.YearEstablished = int.Parse(((TextBox)txtInputBox).Text);
                }

                txtInputBox = FindName("StrLevel");
                businessToUpdate.MembershipLevel = Business.GetMemberShipEnum(((TextBox)txtInputBox).Text);

                txtInputBox = FindName("StrWebsite");
                businessToUpdate.Website = ((TextBox)txtInputBox).Text;

                /* Update the contact person. */
                BusinessRep businessRep = context.BusinessReps.FirstOrDefault(x => x.BusinessId == businessToUpdate.Id);
                ContactPerson updatedContactPerson = context.ContactPeople.FirstOrDefault(x => x.Id == businessRep.ContactPersonId);

                txtInputBox = FindName("StrContactPerson");
                updatedContactPerson.Name = ((TextBox)txtInputBox).Text;

                PhoneNumber updatedPhoneNumber = context.PhoneNumbers
                    .FirstOrDefault(x => x.ContactPersonId == updatedContactPerson.Id && x.GEnumPhoneType != PhoneType.Fax);

                txtInputBox = FindName("StrPhoneNumber");
                updatedPhoneNumber.Number = ((TextBox)txtInputBox).Text;

                PhoneNumber updatedFaxNumber = context.PhoneNumbers
                    .FirstOrDefault(x => x.ContactPersonId == updatedContactPerson.Id && x.GEnumPhoneType == PhoneType.Fax);

                txtInputBox = FindName("StrFaxNumber");
                updatedFaxNumber.Number = ((TextBox)txtInputBox).Text;

                Email updatedEmail = context.Emails.FirstOrDefault(x => x.ContactPersonId == updatedContactPerson.Id);

                txtInputBox = FindName("StrEmailAddress");
                updatedEmail.EmailAddress = ((TextBox)txtInputBox).Text;

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
                GIntBusinessToEditId = businessToConvert.Id;

                BusinessDataModel dataModel = new BusinessDataModel();
                dataModel.StrBuisnessName = businessToConvert.BusinessName;
                dataModel.StrWebsite = businessToConvert.Website;
                dataModel.StrLevel = businessToConvert.MembershipLevel.ToString("D");
                dataModel.IntEstablishedYear = businessToConvert.YearEstablished;

                Address mailingAddress = context.Addresses.FirstOrDefault(x => x.Id == businessToConvert.MailingAddressId);
                dataModel.StrMailingAddress = mailingAddress.StreetAddress;
                dataModel.StrMailCity = mailingAddress.City;
                dataModel.StrMailState = mailingAddress.State;
                dataModel.IntMailZipCode = mailingAddress.ZipCode;

                Address locationAddresss = context.Addresses.FirstOrDefault(x => x.Id == businessToConvert.PhysicalAddressId);
                dataModel.StrLocationAddress = locationAddresss.StreetAddress;
                dataModel.StrLocCity = locationAddresss.City;
                dataModel.StrLocState = locationAddresss.State;
                dataModel.IntLocZipCode = locationAddresss.ZipCode;

                BusinessRep businessRep = context.BusinessReps.FirstOrDefault(x => x.BusinessId == businessToConvert.Id);
                ContactPerson contactPerson = context.ContactPeople.FirstOrDefault(x => x.Id == businessRep.ContactPersonId);
                dataModel.StrContactPerson = contactPerson.Name;

                PhoneNumber phoneNumber = context.PhoneNumbers
                    .FirstOrDefault(x => x.ContactPersonId == contactPerson.Id && x.GEnumPhoneType != PhoneType.Fax);
                dataModel.StrPhoneNumber = phoneNumber.Number;

                PhoneNumber faxNumber = context.PhoneNumbers
                    .FirstOrDefault(x => x.ContactPersonId == contactPerson.Id && x.GEnumPhoneType == PhoneType.Fax);
                dataModel.StrFaxNumber = faxNumber.Number;

                Email email = context.Emails.FirstOrDefault(x => x.ContactPersonId == contactPerson.Id);
                dataModel.StrEmailAddress = email.EmailAddress;

                return dataModel;
            }
        }
    }
}
