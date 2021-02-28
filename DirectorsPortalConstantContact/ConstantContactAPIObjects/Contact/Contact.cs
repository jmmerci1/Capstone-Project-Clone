using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorsPortalConstantContact
{
    /// <summary>
    /// the main Contact object refrence point. This will have a few helper properties and meathods, but will 
    /// mainly be here for holding the data that has been retrieved from constant contact 
    /// as laid out in the GETContact.cs file. 
    /// </summary>
    public class Contact : GETContact
    {
        ///this will also have a toPUT and toPOST to return the object that be serialized for the coresponding request

        public List<ContactList> glstContactLists = new List<ContactList>();
        public List<GETCustomField> glstCustomFields = new List<GETCustomField>();


        public Contact()
        {

        }

        public Contact(string strEmailAddress)
        {
            this.email_address.address = strEmailAddress;
        }


        public PUTContact Update()
        {
            PUTContact objUpdateContact = new PUTContact();

            objUpdateContact.email_address.address = this.email_address.address;
            objUpdateContact.email_address.permission_to_send = this.email_address.permission_to_send;
            objUpdateContact.email_address.opt_out_reason = this.email_address.opt_out_reason;

            objUpdateContact.first_name = this.first_name;
            objUpdateContact.last_name = this.last_name;
            objUpdateContact.job_title = this.job_title;
            objUpdateContact.company_name = this.company_name;
            objUpdateContact.birthday_month = this.birthday_month;
            objUpdateContact.birthday_day = this.birthday_day;
            objUpdateContact.anniversary = this.anniversary;
            objUpdateContact.update_source = "Account";
            objUpdateContact.list_memberships = this.list_memberships;

            foreach (GETPhoneNumber objPhone in this.phone_numbers)
            {
                POSTPhoneNumber objTemp = new POSTPhoneNumber();
                objTemp.phone_number = objPhone.phone_number;
                objTemp.kind = objPhone.kind ?? "";
                objUpdateContact.phone_numbers.Add(objTemp);
            }

            foreach (GETContactCustomField objField in this.custom_fields)
            {
                GETContactCustomField objTemp = new GETContactCustomField();
                objTemp.custom_field_id = objField.custom_field_id;
                objTemp.value = objField.value;
                objUpdateContact.custom_fields.Add(objTemp);
            }

            foreach (GETStreetAddress objAddress in this.street_addresses)
            {
                POSTStreetAddress objTemp = new POSTStreetAddress();
                objTemp.street = objAddress.street;
                objTemp.kind = objAddress.kind;
                objTemp.city = objAddress.city;
                objTemp.state = objAddress.state;
                objTemp.postal_code = objAddress.postal_code;
                objTemp.country = objAddress.country;
                objUpdateContact.street_addresses.Add(objTemp);
            }

            return objUpdateContact;

        }


    }
}
