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
        public int GIntContactCount = 0;

        /// <summary>
        /// A method for building the Edit members UI.
        /// </summary>
        /// <param name="selectedBusiness">Thye business from the selected row.</param>
        public EditMembersPage(Business selectedBusiness)
        {
            InitializeComponent();

            using (DatabaseContext context = new DatabaseContext()) 
            {
                using (var transaction = context.Database.BeginTransaction()) 
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
                                    GStrTitle = "Contact " + GIntContactCount + ":"
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
                                            GVisRemovable = Visibility.Visible
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
                                            GVisRemovable = Visibility.Visible
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
