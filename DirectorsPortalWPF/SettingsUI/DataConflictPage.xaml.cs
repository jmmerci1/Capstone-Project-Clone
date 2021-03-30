using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/// <summary>
/// File Purpose:
///     This file defines the logic for the Data Import Conflict screen. This
///     screen is intended to resolve Excel import conflicts in which a row of data in Excel
///     would cause duplicated data in the Director's Portal Database. The user
///     has the option to select the rows from either the Database or Excel in which they want to keep and
///     save the changes to the database.
///         
/// </summary>
/// 
namespace DirectorsPortalWPF.SettingsUI
{
    /// <summary>
    /// Interaction logic for DataConflictPage.xaml
    /// </summary>
    public partial class DataConflictPage : Page
    {
        /// <summary>
        /// A dictionary global to the page that relates the list view model properties
        /// to their human readable names.
        /// </summary>
        readonly Dictionary<string, string> GDicHumanReadableTableFields
            = new Dictionary<string, string>();

        /// <summary>
        /// Initialization of the Data Import Conflict Screen.
        /// </summary>
        public DataConflictPage()
        {
            InitializeComponent();
            PopulateHumanReadableTableFields();
        }

        /// <summary>
        /// Returns the user back to the Settings screen where they attempted
        /// to import data using Excel
        /// </summary>
        /// <param name="sender">The 'Cancel Import' button</param>
        /// <param name="e">The Click event</param>
        private void BtnCancelLoad_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Used to confirm the data the user want to save to the database and 
        /// resolve the data conflict between Director's Portal and Excel Import.
        /// </summary>
        /// <param name="sender">The 'Resolve and Save' Button</param>
        /// <param name="e">The Click Event</param>
        private void BtnResolveConflict_Click(object sender, RoutedEventArgs e)
        {

            // Template code, need functionality to be implemented. 
            NavigationService.GoBack();
        }

        /// <summary>
        /// Pending Implementation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkResolve_Click(object sender, RoutedEventArgs e)
        {
            // Should be used to mark an item to be saved to the Database.
        }

        /// <summary>
        /// Intended to populate the right table on the Database Conflict Screen. The right screen will
        /// be used to show items in the Excel import that conflict with 
        /// the Director's Portal Database.
        /// </summary>
        /// <param name="sender">The list view that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void LvExcelConflict_Loaded(object sender, RoutedEventArgs e)
        {
            /* Load all member data from the Buisness table into the list view. */
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                List<BusinessTableViewModel> lstTableViewModel = new List<BusinessTableViewModel>();

                List<Business> lstBusiness = dbContext.Businesses.ToList();

                foreach (Business busCurrentBusiness in lstBusiness)
                {
                    BusinessTableViewModel objBusinessTableView = new BusinessTableViewModel
                    {
                        StrBuisnessName = busCurrentBusiness.BusinessName
                    };

                    /* Get the associated addresses for this business. */
                    Address objLocationAddress = dbContext.Addresses.Find(busCurrentBusiness.PhysicalAddress);
                    Address objMalingAddress = dbContext.Addresses.Find(busCurrentBusiness.MailingAddress);

                    objBusinessTableView.StrLocationAddress = objLocationAddress?.StreetAddress;
                    objBusinessTableView.StrMailingAddress = objMalingAddress?.StreetAddress;
                    objBusinessTableView.StrCity = objLocationAddress?.City;
                    objBusinessTableView.StrState = objLocationAddress?.State;
                    objBusinessTableView.IntZipCode = CheckNullableInt(objLocationAddress?.ZipCode);

                    /* Get the business rep from the database. */
                    /* TODO: Need to figure out a way to display more than one buisiness rep. */
                    BusinessRep objBusinessRep = dbContext.BusinessReps
                        .Where(r => r.BusinessId == busCurrentBusiness.Id).FirstOrDefault();

                    if (objBusinessRep == null)
                    {
                        /* If there is no business rep then the business will not have any contact person
                         * related inforamtion. */
                        objBusinessTableView.StrContactPerson = "";
                        objBusinessTableView.StrPhoneNumber = "";
                        objBusinessTableView.StrFaxNumber = "";
                        objBusinessTableView.StrEmailAddress = "";
                    }
                    else
                    {
                        ContactPerson objContactPerson = dbContext.ContactPeople.Find(objBusinessRep.ContactPersonId);

                        objBusinessTableView.StrContactPerson = objContactPerson?.Name;

                        /* Get the phone and fax number of the contact person. */
                        List<PhoneNumber> objPhoneNumbers = dbContext.PhoneNumbers
                            .Where(pn => pn.ContactPersonId == objContactPerson.Id).ToList();

                        foreach (PhoneNumber objCurrentPhoneNumber in objPhoneNumbers)
                        {
                            if (objCurrentPhoneNumber.GEnumPhoneType == PhoneType.Mobile ||
                                objCurrentPhoneNumber.GEnumPhoneType == PhoneType.Office)
                            {
                                objBusinessTableView.StrPhoneNumber = objCurrentPhoneNumber?.Number;
                            }
                            else
                            {
                                objBusinessTableView.StrFaxNumber = objCurrentPhoneNumber?.Number;
                            }
                        }

                        /* Get the contacts persons email address. */
                        Email objEmail = dbContext.Emails
                            .Where(ea => ea.ContactPersonId == objContactPerson.Id).FirstOrDefault();
                        objBusinessTableView.StrEmailAddress = objEmail?.EmailAddress;
                    }

                    objBusinessTableView.StrWebsite = busCurrentBusiness?.Website;
                    objBusinessTableView.StrLevel = GetMebershipLevelString(busCurrentBusiness.MembershipLevel);

                    objBusinessTableView.IntEstablishedYear = CheckNullableInt(busCurrentBusiness?.YearEstablished);

                    lstTableViewModel.Add(objBusinessTableView);
                }

                /* Add a check box to allow user to check off rows of data they want to keep in the system. */
                var btnFactoryCheckResolve = new FrameworkElementFactory(typeof(CheckBox));
                btnFactoryCheckResolve.SetValue(ContentProperty, "");
                btnFactoryCheckResolve.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(ChkResolve_Click));

