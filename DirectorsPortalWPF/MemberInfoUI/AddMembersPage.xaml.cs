using DirectorPortalDatabase;
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
            if (!ValidateDataInForm()) 
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

        private bool ValidateDataInForm() 
        {
            bool boolAllDataIsValid = true;

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

            return boolAllDataIsValid;
        }

        private void ResetFormError(TextBox txtError) 
        {
            txtError.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            txtError.ToolTip = null;
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
}
