﻿using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorsPortalWPF.Controls;
using DirectorsPortalWPF.MemberInfoUI.MemberInfoViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class ModifyMembersPage : Page
    {
        private Business GSelectedBusiness = null;
        private int GIntContactCount = 0;
        public List<int> GIntContactsToRemove { get; set; } = new List<int>();
        private bool GBoolIgnoreWarnings = false;

        /// <summary>
        /// A method for generating the add members UI.
        /// 
        /// Optional parameter will be used to populate this form with data pulled
        /// from a PDF
        /// </summary>
        public ModifyMembersPage([Optional] Dictionary<string, string> dictPdfImport, Business selectedBusiness)
        {
            InitializeComponent();

            if (selectedBusiness != null)
            {
                btnModifyMember.Content = "Update";
                btnModifyMember.Click += BtnUpdateMember_Click;

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
                            txtLocationZip.Text = selectedBusiness.PhysicalAddress?.ZipCode.ToString();
                        }

                        /* Populate the contacts for the selected business. */
                        if (selectedBusiness.BusinessReps != null
                            && selectedBusiness.BusinessReps.Count > 0)
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
                                if (rep.ContactPerson.Emails != null
                                    && rep.ContactPerson.Emails.Count > 0)
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
                                if (rep.ContactPerson.PhoneNumbers != null 
                                    && rep.ContactPerson.PhoneNumbers.Count > 0)
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
            else 
            {
                btnModifyMember.Content = "Add Member";
                btnModifyMember.Click += BtnAddMember_Click;
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
            if (!ValidateDataInForm(GBoolIgnoreWarnings)) 
            {
                /* Return early since the form has invalid data. */
                return;
            }

            using (DatabaseContext context = new DatabaseContext()) 
            {
                using (var transaction = context.Database.BeginTransaction()) 
                {
                    try
                    {
                        /* Get the business info from the form. */
                        Business newBusiness = new Business();
                        newBusiness.BusinessName = txtBusinessName.Text;

                        int.TryParse(txtYearEst.Text, out int intYearEst);
                        newBusiness.YearEstablished = intYearEst;

                        newBusiness.Website = txtWebsite.Text;
                        newBusiness.MembershipLevel = (MembershipLevel)cboMemberLevel.SelectedIndex;

                        /* Get the mailing address from the form. */
                        Address newMailingAddress = new Address();
                        newMailingAddress.StreetAddress = txtMailAddr.Text;
                        newMailingAddress.City = txtMailCity.Text;
                        newMailingAddress.State = txtMailState.Text;

                        int.TryParse(txtMailZip.Text, out int intMailZipCode);
                        newMailingAddress.ZipCode = intMailZipCode;

                        /* Get the location address from the form. */
                        Address newPhysicalAddress = new Address();
                        if (ChkLocationSameAsMailing.IsChecked == false)
                        {
                            newPhysicalAddress.StreetAddress = txtLocationAddr.Text;
                            newPhysicalAddress.City = txtLocationCity.Text;
                            newPhysicalAddress.State = txtLocationState.Text;

                            int.TryParse(txtLocationZip.Text, out int intLocationZipCode);
                            newPhysicalAddress.ZipCode = intLocationZipCode;
                        }
                        else
                        {
                            newPhysicalAddress = newMailingAddress;
                        }

                        if (!newMailingAddress.IsEmpty()) 
                        {
                            newBusiness.MailingAddress = newMailingAddress;
                        }
                        if (!newPhysicalAddress.IsEmpty()) 
                        {
                            newBusiness.PhysicalAddress = newPhysicalAddress;
                        }

                        /* Get the contacts from the form. */
                        newBusiness.BusinessReps = new List<BusinessRep>();
                        foreach (UIElement uiContact in SpContacts.Children)
                        {
                            if (uiContact is ContactInput)
                            {
                                ContactInput contactInput = (uiContact as ContactInput);
                                BusinessRep newBusinessRep = new BusinessRep();
                                ContactPerson newContact = new ContactPerson
                                {
                                    Name = contactInput.TxtName.Text,
                                    Emails = new List<Email>(),
                                    PhoneNumbers = new List<PhoneNumber>()
                                };

                                /* Get the contacts emails from the form. */
                                foreach (UIElement uiEmail in contactInput.SpContactEmails.Children)
                                {
                                    if (uiEmail is EmailInput)
                                    {
                                        EmailInput emailInput = (uiEmail as EmailInput);
                                        Email newEmail = new Email();

                                        newEmail.EmailAddress = emailInput.TxtEmail.Text;
                                        newContact.Emails.Add(newEmail);
                                    }
                                }

                                /* Get the contacts phone numbers from the form. */
                                foreach (UIElement uiPhoneNumber in contactInput.SpContactNumbers.Children)
                                {
                                    if (uiPhoneNumber is ContactNumberInput)
                                    {
                                        ContactNumberInput contactNumberInput = (uiPhoneNumber as ContactNumberInput);
                                        PhoneNumber newPhoneNumber = new PhoneNumber();

                                        newPhoneNumber.Number = contactNumberInput.TxtContactNumber.Text;
                                        newPhoneNumber.GEnumPhoneType = (PhoneType)contactNumberInput.CboNumberType.SelectedIndex;
                                        newContact.PhoneNumbers.Add(newPhoneNumber);
                                    }
                                }

                                newBusinessRep.ContactPerson = newContact;
                                newBusiness.BusinessReps.Add(newBusinessRep);
                            }
                        }

                        context.Businesses.Add(newBusiness);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception ex) 
                    {
                        transaction.Rollback();
                    }
                }
            }

            NavigationService.Navigate(new MembersPage());
        }

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

                        /* Update the contacts from the form. */
                        foreach (UIElement uiContact in SpContacts.Children)
                        {
                            if (uiContact is ContactInput)
                            {
                                ContactInput ciContact = uiContact as ContactInput;
                                BusinessRep currentRep = GSelectedBusiness.BusinessReps
                                    .FirstOrDefault(rep => rep.ContactPersonId == ciContact.GIntContactId);

                                /* Check if the contact was removed. */
                                if (GIntContactsToRemove.Contains(ciContact.GIntContactId))
                                {
                                    foreach (Email email in currentRep.ContactPerson.Emails)
                                    {
                                        context.Remove(email);
                                    }

                                    foreach (PhoneNumber number in currentRep.ContactPerson.PhoneNumbers)
                                    {
                                        context.Remove(number);
                                    }

                                    context.Remove(currentRep.ContactPerson);
                                    context.Remove(currentRep);
                                }
                                else
                                {
                                    /* Check if this is a new or updated contact. */
                                    if (ciContact.GIntContactId != -1)
                                    {
                                        currentRep.ContactPerson.Name = ciContact.TxtName.Text;

                                        /* Get the contact's emails from the form. */
                                        foreach (UIElement uiEmail in ciContact.SpContactEmails.Children)
                                        {
                                            if (uiEmail is EmailInput)
                                            {
                                                EmailInput eiEmail = uiEmail as EmailInput;
                                                Email currentEmail = currentRep.ContactPerson.Emails
                                                    .FirstOrDefault(email => email.Id == eiEmail.GIntEmailId);

                                                if (ciContact.GIntEmailsToRemove.Contains(eiEmail.GIntEmailId))
                                                {
                                                    context.Remove(currentEmail);
                                                }
                                                else if (eiEmail.GIntEmailId == -1)
                                                {
                                                    Email newEmail = new Email();
                                                    newEmail.EmailAddress = eiEmail.TxtEmail.Text;

                                                    context.Emails.Add(newEmail);

                                                    currentRep.ContactPerson.Emails.Add(newEmail);
                                                }
                                                else
                                                {
                                                    currentEmail.EmailAddress = eiEmail.TxtEmail.Text;
                                                }
                                            }
                                        }

                                        /* Get the contact's numbers from the form. */
                                        foreach (UIElement uiNumber in ciContact.SpContactNumbers.Children)
                                        {
                                            if (uiNumber is ContactNumberInput)
                                            {
                                                ContactNumberInput cniNumber = uiNumber as ContactNumberInput;
                                                PhoneNumber currentNumber = currentRep.ContactPerson.PhoneNumbers
                                                    .FirstOrDefault(number => number.Id == cniNumber.GIntNumberId);

                                                if (ciContact.GIntNumbersToRemove.Contains(cniNumber.GIntNumberId))
                                                {
                                                    context.Remove(currentNumber);
                                                }
                                                else if (cniNumber.GIntNumberId == -1)
                                                {
                                                    PhoneNumber newNumber = new PhoneNumber();
                                                    newNumber.Number = cniNumber.TxtContactNumber.Text;
                                                    newNumber.GEnumPhoneType = (PhoneType)cniNumber.CboNumberType.SelectedIndex;

                                                    context.PhoneNumbers.Add(newNumber);

                                                    currentRep.ContactPerson.PhoneNumbers.Add(newNumber);
                                                }
                                                else
                                                {
                                                    currentNumber.Number = cniNumber.TxtContactNumber.Text;
                                                    currentNumber.GEnumPhoneType = (PhoneType)cniNumber.CboNumberType.SelectedIndex;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        BusinessRep newRep = new BusinessRep();
                                        ContactPerson newContact = new ContactPerson
                                        {
                                            Name = ciContact.TxtName.Text,
                                            Emails = new List<Email>(),
                                            PhoneNumbers = new List<PhoneNumber>()
                                        };

                                        newRep.ContactPerson = newContact;

                                        /* Get the contacts emails from the form. */
                                        foreach (UIElement uiEmail in ciContact.SpContactEmails.Children)
                                        {
                                            if (uiEmail is EmailInput)
                                            {
                                                EmailInput emailInput = (uiEmail as EmailInput);
                                                Email email = new Email();

                                                email.EmailAddress = emailInput.TxtEmail.Text;
                                                newRep.ContactPerson.Emails.Add(email);
                                            }
                                        }

                                        /* Get the contacts phone numbers from the form. */
                                        foreach (UIElement uiPhoneNumber in ciContact.SpContactNumbers.Children)
                                        {
                                            if (uiPhoneNumber is ContactNumberInput)
                                            {
                                                ContactNumberInput contactNumberInput = (uiPhoneNumber as ContactNumberInput);
                                                PhoneNumber phoneNumber = new PhoneNumber();

                                                phoneNumber.Number = contactNumberInput.TxtContactNumber.Text;
                                                phoneNumber.GEnumPhoneType = (PhoneType)contactNumberInput.CboNumberType.SelectedIndex;
                                                newRep.ContactPerson.PhoneNumbers.Add(phoneNumber);
                                            }
                                        }

                                        context.BusinessReps.Add(newRep);
                                        GSelectedBusiness.BusinessReps.Add(newRep);
                                    }
                                }
                            }
                        }

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

        private bool ValidateDataInForm(bool boolIgnoreWarnings) 
        {
            string strWarningMessage = "The following fields are missing data:";
            bool boolAllDataIsValid = true;
            bool boolShowWarning = false;

            /* Verify the business info. 
             * Just make sure then name isn't blank and if there is a est. year make sure that it is
             * only numbers. */
            if (txtBusinessName.Text.Equals(""))
            {
                boolAllDataIsValid = false;

                txtBusinessName.BorderBrush = Brushes.Red;

                ToolTip ttBusinessName = new ToolTip();
                ttBusinessName.Content = "Please enter a business name.";

                txtBusinessName.ToolTip = ttBusinessName;
            }
            else 
            {
                ResetFormError(txtBusinessName);
            }

            if (!txtYearEst.Text.Equals("")
                && !int.TryParse(txtYearEst.Text, out int intYearEst))
            {
                boolAllDataIsValid = false;

                txtYearEst.BorderBrush = Brushes.Red;

                ToolTip ttYearEst = new ToolTip();
                ttYearEst.Content = "Year established must be a number.";

                txtYearEst.ToolTip = ttYearEst;
            }
            else
            {
                ResetFormError(txtYearEst);
            }

            /* Verify the mailing address.
             * Make sure that none of the fields are blank and zip code is a number. */
            if (txtMailAddr.Text.Equals(""))
            {
                boolShowWarning = true;

                txtMailAddr.BorderBrush = Brushes.Yellow;

                strWarningMessage += "\nMailing Address";
            }
            else 
            {
                ResetFormError(txtMailAddr);
            }

            if (txtMailCity.Text.Equals(""))
            {
                boolShowWarning = true;

                txtMailCity.BorderBrush = Brushes.Yellow;

                strWarningMessage += "\nMailing City";
            }
            else
            {
                ResetFormError(txtMailCity);
            }

            if (txtMailState.Text.Equals(""))
            {
                boolShowWarning = true;

                txtMailState.BorderBrush = Brushes.Yellow;

                strWarningMessage += "\nMailing State";
            }
            else
            {
                ResetFormError(txtMailState);
            }

            if (!txtMailZip.Text.Equals("")
                && !int.TryParse(txtMailZip.Text, out int intMailZip))
            {
                boolAllDataIsValid = false;

                txtMailZip.BorderBrush = Brushes.Red;

                ToolTip ttMailZip = new ToolTip();
                ttMailZip.Content = "Zip code must be a number.";

                txtMailZip.ToolTip = ttMailZip;
            }
            else if (txtMailZip.Text.Equals(""))
            {
                boolShowWarning = true;

                txtMailZip.BorderBrush = Brushes.Yellow;

                strWarningMessage += "\nMailing Zip";
            }
            else
            {
                ResetFormError(txtMailZip);
            }

            /* Verify the physical address.
             * Make sure that none of the fields are blank and zip code is a number. */
            if (ChkLocationSameAsMailing.IsChecked == false) 
            {
                if (txtLocationAddr.Text.Equals(""))
                {
                    boolShowWarning = true;

                    txtLocationAddr.BorderBrush = Brushes.Yellow;

                    strWarningMessage += "\nLocation Address";
                }
                else
                {
                    ResetFormError(txtLocationAddr);
                }

                if (txtLocationCity.Text.Equals(""))
                {
                    boolShowWarning = true;

                    txtLocationCity.BorderBrush = Brushes.Yellow;

                    strWarningMessage += "\nLocation City";
                }
                else
                {
                    ResetFormError(txtLocationCity);
                }

                if (txtLocationState.Text.Equals(""))
                {
                    boolShowWarning = true;

                    txtLocationState.BorderBrush = Brushes.Yellow;

                    strWarningMessage += "\nLocation State";
                }
                else
                {
                    ResetFormError(txtLocationState);
                }

                if (!txtLocationZip.Text.Equals("")
                    && !int.TryParse(txtLocationZip.Text, out int intLocationZip))
                {
                    boolAllDataIsValid = false;

                    txtLocationZip.BorderBrush = Brushes.Red;

                    ToolTip ttLocationZip = new ToolTip();
                    ttLocationZip.Content = "Zip code must be a number.";

                    txtLocationZip.ToolTip = ttLocationZip;
                }
                else if (txtLocationZip.Text.Equals(""))
                {
                    boolShowWarning = true;

                    txtLocationZip.BorderBrush = Brushes.Yellow;

                    strWarningMessage += "\nLocation Zip";
                }
                else
                {
                    ResetFormError(txtLocationZip);
                }
            }

            Regex regexEmail = new Regex(@"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b");
            Regex regexPhoneNumber = new Regex(@"^[2-9]\d{2}-\d{3}-\d{4}$");

            foreach (UIElement uiContact in SpContacts.Children)
            {
                if (uiContact is ContactInput)
                {
                    ContactInput contactInput = (uiContact as ContactInput);

                    if (contactInput.TxtName.Text.Equals(""))
                    {
                        boolAllDataIsValid = false;

                        contactInput.TxtName.BorderBrush = Brushes.Red;

                        ToolTip ttContactName = new ToolTip();
                        ttContactName.Content = "Contact name cannot be empty.";

                        contactInput.TxtName.ToolTip = ttContactName;
                    }
                    else 
                    {
                        ResetFormError(contactInput.TxtName);
                    }

                    /* Get the contacts emails from the form. */
                    foreach (UIElement uiEmail in contactInput.SpContactEmails.Children)
                    {
                        if (uiEmail is EmailInput)
                        {
                            EmailInput emailInput = (uiEmail as EmailInput);
                            string strEmail = emailInput.TxtEmail.Text.ToLower().Trim();

                            if (!regexEmail.IsMatch(strEmail))
                            {
                                boolAllDataIsValid = false;

                                emailInput.TxtEmail.BorderBrush = Brushes.Red;

                                ToolTip ttEmail = new ToolTip();
                                ttEmail.Content = "The entered address is not valid.";

                                emailInput.TxtEmail.ToolTip = ttEmail;
                            }
                            else
                            {
                                ResetFormError(emailInput.TxtEmail);
                            }
                        }
                    }

                    /* Get the contacts phone numbers from the form. */
                    foreach (UIElement uiPhoneNumber in contactInput.SpContactNumbers.Children)
                    {
                        if (uiPhoneNumber is ContactNumberInput)
                        {
                            ContactNumberInput contactNumberInput = (uiPhoneNumber as ContactNumberInput);
                            string strNumber = contactNumberInput.TxtContactNumber.Text.Trim();

                            if (!regexPhoneNumber.IsMatch(strNumber))
                            {
                                boolAllDataIsValid = false;

                                contactNumberInput.TxtContactNumber.BorderBrush = Brushes.Red;

                                ToolTip ttNumber = new ToolTip();
                                ttNumber.Content = "The entered phone number was not of the form xxx-xxx-xxxx.";

                                contactNumberInput.TxtContactNumber.ToolTip = ttNumber;
                            }
                            else 
                            {
                                ResetFormError(contactNumberInput.TxtContactNumber);
                            }
                        }
                    }
                }
            }

            if (boolShowWarning && !boolIgnoreWarnings) 
            {
                /* Warn the user there are blank fields that will not cause errors. */
                boolAllDataIsValid = false;
                CreateWarningMessageBox(strWarningMessage);
            }

            return boolAllDataIsValid;
        }

        private void ResetFormError(TextBox txtError) 
        {
            txtError.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            txtError.ToolTip = null;
        }

        private void CreateWarningMessageBox(string strMessage) 
        {
            strMessage += "\n\nWould you like to add the data anyways?";

            MessageBoxResult messageBoxResult = MessageBox.Show(strMessage, "Missing Data", MessageBoxButton.YesNo);
            switch (messageBoxResult) 
            {
                case MessageBoxResult.Yes:
                    /* Add the entered business */
                    GBoolIgnoreWarnings = true;
                    BtnAddMember_Click(messageBoxResult, null);
                    break;

                case MessageBoxResult.No:
                    /* Do nothing */
                    break;
            }
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
