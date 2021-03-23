using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorsPortalWPF.Controls;
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
        private int IntContactCount = 0;

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
            /* Get the business info from the form. */
            Business businessFromForm = new Business();
            businessFromForm.BusinessName = txtBusinessName.Text;

            int.TryParse(txtYearEst.Text, out int intYearEst);
            businessFromForm.YearEstablished = intYearEst;

            businessFromForm.Website = txtWebsite.Text;
            businessFromForm.MembershipLevel = (MembershipLevel)cboMemberLevel.SelectedIndex;

            /* Get the mailing address from the form. */
            Address mailingAddrFromForm = new Address();
            mailingAddrFromForm.StreetAddress = txtMailAddr.Text;
            mailingAddrFromForm.City = txtMailCity.Text;
            mailingAddrFromForm.State = txtMailState.Text;

            int.TryParse(txtMailZip.Text, out int intMailZipCode);
            mailingAddrFromForm.ZipCode = intMailZipCode;

            /* Get the location address from the form. */
            Address locationAddressFromForm = new Address();
            if (ChkLocationSameAsMailing.IsChecked == false)
            {
                locationAddressFromForm.StreetAddress = txtLocationAddr.Text;
                locationAddressFromForm.City = txtLocationCity.Text;
                locationAddressFromForm.State = txtLocationState.Text;

                int.TryParse(txtLocationZip.Text, out int intLocationZipCode);
                locationAddressFromForm.ZipCode = intLocationZipCode;
            }
            else 
            {
                locationAddressFromForm = mailingAddrFromForm;
            }

            /* Get the contacts from the form. */
            List<ContactModel> contactsFromForm = new List<ContactModel>();
            foreach (UIElement uiContact in SpContacts.Children) 
            {
                if (uiContact is ContactInput) 
                {
                    ContactInput contactInput = (uiContact as ContactInput);
                    ContactModel contactModel = new ContactModel();

                    contactModel.Name = contactInput.TxtName.Text;

                    /* Get the contacts emails from the form. */
                    foreach (UIElement uiEmail in contactInput.SpContactEmails.Children) 
                    {
                        if (uiEmail is EmailInput) 
                        {
                            EmailInput emailInput = (uiEmail as EmailInput);
                            Email email = new Email();

                            email.EmailAddress = emailInput.TxtEmail.Text;
                            contactModel.Emails.Add(email);
                        }
                    }

                    /* Get the contacts phone numbers from the form. */
                    foreach (UIElement uiPhoneNumber in contactInput.SpContactNumbers.Children) 
                    {
                        if (uiPhoneNumber is ContactNumberInput) 
                        {
                            ContactNumberInput contactNumberInput = (uiPhoneNumber as ContactNumberInput);
                            PhoneNumber phoneNumber = new PhoneNumber();

                            phoneNumber.Number = contactNumberInput.TxtContactNumber.Text;
                            phoneNumber.GEnumPhoneType = (PhoneType)contactNumberInput.CboNumberType.SelectedIndex;
                            contactModel.PhoneNumbers.Add(phoneNumber);
                        }
                    }

                    contactsFromForm.Add(contactModel);
                }
            }

            /* We have all the data from the enetered contact, now verify it. */
            VerifyFormData(businessFromForm, mailingAddrFromForm, locationAddressFromForm, contactsFromForm);

            NavigationService.Navigate(new MembersPage());
        }

        private void VerifyFormData(Business businessFromForm, Address mailingAddressFromFrom,
            Address locationAddressFromForm, List<ContactModel> contactModelsFromForm) 
        {
            using (DatabaseContext context = new DatabaseContext()) 
            {
                using (var transaction = context.Database.BeginTransaction()) 
                {
                    try
                    {
                        /* Verify the address are valid.
                         * For the time being this just means a single street address. */
                        if (!mailingAddressFromFrom.StreetAddress.Equals("")
                            && !locationAddressFromForm.StreetAddress.Equals("")) 
                        {
                            businessFromForm.MailingAddress = mailingAddressFromFrom;
                            businessFromForm.PhysicalAddress = locationAddressFromForm;
                        }

                        /* Verify the business info is valid.
                         * For the time being this means the business has a name. */
                        if (!businessFromForm.BusinessName.Equals("")) 
                        {
                            /* Check for a valid membership level as null defaults to -1. */
                            if (businessFromForm.MembershipLevel < 0) 
                            {
                                businessFromForm.MembershipLevel = MembershipLevel.GOLD;
                            }
                        }
                        businessFromForm.BusinessReps = new List<BusinessRep>();

                        /* Verify the business contacts are valid.
                         * For the time being this means the business has a name. */
                        foreach (ContactModel contact in contactModelsFromForm) 
                        {
                            if (!contact.Name.Equals("")) 
                            {
                                ContactPerson newContact = new ContactPerson
                                {
                                    Name = contact.Name,
                                    Emails = new List<Email>(),
                                    PhoneNumbers = new List<PhoneNumber>()
                                };

                                BusinessRep newBusinessRep = new BusinessRep
                                {
                                    Business = businessFromForm,
                                    ContactPerson = newContact
                                };

                                businessFromForm.BusinessReps.Add(newBusinessRep);

                                /* Verify the contacts emails are valid. */
                                foreach (Email email in contact.Emails) 
                                {
                                    if (!email.EmailAddress.Equals("")) 
                                    {
                                        newContact.Emails.Add(email);
                                    }
                                }

                                /* Verify the contacts numbers are valid. */
                                foreach (PhoneNumber phoneNumber in contact.PhoneNumbers) 
                                {
                                    if (!phoneNumber.Number.Equals("")) 
                                    {
                                        /* Check for a valid phone type as null defaults to -1. */
                                        if (phoneNumber.GEnumPhoneType < 0) 
                                        {
                                            phoneNumber.GEnumPhoneType = PhoneType.Mobile;
                                        }

                                        newContact.PhoneNumbers.Add(phoneNumber);
                                    }
                                }
                            }
                        }

                        context.Businesses.Add(businessFromForm);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception ex) 
                    {
                        /* Log any exceptions? */
                        transaction.Rollback();
                    }
                }
            }
        }

        private void BtnAddContact_Click(object sender, RoutedEventArgs e)
        {
            IntContactCount++;

            ContactInput CiContact = new ContactInput
            {
                GStrTitle = "Contact " + IntContactCount + ":"
            };

            SpContacts.Children.Add(CiContact);
        }

        private void ChkLocationSameAsMailing_Checked(object sender, RoutedEventArgs e)
        {
            SpLocationAddress.IsEnabled = false;
        }

        private void ChkLocationSameAsMailing_Unchecked(object sender, RoutedEventArgs e)
        {
            SpLocationAddress.IsEnabled = true;
        }
    }

    public class ContactModel {
        public string Name { get; set; }
        public List<Email> Emails { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }

        public ContactModel() 
        {
            Emails = new List<Email>();
            PhoneNumbers = new List<PhoneNumber>();
        }


    }
}
