using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorPortalDatabase.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace DirectorsPortalWPF.MemberInfoUI
{
    /// <summary>
    /// Interaction logic for MembersPage.xaml
    /// </summary>
    public partial class MembersPage : Page
    {
        /// <summary>
        /// A dictionary global to the page that relates the list view model properties
        /// to their human readable names.
        /// </summary>
        public static Dictionary<string, string> GDicHumanReadableTableFields 
            = new Dictionary<string, string>();

        /// <summary>
        /// Intializes the Page and content within the Page and populates the
        /// dictionary of human readable property names.
        /// </summary>
        public MembersPage()
        {
            InitializeComponent();

            /* Repopulate the dictionary to accoutn for any field changes. */
            /* TODO: This is currently just a concept for when we have a better idea on how
             * dynamic tables will function. */
            GDicHumanReadableTableFields.Clear();
            GDicHumanReadableTableFields = BusinessTableViewModel.PopulateHumanReadableTableDic();
        }

        private void BtnAddMember_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddMembersPage());
        }

        /// <summary>
        /// Opens a pop-up window that displays the current frames help information. 
        /// </summary>
        /// <param name="sender">Help button</param>
        /// <param name="e">The Click event</param>
        public void HelpButtonHandler(object sender, EventArgs e)
        {
            HelpUI.HelpScreenWindow helpWindow = new HelpUI.HelpScreenWindow();
            helpWindow.Show();
            helpWindow.tabs.SelectedIndex = 0;

        }
        /// <summary>
        /// Pending Implementation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditBusiness_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            BusinessTableViewModel selectedTableViewModel = btn.DataContext as BusinessTableViewModel;

            Business selectedBusiness = new Business();
            using (DatabaseContext context = new DatabaseContext()) 
            {
                selectedBusiness = context.Businesses
                    .FirstOrDefault(business => business.GStrBusinessName.Equals(selectedTableViewModel.StrBuisnessName));
            }

            NavigationService.Navigate(new EditMembersPage(selectedBusiness));
        }

        /// <summary>
        /// A method to populate the lvMemberInfo with the business info once it has been
        /// loaded onto the page.
        /// </summary>
        /// <param name="sender">The list view that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void LvMemberInfo_Loaded(object sender, RoutedEventArgs e)
        {
            /* Load all member data from the Buisness table into the list view. */
            using (DatabaseContext context = new DatabaseContext()) 
            {
                List<BusinessTableViewModel> lstTableViewModel = new List<BusinessTableViewModel>();

                List<Business> lstBusiness = context.Businesses.ToList();
                foreach (Business business in lstBusiness) 
                {
                    BusinessTableViewModel businessTableView = new BusinessTableViewModel();

                    businessTableView.StrBuisnessName = business.GStrBusinessName;

                    /* Get the associated addresses for this business. */
                    Address locationAddress = context.Addresses.Find(business.GIntPhysicalAddressId);
                    Address malingAddress = context.Addresses.Find(business.GIntMailingAddressId);

                    businessTableView.StrLocationAddress = locationAddress?.GStrAddress;
                    businessTableView.StrMailingAddress = malingAddress?.GStrAddress;
                    businessTableView.StrCity = malingAddress?.GStrCity;
                    businessTableView.StrState = malingAddress?.GStrState;
                    businessTableView.IntZipCode = CheckNullableInt(malingAddress?.GIntZipCode);

                    /* Get the business rep from the database. */
                    /* TODO: Need to figure out a way to display more than one buisiness rep. */
                    BusinessRep businessRep = context.BusinessReps
                        .Where(r => r.GIntBusinessId == business.GIntId).FirstOrDefault();

                    if (businessRep == null)
                    {
                        /* If there is no business rep then the business will not have any contact person
                         * related inforamtion. */
                        businessTableView.StrContactPerson = "";
                        businessTableView.StrPhoneNumber = "";
                        businessTableView.StrFaxNumber = "";
                        businessTableView.StrEmailAddress = "";
                    }
                    else 
                    {
                        ContactPerson contactPerson = context.ContactPeople.Find(businessRep.GIntContactPersonId);

                        businessTableView.StrContactPerson = contactPerson?.GStrName;

                        /* Get the phone and fax number of the contact person. */
                        List<PhoneNumber> phoneNumbers = context.PhoneNumbers
                            .Where(pn => pn.GIntContactPersonId == contactPerson.GIntId).ToList();
                        foreach (PhoneNumber phoneNumber in phoneNumbers)
                        {
                            if (phoneNumber.GEnumPhoneType == PhoneType.Mobile ||
                                phoneNumber.GEnumPhoneType == PhoneType.Office)
                            {
                                businessTableView.StrPhoneNumber = phoneNumber?.GStrPhoneNumber;
                            }
                            else
                            {
                                businessTableView.StrFaxNumber = phoneNumber?.GStrPhoneNumber;
                            }
                        }

                        /* Get the contacts persons email address. */
                        Email email = context.Emails
                            .Where(ea => ea.GIntContactPersonId == contactPerson.GIntId).FirstOrDefault();
                        businessTableView.StrEmailAddress = email?.GStrEmailAddress;
                    }

                    businessTableView.StrWebsite = business?.GStrWebsite;
                    businessTableView.StrLevel = Business.GetMebershipLevelString(business.GEnumMembershipLevel);

                    businessTableView.IntEstablishedYear = CheckNullableInt(business?.GIntYearEstablished);

                    lstTableViewModel.Add(businessTableView);
                }

                /* Create the columns for the table. */
                Type typeTableViewModel = typeof(BusinessTableViewModel);
                foreach (var property in typeTableViewModel.GetProperties())
                {
                    GridViewColumn gvcCol = new GridViewColumn();
                    gvcCol.Header = GDicHumanReadableTableFields[property.Name];
                    gvcCol.DisplayMemberBinding = new Binding(property.Name);
                    gvMemberInfo.Columns.Add(gvcCol);
                }

                /* Add a button to edit the business to the end of each row. */
                var btnFactoryEditBusiness = new FrameworkElementFactory(typeof(Button));
                btnFactoryEditBusiness.SetValue(ContentProperty, "Edit");
                btnFactoryEditBusiness.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(BtnEditBusiness_Click));

                DataTemplate dtEdit = new DataTemplate() 
                { 
                    VisualTree = btnFactoryEditBusiness
                };

                GridViewColumn gvcEdit = new GridViewColumn
                {
                    CellTemplate = dtEdit,
                    Header = "Edit"
                };

                gvMemberInfo.Columns.Add(gvcEdit);

                lvMemberInfo.ItemsSource = lstTableViewModel;

                /* Setup the list view filter. */
                CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(lvMemberInfo.ItemsSource);
                collectionView.Filter = LvMemberInfo_Filter;
            }
        }

        /// <summary>
        /// A method for detecting when the text in TxtFilter has changed.
        /// </summary>
        /// <param name="sender">The text box that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvMemberInfo.ItemsSource).Refresh();
        }

        /// <summary>
        /// A method for filtering across all the fields of lvMemberInfo based on the
        /// text entered in TxtFilter.
        /// </summary>
        /// <param name="item">The current item to check from the list view</param>
        /// <returns>A boolean indicated whether or not to display the item.</returns>
        private bool LvMemberInfo_Filter (object item)
        {
            bool boolColHasValue = false;

            if (String.IsNullOrEmpty(txtFilter.Text))
            {
                /* Show all the items in the list view since there is no filter. */
                return true;
            }
            else 
            {
                Type typeItem = typeof(BusinessTableViewModel);
                foreach (var property in typeItem.GetProperties()) 
                {
                    /* Skip over null fields so they don't interrupt the filtering. */
                    if (property.GetValue((item as BusinessTableViewModel), null) == null) 
                    {
                        continue;
                    }

                    /* Check each field in the list view and only display the rows that
                     * contain the entered text. */
                    if (property.GetValue((item as BusinessTableViewModel), null)
                        .ToString()
                        .IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) 
                    {
                        boolColHasValue = true;
                    }
                }

                return boolColHasValue;
            }
        }

        /// <summary>
        /// A method to check if an int in the database is null (or 0).
        /// 
        /// This ensures that null integers are left empty in the table instead of being
        /// displayed as 0.
        /// </summary>
        /// <param name="intCheck">Value to check for null (or 0)</param>
        /// <returns>
        /// If the int in the database is null (or 0) then null is returned.
        /// If the int in the databse is not null then the original value is returned.
        /// </returns>
        public int? CheckNullableInt(int? intCheck) 
        {
            if (intCheck == 0)
            {
                return null;
            }
            else 
            {
                return intCheck;
            }
        }
    }
}