                DataTemplate dtResolve = new DataTemplate()
                {
                    VisualTree = btnFactoryCheckResolve
                };

                GridViewColumn gvcEdit = new GridViewColumn
                {
                    CellTemplate = dtResolve,
                    Header = "Overwrite"
                };

                gvExcelConflictRows.Columns.Add(gvcEdit);

                /* Create the columns for the table. */
                Type typeTableViewModel = typeof(BusinessTableViewModel);
                foreach (var property in typeTableViewModel.GetProperties())
                {
                    GridViewColumn gvcCol = new GridViewColumn
                    {
                        Header = GDicHumanReadableTableFields[property.Name],
                        DisplayMemberBinding = new Binding(property.Name)
                    };
                    gvExcelConflictRows.Columns.Add(gvcCol);
                }

                lvExcelConflict.ItemsSource = lstTableViewModel;
            }
        }

        /// <summary>
        /// A method for popualting the human readable names into the
        /// global dictionary. These fields will be used to relate the names of
        /// the properties to the their human readable column headers.
        /// </summary>
        private void PopulateHumanReadableTableFields()
        {
            /* TODO: This method will need to change as we develope a solution for dynamic
             * fields. */
            GDicHumanReadableTableFields.Clear();

            GDicHumanReadableTableFields.Add("StrBuisnessName", "Business Name");
            GDicHumanReadableTableFields.Add("StrMailingAddress", "Mailing Address");
            GDicHumanReadableTableFields.Add("StrLocationAddress", "Location Address");
            GDicHumanReadableTableFields.Add("StrCity", "City");
            GDicHumanReadableTableFields.Add("StrState", "State");
            GDicHumanReadableTableFields.Add("IntZipCode", "Zip Code");
            GDicHumanReadableTableFields.Add("StrContactPerson", "Contact");
            GDicHumanReadableTableFields.Add("StrPhoneNumber", "Phone Number");
            GDicHumanReadableTableFields.Add("StrFaxNumber", "Fax Number");
            GDicHumanReadableTableFields.Add("StrEmailAddress", "Email Address");
            GDicHumanReadableTableFields.Add("StrWebsite", "Website");
            GDicHumanReadableTableFields.Add("StrLevel", "Level");
            GDicHumanReadableTableFields.Add("IntEstablishedYear", "Established");
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


        /// <summary>
        /// A method for converting the membership level from the business entity to
        /// a human readable string.
        /// </summary>
        /// <param name="membershipLevel">The membership level from the business entity.</param>
        /// <returns>The human readable membership string.</returns>
        private string GetMebershipLevelString(MembershipLevel membershipLevel)
        {
            string strLevel = "";

            switch (membershipLevel)
            {
                case MembershipLevel.GOLD:
                    strLevel = "Gold";
                    break;

                case MembershipLevel.SILVER:
                    strLevel = "Silver";
                    break;

                case MembershipLevel.ASSOCIATE:
                    strLevel = "Associate";
                    break;

                case MembershipLevel.INDIVIDUAL:
                    strLevel = "Individual";
                    break;

                case MembershipLevel.COURTESY:
                    strLevel = "Courtesy";
                    break;
            }

            return strLevel;
        }

    }

    /// <summary>
    /// A view model that defines the members table.
    /// </summary>
    public class BusinessTableViewModel
    {
        public string StrBuisnessName { get; set; }
        public string StrMailingAddress { get; set; }
        public string StrLocationAddress { get; set; }
        public string StrCity { get; set; }
        public string StrState { get; set; }
        public int? IntZipCode { get; set; }
        public string StrContactPerson { get; set; }
        public string StrPhoneNumber { get; set; }
        public string StrFaxNumber { get; set; }
        public string StrEmailAddress { get; set; }
        public string StrWebsite { get; set; }
        public string StrLevel { get; set; }
        public int? IntEstablishedYear { get; set; }
    }
}
