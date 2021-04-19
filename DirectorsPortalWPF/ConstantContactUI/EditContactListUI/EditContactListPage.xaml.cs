﻿using DirectorPortalDatabase;
using DirectorPortalDatabase.Models;
using DirectorsPortalConstantContact;
using Microsoft.EntityFrameworkCore;
using MS.WindowsAPICodePack.Internal;
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

namespace DirectorsPortalWPF.ConstantContactUI.EditContactListUI
{
    /// <summary>
    /// Interaction logic for EditContactListPage.xaml
    /// </summary>
    public partial class EditContactListPage : Page
    {
        private ConstantContact gObjConstContact;
        private ContactList clEditList;
        private string GStrListName;
        private Frame ContactListFrame;

        private ConstantContactPage objCallBackPage;

        public EditContactListPage(ConstantContact ccHelper, string GStrCList, Frame ContactListFrame, ConstantContactPage objPage)
        {
            InitializeComponent();
            gObjConstContact = ccHelper;
            GStrListName = GStrCList;
            clEditList = gObjConstContact.gdctContactLists.Values.FirstOrDefault(x => x.name.Equals(GStrCList));
            Load_Data(clEditList);
            this.ContactListFrame = ContactListFrame;
            objCallBackPage = objPage;
        }

        /// <summary>
        /// Loads pre existing information into the form
        /// </summary>
        /// <param name="clEditList"></param>
        private void Load_Data(ContactList clEditList)
        {
            txtContactListName.Text = clEditList.name;
            txtNotes.Text = clEditList.description;
            List<DirectorPortalDatabase.Models.ContactPerson> rgContacts;
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                rgContacts = dbContext.ContactPeople.Include(x => x.Emails).Where(x => x.Emails.Count() > 0 && x.Emails.Any(y => !y.EmailAddress.Equals(""))).ToList();
            }

            foreach (Contact item in clEditList.glstMembers)
            {

                DirectorPortalDatabase.Models.ContactPerson objDatabaseContact = rgContacts.Find(x => x.Emails.Any(r => r.EmailAddress.Equals(item.email_address.address)));

                if (objDatabaseContact != null)
                    lstContacts.Items.Add(objDatabaseContact);
            }

        }

        /// <summary>
        /// Checks to see if a Contact List Name is already being used
        /// </summary>
        /// <param name="GStrDupeName"></param>
        /// <returns></returns>
        private Boolean checkName(string GStrDupeName)
        {
            if (GStrDupeName.Equals(clEditList.name))
                return true;
            else
                foreach(ContactList contactList in gObjConstContact.gdctContactLists.Values)
                {
                    if (GStrDupeName.Equals(contactList.name))
                        return false;
                }

            return true;
        }

        /// <summary>
        /// Writes changes made to the corresponding Contact List
        /// Returns to the Constant Contact Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_List(object sender, RoutedEventArgs e)
        {
            if (!checkName(txtContactListName.Text))
            {
                MessageBox.Show("Error Contact List name in use, please use a different name","Error");
                return;
            }
            clEditList.name = txtContactListName.Text;
            clEditList.description = txtNotes.Text;
            gObjConstContact.Update(clEditList);
            clEditList = gObjConstContact.FindListByName(txtContactListName.Text);
            foreach (DirectorPortalDatabase.Models.ContactPerson contactMember in lstContacts.Items)
            {

                Contact contactVal = null;
                foreach (Email groupMemberEmail in contactMember.Emails)
                {
                    contactVal = gObjConstContact.FindContactByEmail(groupMemberEmail.EmailAddress);
                }


                //Contact contactVal = gObjConstContact.gdctContacts.Values.FirstOrDefault(x => x.strFullname.Equals(contactMember.strFullname));
                if (contactVal != null)
                {
                    if(!clEditList.glstMembers.Contains(contactVal))
                        gObjConstContact.AddContactToContactList(clEditList, contactVal);
                }
                else
                {
                    Contact objContactNotInConstCont = new Contact(contactMember.Emails[0].EmailAddress, contactMember.Name.Trim(), "");
                    gObjConstContact.Create(objContactNotInConstCont);
                    objContactNotInConstCont = gObjConstContact.FindContactByEmail(objContactNotInConstCont.email_address.address);
                    //System.Threading.Thread.Sleep(2000);
                    if (!clEditList.glstMembers.Contains(objContactNotInConstCont))
                    gObjConstContact.AddContactToContactList(clEditList, objContactNotInConstCont);
                }

            }

            List<DirectorPortalDatabase.Models.ContactPerson> rgContacts;
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                rgContacts = dbContext.ContactPeople.Include(x => x.Emails).Where(x => x.Emails.Count() > 0 && x.Emails.Any(y => !y.EmailAddress.Equals(""))).ToList();
            }

