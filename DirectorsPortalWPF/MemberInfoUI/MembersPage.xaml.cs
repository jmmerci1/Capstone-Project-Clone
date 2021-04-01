using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorPortalDatabase.Utility;
using DirectorsPortal;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                    .Where(business => business.BusinessName.Equals(selectedTableViewModel.StrBuisnessName))
                    .FirstOrDefault();
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

                List<Business> lstBusiness = context.Businesses
                    .Include(bus => bus.MailingAddress)
                    .Include(bus => bus.PhysicalAddress)
                    .ToList();
                foreach (Business business in lstBusiness) 
                {
                    BusinessTableViewModel businessTableView = new BusinessTableViewModel();

                    businessTableView.StrBuisnessName = business.BusinessName;

                    /* Get the associated addresses for this business. */
                    //Address locationAddress = context.Addresses.Find(business.PhysicalAddressId);
                    //Address malingAddress = context.Addresses.Find(business.MailingAddressId);
                    Address locationAddress = business.PhysicalAddress;
                    Address malingAddress = business.MailingAddress;

                    businessTableView.StrLocationAddress = locationAddress?.StreetAddress;
                    businessTableView.StrMailingAddress = malingAddress?.StreetAddress;
                    businessTableView.StrCity = malingAddress?.City;
                    businessTableView.StrState = malingAddress?.State;
                    businessTableView.IntZipCode = CheckNullableInt(malingAddress?.ZipCode);

                    /* Get the business rep from the database. */
                    /* TODO: Need to figure out a way to display more than one buisiness rep. */
                    BusinessRep businessRep = context.BusinessReps
                        .Where(r => r.BusinessId == business.Id).FirstOrDefault();

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
                        ContactPerson contactPerson = context.ContactPeople.Find(businessRep.ContactPersonId);

                        businessTableView.StrContactPerson = contactPerson?.Name;

                        /* Get the phone and fax number of the contact person. */
                        List<PhoneNumber> phoneNumbers = context.PhoneNumbers
                            .Where(pn => pn.ContactPersonId == contactPerson.Id).ToList();
                        foreach (PhoneNumber phoneNumber in phoneNumbers)
                        {
                            if (phoneNumber.GEnumPhoneType == DirectorPortalDatabase.Models.PhoneType.Mobile ||
                                phoneNumber.GEnumPhoneType == DirectorPortalDatabase.Models.PhoneType.Office)
                            {
                                businessTableView.StrPhoneNumber = phoneNumber?.Number;
                            }
                            else
                            {
                                businessTableView.StrFaxNumber = phoneNumber?.Number;
                            }
                        }

                        /* Get the contacts persons email address. */
                        Email email = context.Emails
                            .Where(ea => ea.ContactPersonId == contactPerson.Id).FirstOrDefault();
                        businessTableView.StrEmailAddress = email?.EmailAddress;
                    }

                    businessTableView.StrWebsite = business?.Website;
                    businessTableView.StrLevel = Business.GetMebershipLevelString(business.MembershipLevel);

                    businessTableView.IntEstablishedYear = CheckNullableInt(business?.YearEstablished);

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
                btnFactoryEditBusiness.SetValue(TemplateProperty, (ControlTemplate)System.Windows.Application.Current.Resources["smallButton"]);
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

        GridViewColumnHeader lastHeaderClicked = null;
        ListSortDirection lastDirection = ListSortDirection.Ascending;
        //Takes an input string, and direction
        //Sorts column with name matching the string in the given direction
        private void Sort(string strSortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lvMemberInfo.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(strSortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
        /// <summary>
        /// Calls the sort function when a column header is clicked passing it the column name and direction
        /// Maintains which column is currently being sorted and in which direction
        /// </summary>
        /// <param name="sender">The text box that called the method.</param>
        /// <param name="e">Event data asscociated with this event.</param>
        private void GridViewColumnHeaderClickedHandler(object sender,
                                            RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var strSortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(strSortBy, direction);

                    lastHeaderClicked = headerClicked;
                    lastDirection = direction;
                }
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
        /// <summary>
        /// When the user clicks the 'Add New from PDF' button, they will have the ability to select a 
        /// New Membership Request PDF form that will then be parsed and added to the database automatically.
        /// This is intended to be a faster option that typing in new member details by hand.
        /// </summary>
        /// <param name="sender">The 'Add New from PDF' button</param>
        /// <param name="e">THe Click Event</param>
        private void BtnNewMembPdf_Click(object sender, RoutedEventArgs e)
        {
            // Get fields from PDF then pass to Add Members Screen.
            Dictionary<string, string> dictFields = OpenFile();

            if (dictFields.ContainsKey("Business Name"))
                NavigationService.Navigate(new AddMembersPage(dictFields));
        }

        /// <summary>
        /// When the user click the 'Update from PDF' button, they will have the ability to select a 
        /// Update Membership Request PDF form that will be parsed for existing members and their data changes.
        /// Updated data is updated in the DB.
        /// </summary>
        /// <param name="sender">The 'Update from PDF' button</param>
        /// <param name="e">The Click Event</param>
        private void BtnModMembPdf_Click(object sender, RoutedEventArgs e)
        {
            // Get fields from PDF
            Dictionary<string, string> dictFields = OpenFile();
            Business busModified = new Business();

            if (dictFields.ContainsKey("Business Name"))
            {
                // Do some logic to find the business being modified.
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    busModified = dbContext.Businesses
                        .Where(business => business.BusinessName.Equals(dictFields["Business Name"])).FirstOrDefault();
                }

                if (busModified != null)
                {
                    NavigationService.Navigate(new EditMembersPage(busModified, dictFields));
                }
                else
                    MessageBox.Show($"{ dictFields["Business Name"] } is not an existing Businss in the Database", "Business Not Found");
            }
        }

        List<Dictionary<string, string>> pdfEmails = new List<Dictionary<string, string>>();

        async Task AutoPdfGet()
        {
            var a = await GraphApiClient.GetAllEmails();
            foreach (Message message in a)
                pdfEmails.Add(EmailGetPdfs(message));
        }

        private Dictionary<string, string> EmailGetPdfs(Message a)
        {
            var dicToAdd = new Dictionary<string, string>();

            //create new reader object
            PdfReader reader = new PdfReader(a.Attachments[0].ContentType);
            //variable for form fields in PDF 
            var objFields = reader.AcroFields.Fields;
            //array to contain all values from key value pairs read
            var arrFieldData = new ArrayList();
            //iterates over key value pairs and add values(data from pdf) to the array
            foreach (var item in objFields.Keys)
            {
                arrFieldData.Add(reader.AcroFields.GetField(item.ToString()));
                Console.WriteLine(reader.AcroFields.GetField(item.ToString()));
            }
            //array to split city state zip 
            String[] strCityStateZip = arrFieldData[4].ToString().Split(',');


            //dictionary add statements to add pdf data to ui
            dicToAdd.Add("Business Name", (string)arrFieldData[0]);
            dicToAdd.Add("Website", (string)arrFieldData[8]);
            dicToAdd.Add("Level", (string)arrFieldData[13]);
            dicToAdd.Add("Established", (string)arrFieldData[9]);

            // Mailing Address
            dicToAdd.Add("Mailing Address", (string)arrFieldData[2]);

            if (strCityStateZip.Length > 0)
                dicToAdd.Add("City", strCityStateZip[0]);
            else
                dicToAdd.Add("City", "");

            if (strCityStateZip.Length > 1)
                dicToAdd.Add("State", strCityStateZip[1]);
            else
                dicToAdd.Add("State", "");

            if (strCityStateZip.Length > 2)
                dicToAdd.Add("Zip Code", strCityStateZip[2]);
            else
                dicToAdd.Add("Zip Code", "");

            // Location Address
            dicToAdd.Add("Location Address", (string)arrFieldData[3]);

            if (strCityStateZip.Length > 0)
                dicToAdd.Add("Location City", strCityStateZip[0]);
            else
                dicToAdd.Add("Location City", "");

            if (strCityStateZip.Length > 1)
                dicToAdd.Add("Location State", strCityStateZip[1]);
            else
                dicToAdd.Add("Location State", "");

            if (strCityStateZip.Length > 2)
                dicToAdd.Add("Location Zip Code", strCityStateZip[2]);
            else
                dicToAdd.Add("Location Zip Code", "");

            dicToAdd.Add("Contact Name", (string)arrFieldData[1]);
            dicToAdd.Add("Phone Number", (string)arrFieldData[5]);
            dicToAdd.Add("Fax Number", (string)arrFieldData[6]);
            dicToAdd.Add("Email Address", (string)arrFieldData[7]);

                
            //dictionary return
            return dicToAdd;
        }
        

        /// <summary>
        /// Ability to open a file using the OpenFileDialog. 
        /// 
        /// . 
        /// </summary>
        private Dictionary<string, string> OpenFile()
        {
            //dictionary to store values to be sent to UI
            var dicToAdd = new Dictionary<string, string>();

            //opens file dialog to select pdf
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "PDF Files|*.pdf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Console.WriteLine(System.IO.File.ReadAllText(openFileDialog.FileName));

                //create new reader object
                PdfReader reader = new PdfReader(openFileDialog.FileName);
                //variable for form fields in PDF 
                var objFields = reader.AcroFields.Fields;
                //array to contain all values from key value pairs read
                var arrFieldData = new ArrayList();
                //iterates over key value pairs and add values(data from pdf) to the array
                foreach (var item in objFields.Keys)
                {
                    arrFieldData.Add(reader.AcroFields.GetField(item.ToString()));
                    Console.WriteLine(reader.AcroFields.GetField(item.ToString()));
                }
                //array to split city state zip 
                String[] strCityStateZip = arrFieldData[4].ToString().Split(',');


                //dictionary add statements to add pdf data to ui
                dicToAdd.Add("Business Name", (string)arrFieldData[0]);
                dicToAdd.Add("Website", (string)arrFieldData[8]);
                dicToAdd.Add("Level", (string)arrFieldData[13]);
                dicToAdd.Add("Established", (string)arrFieldData[9]);

                // Mailing Address
                dicToAdd.Add("Mailing Address", (string)arrFieldData[2]);

                if (strCityStateZip.Length > 0)
                    dicToAdd.Add("City", strCityStateZip[0]);
                else
                    dicToAdd.Add("City", "");

                if (strCityStateZip.Length > 1)
                    dicToAdd.Add("State", strCityStateZip[1]);
                else
                    dicToAdd.Add("State", "");

                if (strCityStateZip.Length > 2)
                    dicToAdd.Add("Zip Code", strCityStateZip[2]);
                else
                    dicToAdd.Add("Zip Code", "");

                // Location Address
                dicToAdd.Add("Location Address", (string)arrFieldData[3]);

                if (strCityStateZip.Length > 0)
                    dicToAdd.Add("Location City", strCityStateZip[0]);
                else
                    dicToAdd.Add("Location City", "");

                if (strCityStateZip.Length > 1)
                    dicToAdd.Add("Location State", strCityStateZip[1]);
                else
                    dicToAdd.Add("Location State", "");

                if (strCityStateZip.Length > 2)
                    dicToAdd.Add("Location Zip Code", strCityStateZip[2]);
                else
                    dicToAdd.Add("Location Zip Code", "");

                dicToAdd.Add("Contact Name", (string)arrFieldData[1]);
                dicToAdd.Add("Phone Number", (string)arrFieldData[5]);
                dicToAdd.Add("Fax Number", (string)arrFieldData[6]);
                dicToAdd.Add("Email Address", (string)arrFieldData[7]);
            }
            //dictionary return
            return dicToAdd;
        }
    }
}
