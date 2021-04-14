﻿using DirectorsPortalConstantContact;
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

namespace DirectorsPortalWPF.ConstantContactUI.AddContactListUI
{
    /// <summary>
    /// Interaction logic for AddContactListPage.xaml
    /// </summary>
    public partial class AddContactListPage : Page
    {
        private ConstantContact gObjConstContact;
        public AddContactListPage(ConstantContact ccHelper)
        {
            InitializeComponent();
            gObjConstContact = ccHelper;
        }

        /// <summary>
        /// Gets called on the click of the "Remove Member" button on the add page.
        /// Will remove member from listbox
        /// </summary>
        /// <param name="sender">The Save button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Remove_Contact(object sender, RoutedEventArgs e)
        {
            lstContacts.Items.Remove(lstContacts.SelectedItem);
        }

        /// <summary>
        /// Checks to see if a Contact List Name is already being used
        /// </summary>
        /// <param name="GStrDupeName"></param>
        /// <returns></returns>
        private Boolean checkName(string GStrDupeName)
        {
                foreach (ContactList contactList in gObjConstContact.gdctContactLists.Values)
                {
                    if (GStrDupeName.Equals(contactList.name))
                        return false;
                }

            return true;
        }

        /// <summary>
        /// Creates a new contact list and populates it with selected contacts
        /// Returns to the Constant Contact Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_List(object sender, RoutedEventArgs e)
        {
            
            string strGroupName = txtContactListName.Text;
            string strNotes = txtNotes.Text;
            if (!checkName(strGroupName))
            {
                MessageBox.Show("Error Contact List name in use, please use a different name", "Error");
                return;
            }
            ContactList newList = new ContactList(strGroupName, strNotes);
            gObjConstContact.Create(newList);
            newList = gObjConstContact.FindListByName(strGroupName);
                foreach (Contact groupMember in lstContacts.Items)
                {
                    Contact b = gObjConstContact.gdctContacts.Values.FirstOrDefault(x => x.strFullname.Equals(groupMember.strFullname));
                    gObjConstContact.AddContactToContactList(newList, b);
                }

            this.NavigationService.Navigate(new ConstantContactUI.ConstantContactPage(gObjConstContact));

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

            popSearch.IsOpen = true;


            foreach (Contact business in gObjConstContact.gdctContacts.Values)
            {
                    if (business.strFullname.ToLower().Contains(strSearchTerm.ToLower()))
                    {
                        lstPopup.Items.Add(business);
                    }
                        
                
            }
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
                lstContacts.Items.Add(lstPopup.SelectedItem);
                txtAddContacts.Clear();
                popSearch.IsOpen = false;
            }
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
        /// Gets called on the click of the "Cancel" button on the add page.
        /// Will return the user to the Constant Contact Page
        /// </summary>
        /// <param name="sender">The Cancel button object that has called the function.</param>
        /// <param name="e">The button press event</param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ConstantContactUI.ConstantContactPage(gObjConstContact));
        }
    }
}