            for (int i = clEditList.glstMembers.Count() - 1; i>=0; i--)
            {
                Contact contactVal = clEditList.glstMembers.Last(); ;
                DirectorPortalDatabase.Models.ContactPerson objDatabaseContact = rgContacts.Find(x => x.Emails.Any(r => r.EmailAddress.Equals(contactVal.email_address.address)));
                if (!lstContacts.Items.Contains(objDatabaseContact))
                {
                    gObjConstContact.RemoveContactFromContactList(clEditList, contactVal);
                }

                    
            }
            gObjConstContact.RefreshData();
            ContactListFrame.Navigate(new AddContactListUI.AddContactListPage(gObjConstContact, ContactListFrame, objCallBackPage));

        }

        /// <summary>
        /// Searches stored data for contacts that match the given substring
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Database(object sender, TextChangedEventArgs e)
        {
            lstPopup.Items.Clear();
            string strSearchTerm = txtAddContacts.Text;
            List<DirectorPortalDatabase.Models.ContactPerson> rgContacts;
            popSearch.IsOpen = true;

            using (DatabaseContext dbContext = new DatabaseContext())
            {
                rgContacts = dbContext.ContactPeople.Include(x => x.Emails).Where(x => x.Emails.Count() > 0 && x.Emails.Any(y => !y.EmailAddress.Equals(""))).ToList();
            }

            /*            foreach (Contact objContact in gObjConstContact.gdctContacts.Values)
                        {
                            if (objContact.strFullname.ToLower().Contains(strSearchTerm.ToLower()) && !CheckAlreadyInList(objContact))
                            {
                                lstPopup.Items.Add(objContact);
                            }
                        }*/

            foreach (DirectorPortalDatabase.Models.ContactPerson currentContact in rgContacts)
            {
                if (currentContact.Name.ToLower().Contains(strSearchTerm.ToLower()) && !CheckAlreadyInList(currentContact))
                {
                    lstPopup.Items.Add(currentContact);
                }
            }

        }

        /// <summary>
        /// given a contact, check if it is already in the list
        /// </summary>
        /// <param name="objContact">Contact to check</param>
        /// <returns></returns>
        private bool CheckAlreadyInList(Contact objContact)
        {
            foreach (Contact objTemp in lstContacts.Items)
            {
                if (objContact.strFullname.Equals(objTemp.strFullname))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// given a contact, check if it is already in the list
        /// </summary>
        /// <param name="objContact">Contact to check</param>
        /// <returns></returns>
        private bool CheckAlreadyInList(DirectorPortalDatabase.Models.ContactPerson objContact)
        {
            foreach (DirectorPortalDatabase.Models.ContactPerson objTemp in lstContacts.Items)
            {
                if (objContact.Name.Equals(objTemp.Name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets called on the click of the "Remove Member" button on the edit page.
        /// Will remove member from listbox
        /// </summary>
        /// <param name="sender">The Save button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Remove_Contact(object sender, RoutedEventArgs e)
        {
            lstContacts.Items.Remove(lstContacts.SelectedItem);
        }

        /// <summary>
        /// Closes the search box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hide_Search(object sender, RoutedEventArgs e)
        {
            popSearch.IsOpen = false;
        }

        /// <summary>
        /// Adds contact to list box and closes the search box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Contact_To_List(object sender, SelectionChangedEventArgs e)
        {
            if (lstPopup.SelectedIndex >= 0)
            {
                foreach (DirectorPortalDatabase.Models.ContactPerson objContact in lstContacts.Items)
                {
                    if (objContact.Name!=((DirectorPortalDatabase.Models.ContactPerson)lstPopup.SelectedItem).Name)
                    {
                        lstContacts.Items.Add(lstPopup.SelectedItem);
                        txtAddContacts.Clear();
                        popSearch.IsOpen = false;
                        return;
                    }
                    
                }
                if (lstContacts.Items.Count == 0)
                {
                    lstContacts.Items.Add(lstPopup.SelectedItem);
                    txtAddContacts.Clear();
                    popSearch.IsOpen = false;
                    return;
                }
            }
        }

        /// <summary>
        /// Gets called on the click of the "Cancel" button on the edit page.
        /// Will return the user to the Constant Contact Page
        /// </summary>
        /// <param name="sender">The Cancel button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            ContactListFrame.Navigate(new AddContactListUI.AddContactListPage(gObjConstContact, ContactListFrame, objCallBackPage));
        }

        /// <summary>
        /// remove a contactlist from constant contact
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete {txtContactListName.Text}?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                gObjConstContact.RemoveContactList(gObjConstContact.FindListByName(txtContactListName.Text));
                ContactListFrame.Navigate(new AddContactListUI.AddContactListPage(gObjConstContact, ContactListFrame, objCallBackPage));

                objCallBackPage.LoadContactLists(gObjConstContact);
            }

        }
    }
}
