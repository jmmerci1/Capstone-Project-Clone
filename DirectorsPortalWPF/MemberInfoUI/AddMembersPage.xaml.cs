using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorsPortalWPF.MemberInfoUI.MemberInfoViewModels;
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
        /// </summary>
        public AddMembersPage()
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
                newMailingAddress.GStrAddress = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrMailCity");
                newMailingAddress.GStrCity = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrMailState");
                newMailingAddress.GStrState = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntMailZipCode");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    newMailingAddress.GIntZipCode = int.Parse(((TextBox)txtInputBox).Text);
                }

                /* Create a new location address. */
                Address newLocationAddress = new Address();

                txtInputBox = FindName("StrLocationAddress");
                newLocationAddress.GStrAddress = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrLocCity");
                newLocationAddress.GStrCity = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("StrLocState");
                newLocationAddress.GStrState = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntLocZipCode");
                if (!(((TextBox)txtInputBox).Text.Equals(""))) 
                {
                    newLocationAddress.GIntZipCode = int.Parse(((TextBox)txtInputBox).Text);
                }

                /* Verify that the two new address are not the same. */
                if (newMailingAddress.Equals(newLocationAddress))
                {
                    if (!(newMailingAddress.GStrAddress.Equals(""))) 
                    {
                        context.Addresses.Add(newMailingAddress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    if (!(newMailingAddress.GStrAddress.Equals("")))
                    {
                        context.Addresses.Add(newMailingAddress);
                        context.SaveChanges();
                    }

                    if (!(newLocationAddress.GStrAddress.Equals("")))
                    {
                        context.Addresses.Add(newLocationAddress);
                        context.SaveChanges();

                    }
                }

                /* Create the new Business */
                Business newBusiness = new Business();

                txtInputBox = FindName("StrBuisnessName");
                newBusiness.GStrBusinessName = ((TextBox)txtInputBox).Text;

                txtInputBox = FindName("IntEstablishedYear");
                if (!(((TextBox)txtInputBox).Text.Equals("")))
                {
                    newBusiness.GIntYearEstablished = int.Parse(((TextBox)txtInputBox).Text);
                }

                txtInputBox = FindName("StrLevel");
                newBusiness.GEnumMembershipLevel = Business.GetMemberShipEnum(((TextBox)txtInputBox).Text);

                Address mailingAddress = context.Addresses
                    .Where(addr => addr.GStrAddress.Equals(newMailingAddress.GStrAddress))
                    .FirstOrDefault();

                if (mailingAddress != null) 
                {
                    newBusiness.GIntMailingAddressId = mailingAddress.GIntId;
                }

                Address locationAddress = context.Addresses
                    .Where(addr => addr.GStrAddress.Equals(newLocationAddress.GStrAddress))
                    .FirstOrDefault();

                if (locationAddress != null) 
                {
                    newBusiness.GIntPhysicalAddressId = locationAddress.GIntId;
                }

                txtInputBox = FindName("StrWebsite");
                newBusiness.GStrWebsite = ((TextBox)txtInputBox).Text;

                context.Businesses.Add(newBusiness);

                if (!(newBusiness.GStrBusinessName.Equals(""))) 
                {
                    context.SaveChanges();
                }

                /* Create the new Contact Person. */
                ContactPerson newContactPerson = new ContactPerson();

                txtInputBox = FindName("StrContactPerson");
                newContactPerson.GStrName = ((TextBox)txtInputBox).Text;

                context.ContactPeople.Add(newContactPerson);

                if (!(newContactPerson.GStrName.Equals(""))) 
                {
                    context.SaveChanges();

                    /* Contact operson related info is only created if a valid contact naem was given. */
                    /* Create the new Business Rep. */
                    BusinessRep newBusinessRep = new BusinessRep
                    {
                        GIntBusinessId = context.Businesses
                            .Where(bus => bus.GStrBusinessName.Equals(newBusiness.GStrBusinessName))
                            .FirstOrDefault().GIntId,
                        GIntContactPersonId = context.ContactPeople
                            .Where(per => per.GStrName.Equals(newContactPerson.GStrName))
                            .FirstOrDefault().GIntId
                    };

                    context.BusinessReps.Add(newBusinessRep);
                    context.SaveChanges();

                    /* Create the new Phone Numbers. */
                    /* TODO: No way to determine phone type in the current UI. */
                    PhoneNumber newPhoneNumber = new PhoneNumber();

                    txtInputBox = FindName("StrPhoneNumber");
                    newPhoneNumber.GStrPhoneNumber = ((TextBox)txtInputBox).Text;
                    newPhoneNumber.GIntContactPersonId = newBusinessRep.GIntContactPersonId;
                    newPhoneNumber.GEnumPhoneType = PhoneType.Office;

                    PhoneNumber newFaxNumber = new PhoneNumber();

                    txtInputBox = FindName("StrFaxNumber");
                    newFaxNumber.GStrPhoneNumber = ((TextBox)txtInputBox).Text;
                    newFaxNumber.GIntContactPersonId = newBusinessRep.GIntContactPersonId;
                    newFaxNumber.GEnumPhoneType = PhoneType.Fax;

                    context.PhoneNumbers.Add(newPhoneNumber);
                    context.PhoneNumbers.Add(newFaxNumber);
                    context.SaveChanges();

                    /* Create the new Email. */
                    Email newEmail = new Email();

                    txtInputBox = FindName("StrEmailAddress");
                    newEmail.GStrEmailAddress = ((TextBox)txtInputBox).Text;
                    newEmail.GIntContactPersonId = newBusinessRep.GIntContactPersonId;

                    context.Emails.Add(newEmail);
                    context.SaveChanges();
                }
            }

            NavigationService.Navigate(new MembersPage());
        }
    }
}
