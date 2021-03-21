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
    /// Interaction logic for EditMembersPage.xaml
    /// </summary>
    public partial class EditMembersPage : Page
    {
        public int GIntSelectedBusinessId = -1;
        public int GIntContactCount = 0;
        public List<int> GIntContactsToRemove { get; set; } = new List<int>();

        /// <summary>
        /// A method for building the Edit members UI.
        /// </summary>
        /// <param name="selectedBusiness">Thye business from the selected row.</param>
        public EditMembersPage(Business selectedBusiness)
        {
            InitializeComponent();

            GIntSelectedBusinessId = selectedBusiness.GIntId;

            using (DatabaseContext context = new DatabaseContext()) 
            {
                try
                {
                    /* Populate the business info from the selected business. */
                    txtBusinessName.Text = selectedBusiness.GStrBusinessName;
                    txtYearEst.Text = selectedBusiness.GIntYearEstablished.ToString();
                    txtWebsite.Text = selectedBusiness.GStrWebsite;
                    cboMemberLevel.SelectedIndex = (int)selectedBusiness.GEnumMembershipLevel;

                    /* Populate the addresses for the selected business. */
                    if (selectedBusiness.GIntMailingAddressId == selectedBusiness.GIntPhysicalAddressId)
                    {
                        ChkLocationSameAsMailing.IsChecked = true;

                        Address mailingAddress = context.Addresses.Find(selectedBusiness.GIntMailingAddressId);

                        txtMailAddr.Text = mailingAddress?.GStrAddress;
                        txtMailCity.Text = mailingAddress?.GStrCity;
                        txtMailState.Text = mailingAddress?.GStrState;
                        txtMailZip.Text = mailingAddress?.GIntZipCode.ToString();
                    }
                    else 
                    {
                        Address mailingAddress = context.Addresses.Find(selectedBusiness.GIntMailingAddressId);
                        Address locationAddress = context.Addresses.Find(selectedBusiness.GIntPhysicalAddressId);

                        txtMailAddr.Text = mailingAddress?.GStrAddress;
                        txtMailCity.Text = mailingAddress?.GStrCity;
                        txtMailState.Text = mailingAddress?.GStrState;
                        txtMailZip.Text = mailingAddress?.GIntZipCode.ToString();

                        txtLocationAddr.Text = locationAddress?.GStrAddress;
                        txtLocationCity.Text = locationAddress?.GStrCity;
                        txtLocationState.Text = locationAddress?.GStrState;
                        txtLocationZip.Text = locationAddress?.GIntZipCode.ToString();
                    }

                    /* Populate the contacts for the selected business. */
                    List<BusinessRep> businessReps = context.BusinessReps.Where(rep => rep.GIntBusinessId == selectedBusiness.GIntId).ToList();
                    List<ContactPerson> contacts = new List<ContactPerson>();

                    foreach (BusinessRep rep in businessReps) 
                    {
                        ContactPerson contact = context.ContactPeople.Find(rep.GIntContactPersonId);
                        contacts.Add(contact);
                    }

                    if (contacts.Count > 0) 
                    {
                        /* Generate the UI elements for each contact. */
                        foreach (ContactPerson contact in contacts) 
                        {
                            GIntContactCount++;

                            ContactInput CiContact = new ContactInput
                            {
                                GStrTitle = "Contact " + GIntContactCount + ":",
                                GIntContactId = contact.GIntId
                            };
                            CiContact.TxtName.Text = contact.GStrName;

                            /* Populate the emails for the contact. */
                            List<Email> emails = context.Emails.Where(email => email.GIntContactPersonId == contact.GIntId).ToList();
                            if (emails.Count > 0) 
                            {
                                foreach (Email email in emails) 
                                {
                                    CiContact.GntEmailCount++;

                                    EmailInput eiEmail = new EmailInput
                                    {
                                        GStrInputName = "Email " + CiContact.GntEmailCount + ":",
                                        GCiContactInputParent = CiContact,
                                        GIntEmailId = email.GIntId
                                    };
                                    eiEmail.TxtEmail.Text = email.GStrEmailAddress;

                                    CiContact.SpContactEmails.Children.Add(eiEmail);
                                }
                            }

                            /* populate the numbers for the contact. */
                            List<PhoneNumber> numbers = context.PhoneNumbers.Where(number => number.GIntContactPersonId == contact.GIntId).ToList();
                            if (numbers.Count > 0)
                            {
                                foreach (PhoneNumber number in numbers)
                                {
                                    CiContact.GIntNumberCount++;

                                    ContactNumberInput cniNumber = new ContactNumberInput
                                    {
                                        GStrInputName = "Number " + CiContact.GIntNumberCount + ":",
                                        GCiContactInputParent = CiContact,
                                        GIntNumberId = number.GIntId
                                    };
                                    cniNumber.TxtContactNumber.Text = number.GStrPhoneNumber;
                                    cniNumber.CboNumberType.SelectedIndex = (int)number.GEnumPhoneType;

                                    CiContact.SpContactNumbers.Children.Add(cniNumber);
                                }
                            }

                            SpContacts.Children.Add(CiContact);
                        }
                    }
                }
                catch (Exception ex) 
                {
                    
                }
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
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        /* Update the business info from the form. */
                        Business business = context.Businesses.Find(GIntSelectedBusinessId);
                        business.GStrBusinessName = txtBusinessName.Text;

                        int.TryParse(txtYearEst.Text, out int intYearEst);
                        business.GIntYearEstablished = intYearEst;

                        business.GStrWebsite = txtWebsite.Text;
                        business.GEnumMembershipLevel = (MembershipLevel)cboMemberLevel.SelectedIndex;

                        /* Update the mailing address from the form. */
                        Address mailingAddress = context.Addresses.Find(business.GIntMailingAddressId);
                        mailingAddress.GStrAddress = txtMailAddr.Text;
                        mailingAddress.GStrCity = txtMailCity.Text;
                        mailingAddress.GStrState = txtMailState.Text;

                        int.TryParse(txtMailZip.Text, out int intMailZipCode);
                        mailingAddress.GIntZipCode = intMailZipCode;

                        Address newLocationAddress = new Address();
                        if (business.GIntMailingAddressId == business.GIntPhysicalAddressId &&
                            ChkLocationSameAsMailing.IsChecked == false)
                        {
                            /* The mailing address was the same as the location address, but now we want to add
                             * a new location address. */
                            newLocationAddress.GStrAddress = txtLocationAddr.Text;
                            newLocationAddress.GStrCity = txtLocationCity.Text;
                            newLocationAddress.GStrState = txtLocationState.Text;

                            int.TryParse(txtLocationZip.Text, out int intLocationZipCode);
                            newLocationAddress.GIntZipCode = intLocationZipCode;

                            context.Addresses.Add(newLocationAddress);
                            context.SaveChanges();

                            business.GIntPhysicalAddressId = newLocationAddress.GIntId;
                        }
                        else if (business.GIntMailingAddressId != business.GIntPhysicalAddressId &&
                            ChkLocationSameAsMailing.IsChecked == true)
                        {
                            /* The location address was removed, delete it and update the business location ID 
                             * to the mailing ID. */
                            Address locationAddress = context.Addresses.Find(business.GIntPhysicalAddressId);
                            context.Addresses.Remove(locationAddress);

                            business.GIntPhysicalAddressId = mailingAddress.GIntId;
                        }
                        else if (business.GIntMailingAddressId != business.GIntPhysicalAddressId &&
                            ChkLocationSameAsMailing.IsChecked == false)
                        {
                            /* Update the location address with the one from the form. */
                            Address locationAddress = context.Addresses.Find(business.GIntPhysicalAddressId);
                            locationAddress.GStrAddress = txtLocationAddr.Text;
                            locationAddress.GStrCity = txtLocationCity.Text;
                            locationAddress.GStrState = txtLocationState.Text;

                            int.TryParse(txtLocationZip.Text, out int intLocationZipCode);
                            locationAddress.GIntZipCode = intLocationZipCode;
                        }

                        /* Remove all contacts from the list. */
                        foreach (int id in GIntContactsToRemove) 
                        {
                            ContactPerson contactPersonToRemove = context.ContactPeople.Find(id);

                            context.Remove(contactPersonToRemove);
                        }

                        /* Update the contacts from the form. */
                        

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            NavigationService.Navigate(new MembersPage());
        }

        private void BtnAddContact_Click(object sender, RoutedEventArgs e)
        {
            GIntContactCount++;

            ContactInput CiContact = new ContactInput
            {
                GStrTitle = "Contact " + GIntContactCount + ":"
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
}
