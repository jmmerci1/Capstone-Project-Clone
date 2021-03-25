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
    /// Interaction logic for EditAddMembersPage.xaml
    /// </summary>
    public partial class AddMembersPage : Page
    {
        Dictionary<string, string> GDicHumanReadableDataFields = new Dictionary<string, string>();

        /// <summary>
        /// A method for generating the add members UI.
        /// 
        /// Optional parameter will be used to populate this form with data pulled
        /// from a PDF
        /// </summary>
        public AddMembersPage([Optional] Dictionary<string, string> dictPdfImport)
        {
            InitializeComponent();

            GDicHumanReadableDataFields.Clear();
            GDicHumanReadableDataFields = BusinessDataModel.PopulateHumanReadableDataDic();

            int intRowCounter = 0;

            /* Create the entry fields. */
            Type typeTableViewModel = typeof(BusinessDataModel);
            foreach (var property in typeTableViewModel.GetProperties())
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
                RegisterName(property.Name, txtFieldEntry);

                // TODO
                if (GDicHumanReadableDataFields.TryGetValue(property.Name,out string strHumanReadable) && dictPdfImport != null && dictPdfImport.TryGetValue(strHumanReadable, out string strPdfValue))
               //if (dictPdfImport[GDicHumanReadableDataFields[property.Name]] != null)
                  txtFieldEntry.Text = strPdfValue;

                Grid.SetColumn(txtFieldEntry, 1);
                Grid.SetRow(txtFieldEntry, intRowCounter);
                grdForm.Children.Add(txtFieldEntry);

                intRowCounter++;
            }
        }

        /// <summary>
        /// A method for canceling add business action and taking the user back to the member info page.
        /// </summary>
        /// <param name="sender">The object that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MembersPage());
        }

        /// <summary>
        /// A method for converting the view model to a new business and writing it to the database.
        /// </summary>
        /// <param name="sender">The object that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void BtnAddMember_Click(object sender, RoutedEventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                /* Create a new mailing address. */
                Address newMailingAddress = new Address();

                object txtInputBox = FindName("StrMailingAddress");
                newMailingAddress.StreetAddress = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrMailCity");
                newMailingAddress.City = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrMailState");
                newMailingAddress.State = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntMailZipCode");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    newMailingAddress.ZipCode = int.Parse(((TextBox)txtInputBox).Text);
                }

                /* Create a new location address. */
                Address newLocationAddress = new Address();

                txtInputBox = FindName("StrLocationAddress");
                newLocationAddress.StreetAddress = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrLocCity");
                newLocationAddress.City = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrLocState");
                newLocationAddress.State = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntLocZipCode");
                if (!(((TextBox)txtInputBox).Text.Equals(""))) 
                {
                    newLocationAddress.ZipCode = int.Parse(((TextBox)txtInputBox).Text);
                }

                /* Verify that the two new address are not the same. */
                if (newMailingAddress.Equals(newLocationAddress))
                {
                    if (!(newMailingAddress.StreetAddress.Equals(""))) 
                    {
                        context.Addresses.Add(newMailingAddress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    if (!(newMailingAddress.StreetAddress.Equals("")))
                    {
                        context.Addresses.Add(newMailingAddress);
                        context.SaveChanges();
                    }

                    if (!(newLocationAddress.StreetAddress.Equals("")))
                    {
                        context.Addresses.Add(newLocationAddress);
                        context.SaveChanges();

                    }
                }

                /* Create the new Business */
                Business newBusiness = new Business();

                txtInputBox = FindName("StrBuisnessName");
                newBusiness.BusinessName = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntEstablishedYear");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    newBusiness.YearEstablished = int.Parse(((TextBox)txtInputBox).Text);
                }

                txtInputBox = FindName("StrLevel");
                newBusiness.MembershipLevel = Business.GetMemberShipEnum(((TextBox)txtInputBox).Text);

                Address mailingAddress = context.Addresses
                    .Where(addr => addr.StreetAddress.Equals(newMailingAddress.StreetAddress))
                    .FirstOrDefault();

                if (mailingAddress != null) 
                {
                    newBusiness.MailingAddressId = mailingAddress.Id;
                }

                Address locationAddress = context.Addresses
                    .Where(addr => addr.StreetAddress.Equals(newLocationAddress.StreetAddress))
                    .FirstOrDefault();

                if (locationAddress != null) 
                {
                    newBusiness.PhysicalAddressId = locationAddress.Id;
                }

                txtInputBox = FindName("StrWebsite");
                newBusiness.Website = ((TextBox)txtInputBox).Text;

                context.Businesses.Add(newBusiness);

                if (!(newBusiness.BusinessName.Equals(""))) 
                {
                    context.SaveChanges();
                }

                /* Create the new Contact Person. */
                ContactPerson newContactPerson = new ContactPerson();

                txtInputBox = FindName("StrContactPerson");
                newContactPerson.Name = ((TextBox)txtInputBox).Text;

                context.ContactPeople.Add(newContactPerson);

                if (!(newContactPerson.Name.Equals(""))) 
                {
                    context.SaveChanges();

                    /* Contact operson related info is only created if a valid contact naem was given. */
                    /* Create the new Business Rep. */
                    BusinessRep newBusinessRep = new BusinessRep
                    {
                        BusinessId = context.Businesses
                            .Where(bus => bus.BusinessName.Equals(newBusiness.BusinessName))
                            .FirstOrDefault().Id,
                        ContactPersonId = context.ContactPeople
                            .Where(per => per.Name.Equals(newContactPerson.Name))
                            .FirstOrDefault().Id
                    };

                    context.BusinessReps.Add(newBusinessRep);
                    context.SaveChanges();

                    /* Create the new Phone Numbers. */
                    /* TODO: No way to determine phone type in the current UI. */
                    PhoneNumber newPhoneNumber = new PhoneNumber();

                    txtInputBox = FindName("StrPhoneNumber");
                    newPhoneNumber.Number = ((TextBox)txtInputBox).Text;
                    newPhoneNumber.ContactPersonId = newBusinessRep.ContactPersonId;
                    newPhoneNumber.GEnumPhoneType = PhoneType.Office;

                    PhoneNumber newFaxNumber = new PhoneNumber();

                    txtInputBox = FindName("StrFaxNumber");
                    newFaxNumber.Number = ((TextBox)txtInputBox).Text;
                    newFaxNumber.ContactPersonId = newBusinessRep.ContactPersonId;
                    newFaxNumber.GEnumPhoneType = PhoneType.Fax;

                    context.PhoneNumbers.Add(newPhoneNumber);
                    context.PhoneNumbers.Add(newFaxNumber);
                    context.SaveChanges();

                    /* Create the new Email. */
                    Email newEmail = new Email();

                    txtInputBox = FindName("StrEmailAddress");
                    newEmail.EmailAddress = ((TextBox)txtInputBox).Text;
                    newEmail.ContactPersonId = newBusinessRep.ContactPersonId;

                    context.Emails.Add(newEmail);
                    context.SaveChanges();
                }
            }

            NavigationService.Navigate(new MembersPage());
        }


    }
}
