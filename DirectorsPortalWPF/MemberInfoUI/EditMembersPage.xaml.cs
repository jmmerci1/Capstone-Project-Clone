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
    /// Interaction logic for EditMembersPage.xaml
    /// </summary>
    public partial class EditMembersPage : Page
    {
        public Business GSelectedBusiness = null;
        public int GIntContactCount = 0;
        public List<int> GIntContactsToRemove { get; set; } = new List<int>();

        /// <summary>
        /// A method for building the Edit members UI.
        /// </summary>
        /// <param name="selectedBusiness">Thye business from the selected row.</param>
        public EditMembersPage(Business selectedBusiness)
        {
            InitializeComponent();

            GSelectedBusiness = selectedBusiness;

            using (DatabaseContext context = new DatabaseContext()) 
            {
                try
                {
                    /* Populate the business info from the selected business. */
                    txtBusinessName.Text = selectedBusiness.BusinessName;
                    txtYearEst.Text = selectedBusiness.YearEstablished.ToString();
                    txtWebsite.Text = selectedBusiness.Website;
                    cboMemberLevel.SelectedIndex = (int)selectedBusiness.MembershipLevel;

                    /* Populate the addresses for the selected business. */
                    txtMailAddr.Text = selectedBusiness.MailingAddress?.StreetAddress;
                    txtMailCity.Text = selectedBusiness.MailingAddress?.City;
                    txtMailState.Text = selectedBusiness.MailingAddress?.State;
                    txtMailZip.Text = selectedBusiness.MailingAddress?.ZipCode.ToString();

                    if (selectedBusiness.MailingAddressId == selectedBusiness.PhysicalAddressId)
                    {
                        ChkLocationSameAsMailing.IsChecked = true;
                    }
                    else 
                    {
                        txtLocationAddr.Text = selectedBusiness.PhysicalAddress?.StreetAddress;
                        txtLocationCity.Text = selectedBusiness.PhysicalAddress?.City;
                        txtLocationState.Text = selectedBusiness.PhysicalAddress?.State;
                        txtLocationZip.Text = selectedBusiness.PhysicalAddress?.ToString();
                    }

                    /* Populate the contacts for the selected business. */
                    if (selectedBusiness.BusinessReps.Count > 0) 
                    {
                        /* Generate the UI elements for each contact. */
                        foreach (BusinessRep rep in selectedBusiness.BusinessReps) 
                        {
                            GIntContactCount++;

                            ContactInput CiContact = new ContactInput
                            {
                                GStrTitle = "Contact " + GIntContactCount + ":",
                                GIntContactId = rep.ContactPersonId
                            };
                            CiContact.TxtName.Text = rep.ContactPerson.Name;

                            /* Populate the emails for the contact. */
                            if (rep.ContactPerson.Emails.Count > 0) 
                            {
                                foreach (Email email in rep.ContactPerson.Emails) 
                                {
                                    CiContact.GntEmailCount++;

                                    EmailInput eiEmail = new EmailInput
                                    {
                                        GStrInputName = "Email " + CiContact.GntEmailCount + ":",
                                        GCiContactInputParent = CiContact,
                                        GIntEmailId = email.Id
                                    };
                                    eiEmail.TxtEmail.Text = email.EmailAddress;

                                    CiContact.SpContactEmails.Children.Add(eiEmail);
                                }
                            }

                            /* populate the numbers for the contact. */
                            if (rep.ContactPerson.PhoneNumbers.Count > 0)
                            {
                                foreach (PhoneNumber number in rep.ContactPerson.PhoneNumbers)
                                {
                                    CiContact.GIntNumberCount++;

                                    ContactNumberInput cniNumber = new ContactNumberInput
                                    {
                                        GStrInputName = "Number " + CiContact.GIntNumberCount + ":",
                                        GCiContactInputParent = CiContact,
                                        GIntNumberId = number.Id
                                    };
                                    cniNumber.TxtContactNumber.Text = number.Number;
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
                        GSelectedBusiness.BusinessName = txtBusinessName.Text;

                        int.TryParse(txtYearEst.Text, out int intYearEst);
                        GSelectedBusiness.YearEstablished = intYearEst;

                        GSelectedBusiness.Website = txtWebsite.Text;
                        GSelectedBusiness.MembershipLevel = (MembershipLevel)cboMemberLevel.SelectedIndex;

                        /* Update the mailing address from the form. */
                        GSelectedBusiness.MailingAddress.StreetAddress = txtMailAddr.Text;
                        GSelectedBusiness.MailingAddress.City = txtMailCity.Text;
                        GSelectedBusiness.MailingAddress.State = txtMailState.Text;

                        int.TryParse(txtMailZip.Text, out int intMailZipCode);
                        GSelectedBusiness.MailingAddress.ZipCode = intMailZipCode;

                        if (GSelectedBusiness.MailingAddressId == GSelectedBusiness.PhysicalAddressId &&
                            ChkLocationSameAsMailing.IsChecked == false)
                        {
                            /* The mailing address was the same as the location address, but now we want to add
                             * a new location address. */
                            Address newLocationAddress = new Address();
                            newLocationAddress.StreetAddress = txtLocationAddr.Text;
                            newLocationAddress.City = txtLocationCity.Text;
                            newLocationAddress.State = txtLocationState.Text;

                            int.TryParse(txtLocationZip.Text, out int intLocationZipCode);
                            newLocationAddress.ZipCode = intLocationZipCode;

                            context.Addresses.Add(newLocationAddress);

                            GSelectedBusiness.PhysicalAddress = newLocationAddress;
                        }
                        else if (GSelectedBusiness.MailingAddressId != GSelectedBusiness.PhysicalAddressId &&
                            ChkLocationSameAsMailing.IsChecked == true)
                        {
                            /* The location address was removed, delete it and update the business location ID 
                             * to the mailing ID. */
                            context.Addresses.Remove(GSelectedBusiness.PhysicalAddress);

                            GSelectedBusiness.PhysicalAddress = GSelectedBusiness.MailingAddress;
                        }
                        else if (GSelectedBusiness.MailingAddressId != GSelectedBusiness.PhysicalAddressId &&
                            ChkLocationSameAsMailing.IsChecked == false)
                        {
                            /* Update the location address with the one from the form. */
                            GSelectedBusiness.PhysicalAddress.StreetAddress = txtLocationAddr.Text;
                            GSelectedBusiness.PhysicalAddress.City = txtLocationCity.Text;
                            GSelectedBusiness.PhysicalAddress.State = txtLocationState.Text;

                            int.TryParse(txtLocationZip.Text, out int intLocationZipCode);
                            GSelectedBusiness.PhysicalAddress.ZipCode = intLocationZipCode;
                        }

                        /* Remove all contacts from the list. */
                        foreach (int id in GIntContactsToRemove) 
                        {
                            ContactPerson contactPersonToRemove = context.ContactPeople.Find(id);

                            context.Remove(contactPersonToRemove);
                        }

                        /* Update the contacts from the form. */

                        context.Update(GSelectedBusiness);
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
