using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorsPortalWPF.Controls;
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
        private int IntContactCount = 0;

        /// <summary>
        /// A method for generating the add members UI.
        /// </summary>
        public AddMembersPage()
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
            businessFromForm.GStrBusinessName = txtBusinessName.Text;

            int.TryParse(txtYearEst.Text, out int intYearEst);
            businessFromForm.GIntYearEstablished = intYearEst;

            businessFromForm.GStrWebsite = txtWebsite.Text;
            businessFromForm.GEnumMembershipLevel = (MembershipLevel)cboMemberLevel.SelectedIndex;

            /* Get the mailing address from the form. */
            Address mailingAddrFromForm = new Address();
            mailingAddrFromForm.GStrAddress = txtMailAddr.Text;
            mailingAddrFromForm.GStrCity = txtMailCity.Text;
            mailingAddrFromForm.GStrState = txtMailState.Text;

            int.TryParse(txtMailZip.Text, out int intMailZipCode);
            mailingAddrFromForm.GIntZipCode = intMailZipCode;

            /* Get the location address from the form. */
            Address locationAddressFromForm = new Address();
            if (ChkLocationSameAsMailing.IsChecked == false) 
            {
                locationAddressFromForm.GStrAddress = txtLocationAddr.Text;
                locationAddressFromForm.GStrCity = txtLocationCity.Text;
                locationAddressFromForm.GStrState = txtLocationState.Text;

                int.TryParse(txtLocationZip.Text, out int intLocationZipCode);
                locationAddressFromForm.GIntZipCode = intLocationZipCode;
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

                            email.GStrEmailAddress = emailInput.TxtEmail.Text;
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

                            phoneNumber.GStrPhoneNumber = contactNumberInput.TxtContactNumber.Text;
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
                        if (ChkLocationSameAsMailing.IsChecked == true 
                            && !mailingAddressFromFrom.GStrAddress.Equals(""))
                        {
                            /* Add only the mailing address to the database. */
                            context.Addresses.Add(mailingAddressFromFrom);
                            context.SaveChanges();

                            businessFromForm.GIntMailingAddressId = mailingAddressFromFrom.GIntId;
                            businessFromForm.GIntPhysicalAddressId = mailingAddressFromFrom.GIntId;
                        }
                        else if (ChkLocationSameAsMailing.IsChecked == false
                            && !mailingAddressFromFrom.GStrAddress.Equals("")
                            && !locationAddressFromForm.GStrAddress.Equals(""))
                        {
                            /* Add both addresses to the form. */
                            context.Addresses.Add(mailingAddressFromFrom);
                            context.Addresses.Add(locationAddressFromForm);
                            context.SaveChanges();

                            businessFromForm.GIntMailingAddressId = mailingAddressFromFrom.GIntId;
                            businessFromForm.GIntPhysicalAddressId = locationAddressFromForm.GIntId;
                        }

                        /* Verify the business info is valid.
                         * For the time being this means the business has a name. */
                        if (!businessFromForm.GStrBusinessName.Equals("")) 
                        {
                            /* Check for a valid membership level as null defaults to -1. */
                            if (businessFromForm.GEnumMembershipLevel < 0) 
                            {
                                businessFromForm.GEnumMembershipLevel = MembershipLevel.GOLD;
                            }

                            context.Businesses.Add(businessFromForm);
                            context.SaveChanges();
                        }

                        /* Verify the business contacts are valid.
                         * For the time being this means the business has a name. */
                        foreach (ContactModel contact in contactModelsFromForm) 
                        {
                            if (!contact.Name.Equals("")) 
                            {
                                ContactPerson newContact = new ContactPerson();
                                newContact.GStrName = contact.Name;

                                context.ContactPeople.Add(newContact);
                                context.SaveChanges();

                                BusinessRep newBusinessRep = new BusinessRep();
                                newBusinessRep.GIntBusinessId = businessFromForm.GIntId;
                                newBusinessRep.GIntContactPersonId = newContact.GIntId;

                                context.BusinessReps.Add(newBusinessRep);
                                context.SaveChanges();

                                /* Verify the contacts emails are valid. */
                                foreach (Email email in contact.Emails) 
                                {
                                    if (!email.GStrEmailAddress.Equals("")) 
                                    {
                                        email.GIntContactPersonId = newContact.GIntId;

                                        context.Emails.Add(email);
                                    }
                                }
                                context.SaveChanges();

                                /* Verify the contacts numbers are valid. */
                                foreach (PhoneNumber phoneNumber in contact.PhoneNumbers) 
                                {
                                    if (!phoneNumber.GStrPhoneNumber.Equals("")) 
                                    {
                                        /* Check for a valid phone type as null defaults to -1. */
                                        if (phoneNumber.GEnumPhoneType < 0) 
                                        {
                                            phoneNumber.GEnumPhoneType = PhoneType.Mobile;
                                        }

                                        phoneNumber.GIntContactPersonId = newContact.GIntId;

                                        context.PhoneNumbers.Add(phoneNumber);
                                    }
                                }
                                context.SaveChanges();
                            }
                        }

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
