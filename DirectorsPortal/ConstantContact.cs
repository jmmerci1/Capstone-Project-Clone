using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace DirectorsPortal
{
    class ConstantContact
    {
        readonly private string mstrBaseURL = "https://api.cc.email/v3/";
        readonly private string mstrContactsUrl = "contacts?include=custom_fields,list_memberships,phone_numbers,street_addresses&limit=500";
        readonly private string mstrContactListUrl = "contact_lists?limit=1000";
        readonly private string mstrContactCustomFieldUrl = "contact_custom_fields?limit=100";
        readonly private string mstrEmailCampaignUrl = "emails?limit=500";

        public Dictionary<string, Contact> mdctContacts = new Dictionary<string, Contact>();
        public Dictionary<string, ContactList> mdctContactLists = new Dictionary<string, ContactList>();
        public Dictionary<string, ContactCustomField> mdctContactCustomFields = new Dictionary<string, ContactCustomField>();
        public Dictionary<string, EmailCampaign> mdctEmailCampaigns = new Dictionary<string, EmailCampaign>();

        ConstantContactOAuth CCO = new ConstantContactOAuth();

        private string mstrTokenHeader
        {
            get
            {
                return $"Bearer {this.CCO.AccessToken}";
            }
        }

        public void Authenticate()
        {

            this.CCO.LocalRoute = "http://localhost:42069/";
            this.CCO.mstrAppAPIKey = "08d80131-0c76-4829-83fc-be50e14bf0b4";
            this.CCO.mstrAppAPISecret = "HvdbdEaUYXhVYQcUV2XEXg";

            CCO.GetAccessToken();
        }

        public void RefreshCCData()
        {
            this.AddContact();
            this.UpdateContacts();/*
            this.UpdateContactLists();
            this.UpdateContactCustomFields();
            this.UpdateEmailCampaigns();
            */
        }

        private void UpdateContacts()
        {

            string strJson = this.ReadJsonFromUrl(this.mstrContactsUrl);

            Console.WriteLine(strJson);

            Dictionary<string, List<GETContact>> dctDecodedJson = JsonConvert.DeserializeObject<Dictionary<string, List<GETContact>>>(strJson);
            
            foreach (GETContact contact in dctDecodedJson["contacts"])
            {
                this.mdctContacts.Add(contact.contact_id, Contact.FillFromGET(contact));
                
            }

            Console.WriteLine(strJson);
        }

        private void UpdateContactLists()
        {
            string strJson = this.ReadJsonFromUrl(this.mstrContactListUrl);

            Console.WriteLine(strJson);

            Dictionary<string, List<ContactList>> dctDecodedJson = JsonConvert.DeserializeObject<Dictionary<string, List<ContactList>>>(strJson);

            foreach (ContactList lstContactList in dctDecodedJson["lists"])
            {
                this.mdctContactLists.Add(lstContactList.list_id, lstContactList);
            }
        }

        private void UpdateContactCustomFields()
        {
            string strJson = this.ReadJsonFromUrl(this.mstrContactCustomFieldUrl);

            Console.WriteLine(strJson);

            Dictionary<string, List<ContactCustomField>> dctDecodedJson = JsonConvert.DeserializeObject<Dictionary<string, List<ContactCustomField>>>(strJson);

            foreach (ContactCustomField lstFieldList in dctDecodedJson["custom_fields"])
            {
                this.mdctContactCustomFields.Add(lstFieldList.custom_field_id, lstFieldList);
            }
        }

        private void UpdateEmailCampaigns()
        {
            string strJson = this.ReadJsonFromUrl(this.mstrEmailCampaignUrl);

            Console.WriteLine(strJson);

            Dictionary<string, List<EmailCampaign>> dctDecodedJson = JsonConvert.DeserializeObject<Dictionary<string, List<EmailCampaign>>>(strJson);

            foreach (EmailCampaign lstContactList in dctDecodedJson["campaigns"])
            {
                this.mdctEmailCampaigns.Add(lstContactList.campaign_id, lstContactList);
            }
        }

        private void AddContact()
        {
            Contact test = new Contact();

        }

        private string ReadJsonFromUrl(string strUrl)
        {

            HttpWebRequest objAccessTokenRequest = (HttpWebRequest)WebRequest.Create(this.mstrBaseURL + strUrl);
            objAccessTokenRequest.Headers["Authorization"] = this.mstrTokenHeader;
            objAccessTokenRequest.ContentType = "application/json";

            objAccessTokenRequest.Method = "GET";
            HttpWebResponse responce = (HttpWebResponse)objAccessTokenRequest.GetResponse();

            Stream objStreamResponse = responce.GetResponseStream();
            StreamReader objStreamRead = new StreamReader(objStreamResponse);
            Char[] chrBufferArray = new Char[256];
            int intCount = objStreamRead.Read(chrBufferArray, 0, 256);

            string strHttpResponse = "";

            while (intCount > 0)
            {
                String outputData = new String(chrBufferArray, 0, intCount);
                strHttpResponse += outputData;
                intCount = objStreamRead.Read(chrBufferArray, 0, 256);
            }

            strHttpResponse = strHttpResponse.Replace("\n", "");
            return strHttpResponse;

        }

    }
}
