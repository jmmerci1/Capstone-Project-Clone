using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Globalization;

namespace DirectorsPortalConstantContact
{
    /// <summary>
    /// This is the main Constant Contact interface. after you give the .Authenticate() command, 
    ///     you will be able to access the attribiutes that have been queried from Constant Contact API
    /// </summary>
    public class ConstantContact
    {
        readonly private string gstrBaseURL = "https://api.cc.email/v3/";
        readonly private string gstrContactsUrl = "contacts?include=custom_fields,list_memberships,phone_numbers,street_addresses&limit=500";
        readonly private string gstrContactListUrl = "contact_lists?limit=1000";
        readonly private string gstrContactCustomFieldUrl = "contact_custom_fields?limit=100";
        readonly private string gstrEmailCampaignUrl = "emails?limit=500";

        //these will become private
        public Dictionary<string, Contact> gdctContacts = new Dictionary<string, Contact>();
        public Dictionary<string, ContactList> gdctContactLists = new Dictionary<string, ContactList>();
        public Dictionary<string, CustomField> gdctCustomFields = new Dictionary<string, CustomField>();
        public Dictionary<string, EmailCampaign> gdctEmailCampaigns = new Dictionary<string, EmailCampaign>();
        public Dictionary<string, EmailCampaignActivity> gdctEmailCampaignActivities = new Dictionary<string, EmailCampaignActivity>();
        public List<EmailCampaignActivityPreview> glstEmailCampaignActivityPreviews = new List<EmailCampaignActivityPreview>();

        private ConstantContactOAuth gobjCCAuth = new ConstantContactOAuth()
        {
            MstrLocalRoute = "http://localhost:40000/",
            mstrAppAPIKey = "08d80131-0c76-4829-83fc-be50e14bf0b4",
            mstrAppAPISecret = "HvdbdEaUYXhVYQcUV2XEXg"
        };

        private string mstrTokenHeader => $"Bearer {this.gobjCCAuth.MstrAccessToken}";


        // add properties for dictionaries

        public Dictionary<string, Contact>.ValueCollection Contacts => this.gdctContacts.Values;
        public Dictionary<string, ContactList>.ValueCollection ContactLists => this.gdctContactLists.Values;
        public Dictionary<string, CustomField>.ValueCollection CustomFields => this.gdctCustomFields.Values;
        public Dictionary<string, EmailCampaign>.ValueCollection EmailCampaigns => this.gdctEmailCampaigns.Values;


        /// <summary>
        /// One function to run all of the update functions. 
        ///
        /// </summary>
        public void RefreshData()
        {
            //get all info
            this.gobjCCAuth.ValidateAuthentication();
            //print("Updating Contacts");
            this.UpdateContacts();
            System.Threading.Thread.Sleep(300);
            //print("Updating ContactLists");
            this.UpdateContactLists();
            //print("Updating Fields");
            this.UpdateContactCustomFields();
            System.Threading.Thread.Sleep(300);
            //print("Updating Campaigns");
            this.UpdateEmailCampaigns();
            System.Threading.Thread.Sleep(200);
            //print("Updating activities");
            this.UpdateEmailCampaignActivities();
            System.Threading.Thread.Sleep(200);
            //print("Updating previews");
            this.UpdateEmailCampaignActivityPreviews();

            //print("Updating Reporting");
            this.UpdateContactTrackingReporting();
            //print("Updating open rate");
            this.UpdateContactOpenRate();

            //assignments
            //print("Updating assignments");
            this.ContactListAssignment();
            this.CustomFieldAssignment();


            //print("saving");
            this.save();

            // GAVIN
            AddContactToContactList(this.FindListByName("gavinTempList"), this.FindContactByEmail("gemalisk@svsu.edu"));
            AddContactToContactList(this.FindListByName("gavinTempList"), this.FindContactByEmail("edwalk@svsu.edu"));
        }

        /// <summary>
        /// Talks to this.ReadJsonFromUrl with the url for GETContacts and 
        ///     creates the coresponding objects that we are given
        /// </summary>
        private void UpdateContacts()
        {
            string strLink = this.gstrContactsUrl;

            Dictionary<string, Contact> dctTempContacts = new Dictionary<string, Contact>();

            while (true) {
                string strJson = this.ReadJsonFromUrl(strLink);

                JObject objJson = JObject.Parse(strJson);
                string strContactList = objJson.GetValue("contacts").ToString();

                List<Contact> lstDecodedJson = JsonConvert.DeserializeObject<List<Contact>>(strContactList);

                foreach (Contact contact in lstDecodedJson)
                {
                    dctTempContacts.Add(contact.contact_id, contact);
                }
                
                try
                {
                    strLink = (string)objJson["_links"]["next"]["href"];
                    strLink = strLink.Substring(4, strLink.Length - 4);
                }
                catch (System.NullReferenceException)
                {
                    //do i need to manage the gc here?
                    this.gdctContacts = dctTempContacts;
                    return;
                }
                
            }
            

        }
        /// <summary>
        /// Talks to this.ReadJsonFromUrl with the url for GETContactList and 
        ///     creates the coresponding objects that we are given
        /// </summary>
        private void UpdateContactLists()
        {
            //fix for while True
            string strJson = this.ReadJsonFromUrl(this.gstrContactListUrl);


            Dictionary<string, ContactList> dctTempContactLists = new Dictionary<string, ContactList>();

            //Console.WriteLine(strJson);

            Dictionary<string, List<ContactList>> dctDecodedJson = JsonConvert.DeserializeObject<Dictionary<string, List<ContactList>>>(strJson);

            foreach (ContactList lstContactList in dctDecodedJson["lists"])
            {
                dctTempContactLists.Add(lstContactList.list_id, lstContactList);
            }

            this.gdctContactLists = dctTempContactLists;
            return;
        }

        /// <summary>
        /// Talks to this.ReadJsonFromUrl with the url for GETCustomFields and 
        ///     creates the coresponding objects that we are given
        /// </summary>
        private void UpdateContactCustomFields()
        {
            string strJson = this.ReadJsonFromUrl(this.gstrContactCustomFieldUrl);

            Dictionary<string, CustomField> dctTempCustomFields = new Dictionary<string, CustomField>();

            Dictionary<string, List<CustomField>> dctDecodedJson = JsonConvert.DeserializeObject<Dictionary<string, List<CustomField>>>(strJson);

            foreach (CustomField lstFieldList in dctDecodedJson["custom_fields"])
            {
                dctTempCustomFields.Add(lstFieldList.custom_field_id, lstFieldList);
            }

            this.gdctCustomFields = dctTempCustomFields;
            return;
        }

        /// <summary>
        /// Talks to this.ReadJsonFromUrl with the url for EmailCampaigns and 
        ///     creates the coresponding objects that we are given
        /// </summary>
        private void UpdateEmailCampaigns()
        {
            string strLink = this.gstrEmailCampaignUrl;
            //this should make a temp dict and use that instead of just erasing the data first. 
            Dictionary<string, EmailCampaign> dctTempEmailCampaigns = new Dictionary<string, EmailCampaign>();
            while (true)
            {
                //json response for all campaigns
                string strJson = this.ReadJsonFromUrl(strLink);

                //seperate the campaigns key from the other json data
                JObject objJson = JObject.Parse(strJson);
                string strLists = objJson.GetValue("campaigns").ToString();

                List<EmailCampaign> dctDecodedJson = JsonConvert.DeserializeObject<List<EmailCampaign>>(strLists);

                foreach (EmailCampaign lstEmailCampaign in dctDecodedJson)
                {
                    if (lstEmailCampaign.current_status != "Removed")
                    {
                        dctTempEmailCampaigns.Add(lstEmailCampaign.campaign_id, lstEmailCampaign);
                    }
                }

                //Trabis is dumb
                try
                {
                    strLink = (string)objJson["_links"]["next"]["href"];
                    strLink = strLink.Substring(4, strLink.Length - 4);
                }
                catch (System.NullReferenceException)
                {
                    this.gdctEmailCampaigns = dctTempEmailCampaigns;
                    return;
                }
            }
        }

        /// <summary>
        ///     update all of the activities for each campaign, with aas much info as we can get. 
        /// </summary>
        private void UpdateEmailCampaignActivities()
        {
            //loop through all of the campaigns to get their id, then request all of that campaigns activities

            gdctEmailCampaignActivities = new Dictionary<string, EmailCampaignActivity>();
            foreach (EmailCampaign objCampaign in this.gdctEmailCampaigns.Values)
            {
                string strUrl = $"emails/{objCampaign.campaign_id}";

                string strJson = this.ReadJsonFromUrl(strUrl);

                JObject objJson = JObject.Parse(strJson);
                string strActivities = objJson.GetValue("campaign_activities").ToString();
                List<ActivityList> lstDecodedJson = JsonConvert.DeserializeObject<List<ActivityList>>(strActivities);

                foreach(ActivityList objTempActivity in lstDecodedJson)
                {

                    string strActivityUrl = $"emails/activities/{objTempActivity.campaign_activity_id}";//?include=permalink_url";
                    string strActivityJson = this.ReadJsonFromUrl(strActivityUrl);
                    EmailCampaignActivity objActivity = JsonConvert.DeserializeObject<EmailCampaignActivity>(strActivityJson);
                    objCampaign.Activities.Add(objActivity);
                    gdctEmailCampaignActivities.Add(objActivity.campaign_activity_id, objActivity);



                }
                System.Threading.Thread.Sleep(250);

            }
        }

        private void UpdateEmailCampaignActivityPreviews()
        {
            foreach(EmailCampaign objCampaign in this.gdctEmailCampaigns.Values)
            {
                foreach(EmailCampaignActivity objActivity in objCampaign.Activities)
                {
                    //get scheduals for the activity
                    //only promary email will have sched list
                    if (objActivity.role == "primary_email")
                    {
                        string strUrl = $"emails/activities/{objActivity.campaign_activity_id}/previews";
                        string strData = this.ReadJsonFromUrl(strUrl);

                        EmailCampaignActivityPreview objPreview = JsonConvert.DeserializeObject<EmailCampaignActivityPreview>(strData);

                        objPreview.activity = objActivity;

                        objActivity.mobjPreview = objPreview;
                        this.glstEmailCampaignActivityPreviews.Add(objPreview);

                        //TESTING
                        if (objActivity.contact_list_ids.Count() > 0)
                        {
                            DateTime objTime = DateTime.Now.AddMinutes(2);
                            //this.SendActivity(objActivity, objTime);
                        }
                            
                    }
                    
                }
            }
        }

        private void UpdateContactTrackingReporting()
        {
            //current data set does not meet the finctionality of this function yet
            return;
            foreach (Contact objContact in this.Contacts)
            {
            //Contact objContact = this.FindContactByEmail("edwalk@svsu.edu");
                string strUrl = $"reports/contact_reports/{objContact.contact_id}/activity_details?tracking_activities_list=em_sends,em_opens,em_clicks,em_bounces,em_optouts,em_forwards";
                string strResponce = this.ReadJsonFromUrl(strUrl);

                JObject objJson = JObject.Parse(strResponce);
                string strTracking = objJson.GetValue("tracking_activities").ToString();

                List<Dictionary<string, string>> lstTrackingEvents = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(strTracking);

                foreach (Dictionary<string, string> dctEvent in lstTrackingEvents)
                {
                    objContact.gdctTracking[dctEvent["tracking_activity_type"]].Add(this.gdctEmailCampaignActivities[dctEvent["campaign_activity_id"]]);
                }
                //Console.WriteLine(lstTrackingEvents.ToString());
                System.Threading.Thread.Sleep(250);
            }

        }

        private void UpdateContactOpenRate()
        {
            //current data set does not meet the finctionality of this function yet
            return;
            foreach (Contact objContact in this.Contacts)
            {
                //Contact objContact = this.FindContactByEmail("edwalk@svsu.edu");
                string strStart = DateTime.Now.AddYears(-5).AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss.ffZ", CultureInfo.InvariantCulture);
                string strEnd = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffZ", CultureInfo.InvariantCulture);
                string strUrl = $"reports/contact_reports/{objContact.contact_id}/open_and_click_rates?start={strStart}&end={strEnd}"; 
                string strResponce = this.ReadJsonFromUrl(strUrl);


                Dictionary<string, string> dctRating = JsonConvert.DeserializeObject<Dictionary<string, string>>(strResponce);

                objContact.open_rate = Convert.ToDouble(dctRating["average_open_rate"]);
                objContact.click_rate = Convert.ToDouble(dctRating["average_click_rate"]);
                objContact.included_activities_count = Convert.ToInt32(dctRating["included_activities_count"]);

                //Console.WriteLine(dctRating.ToString());
                System.Threading.Thread.Sleep(250);

            }
        }


        /// <summary>
        /// Used to make a web request and retrieve the JSON from the given URL
        /// </summary>
        /// 
        /// [TODO] this function will be changed to use the HttpClient class in order to make it cleaner. 
        /// 
        /// <param name="strUrl"> The URL extention that the request is to be made to </param>
        /// <returns></returns>
        private string ReadJsonFromUrl(string strUrl)
        {
           
            HttpWebRequest objAccessTokenRequest = (HttpWebRequest)WebRequest.Create(this.gstrBaseURL + strUrl);
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

        /// <summary>
        /// Used to update an api object with the url and json to put. 
        /// </summary>
        /// <param name="strJson">json to use as the data</param>
        /// <param name="strUrl">url to PUT to</param>
        private void PUTJson(string strJson, string strUrl)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", this.mstrTokenHeader);
            
            var data = new StringContent(strJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(this.gstrBaseURL + strUrl, data).Result;
        }

        /// <summary>
        /// Function to add contacts to CC
        /// </summary>
        /// <param name="strJson">contact string json</param>
        /// <param name="strUrl">post url</param>
        private void PostJson(string strJson, string strUrl)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", this.mstrTokenHeader);

            var data = new StringContent(strJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(this.gstrBaseURL+strUrl, data).Result;
            Console.WriteLine(response.StatusCode);
        }

        /// <summary>
        /// Used to delete a contact by its ID
        /// </summary>
        /// <param name="contact_id">string ID of contact to delete</param>
        private async void DELETEContact(string contact_id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", this.mstrTokenHeader);

            var response = await client.DeleteAsync(this.gstrBaseURL + $"contacts/{contact_id}");
            Console.WriteLine(response.StatusCode);
        }

        //temp function to add contacts
        public void massPost()
        {
            string[] cs = File.ReadAllText("C:\\Users\\evanw\\ownCloud\\School\\471\\test\\make_contacts\\out.txt").Split('|');
            int i = 1000;
            foreach(string c in cs)
            {
                Console.WriteLine(i--);
                this.PostJson(c, "contacts");
                System.Threading.Thread.Sleep(300);
            }
        }

        /// <summary>
        /// Loops though the contacts and contact lists to create in memory refrences for easy access
        /// </summary>
        private void ContactListAssignment()
        {
            //loop through all contacts
            foreach (KeyValuePair<string, Contact> objContact in this.gdctContacts)
            {
                //loop through all lists that the contact is apart of and add to that contacts glstContactLists
                foreach (string strListId in objContact.Value.list_memberships)
                {
                    objContact.Value.glstContactLists.Add(this.gdctContactLists[strListId]);
                    //also add contact to the list's members list
                    this.gdctContactLists[strListId].glstMembers.Add(objContact.Value);
                }
            }
        }

        /// <summary>
        /// Loops through the contacts and assigns them with the custom feilds that they have
        /// </summary>
        private void CustomFieldAssignment()
        {
            //loop through all contacts
            foreach (KeyValuePair<string, Contact> objContact in this.gdctContacts)
            {
                //loop through the custom fields that they have and add references
                foreach (GETContactCustomField objField in objContact.Value.custom_fields)
                {
                    objContact.Value.glstCustomFields.Add(this.gdctCustomFields[objField.custom_field_id]);
                    this.gdctCustomFields[objField.custom_field_id].glstContacts.Add(objContact.Value);
                }
            }
        }
        
        /// <summary>
        /// Given an email, returns the contact object 
        /// </summary>
        /// <param name="strEmail">string email</param>
        /// <returns>contact object, null if no match</returns>
        public Contact FindContactByEmail(string strEmail)
        {
            foreach (Contact objContact in this.gdctContacts.Values)
            {
                if (objContact.email_address.address.Trim() == strEmail.Trim())
                {
                    return objContact;
                }
            }
            return null;
        }

        /// <summary>
        /// return ContactList object by its Campaign name
        /// </summary>
        /// <param name="strname">Name of the Campaign </param>
        /// <returns>contact List, null if not found</returns>
        public ContactList FindListByName(string strname)
        {
            foreach (ContactList objList in this.gdctContactLists.Values){
                if (objList.name == strname.Trim())
                {
                    return objList;
                }
            }
            return null;
        }

        /// <summary>
        /// return Campaign Object by its campaign id
        /// </summary>
        /// <param name="strName">Name of the campaign</param>
        /// <returns></returns>
        public EmailCampaign FindCampaignByName(string strName)
        {
            foreach (EmailCampaign objTemp in this.gdctEmailCampaigns.Values)
            {
                if (objTemp.name == strName.Trim())
                {
                    return objTemp;
                }
            }
            return null;
        }

        /// <summary>
        /// provided an existing contact in the list, update that contact in Constant Contact
        /// </summary>
        /// <param name="objContact">Contact to update</param>
        public void Update(Contact objContact)
        {
            PUTContact objTemp = objContact.Update();
            string strUrl = $"contacts/{objContact.contact_id}";

            string strJson = JsonConvert.SerializeObject(objTemp,new JsonSerializerSettings 
            { 
                NullValueHandling = NullValueHandling.Ignore
            });
            this.PUTJson(strJson, strUrl);
        }


        /// <summary>
        /// Given a Contact List, update it on constant contact
        /// </summary>
        /// <param name="objContactList">Contact to update</param>
        public void Update(ContactList objContactList)
        {
            PUTContactList objTemp = objContactList.Update();
            string strUrl = $"contact_lists/{objContactList.list_id}";

            string strJson = JsonConvert.SerializeObject(objTemp, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            this.PUTJson(strJson, strUrl);
        }

        public void Create(Contact objContact)
        {
            this.gobjCCAuth.ValidateAuthentication();
            POSTContact objTempContact = objContact.Create();
            string strJson = JsonConvert.SerializeObject(objTempContact, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                
            });
            this.PostJson(strJson, "contacts");
        }

        // GAVIN
        public void Create(ContactList objContactList)
        {
            this.gobjCCAuth.ValidateAuthentication();
            POSTContactList objTempContactList = objContactList.Create();
            string strJson = JsonConvert.SerializeObject(objTempContactList, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,

            });
            this.PostJson(strJson, "contact_lists");
        }

        private async void DELETEContactList(string contactList_id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", this.mstrTokenHeader);

            var response = await client.DeleteAsync(this.gstrBaseURL + $"contact_lists/{contactList_id}");
            Console.WriteLine(response.StatusCode);
        }


        public void AddContactToContactList(ContactList objContactList, Contact objContact)
        {
            if (objContact.list_memberships.Count() >= 50)
            {
                Console.WriteLine("User is at max list count");
            }
            else
            {
                this.gobjCCAuth.ValidateAuthentication();

                JArray objContactIds = new JArray(objContact.contact_id);
                JProperty objContactProp = new JProperty("contact_ids", objContactIds);

                JObject objSource = new JObject(objContactProp);

                JProperty objSourceProp = new JProperty("source", objSource);



                JArray LstListIDs = new JArray(objContactList.list_id);
                JProperty objListProp = new JProperty("list_ids", LstListIDs);


                JObject objFinal = new JObject();
                objFinal.Add(objSourceProp);
                objFinal.Add(objListProp);



                string strFinalJson = JsonConvert.SerializeObject(objFinal);


                this.PostJson(strFinalJson, "/activities/add_list_memberships");
            }
        }


        /*
        public void AddContactToContactList(ContactList objContactList, Contact objContact)
        {
            if (objContact.list_memberships.Count() >= 50) {
                Console.WriteLine("User is at max list count");
            } 
            else
            {
                string id = objContactList.list_id;
                objContact.list_memberships.Add(id);
                this.Update(objContact);
            }
        }
        */

        public void RemoveContactFromContactList(ContactList objContactList, Contact objContact)
        {
            string id = objContactList.list_id;
            if (objContact.list_memberships.Remove(id))
            {
                this.Update(objContact);
            }
        }


        /// <summary>
        /// temp code for adding a campaign. need to work with Ben Dore on this stuff as it realies heavily on the UI. 
        /// </summary>
        public void AddCampaign()
        {
            EmailCampaign objTemp = new EmailCampaign()
            {
                name = "From Directers Po"
            };
            string html = "<!doctype html>\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n  <head>\r\n    <title>\r\n    </title>\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <style type=\"text/css\">\r\n      #outlook a{padding: 0;}\r\n      \t\t\t.ReadMsgBody{width: 100%;}\r\n      \t\t\t.ExternalClass{width: 100%;}\r\n      \t\t\t.ExternalClass *{line-height: 100%;}\r\n      \t\t\tbody{margin: 0; padding: 0; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;}\r\n      \t\t\ttable, td{border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;}\r\n      \t\t\timg{border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic;}\r\n      \t\t\tp{display: block; margin: 13px 0;}\r\n    </style>\r\n    <!--[if !mso]><!-->\r\n    <style type=\"text/css\">\r\n      @media only screen and (max-width:480px) {\r\n      \t\t\t  \t\t@-ms-viewport {width: 320px;}\r\n      \t\t\t  \t\t@viewport {\twidth: 320px; }\r\n      \t\t\t\t}\r\n    </style>\r\n    <!--<![endif]-->\r\n    <!--[if mso]> \r\n\t\t<xml> \r\n\t\t\t<o:OfficeDocumentSettings> \r\n\t\t\t\t<o:AllowPNG/> \r\n\t\t\t\t<o:PixelsPerInch>96</o:PixelsPerInch> \r\n\t\t\t</o:OfficeDocumentSettings> \r\n\t\t</xml>\r\n\t\t<![endif]-->\r\n    <!--[if lte mso 11]> \r\n\t\t<style type=\"text/css\"> \r\n\t\t\t.outlook-group-fix{width:100% !important;}\r\n\t\t</style>\r\n\t\t<![endif]-->\r\n    <style type=\"text/css\">\r\n      @media only screen and (max-width:480px) {\r\n      \r\n      \t\t\t  table.full-width-mobile { width: 100% !important; }\r\n      \t\t\t\ttd.full-width-mobile { width: auto !important; }\r\n      \r\n      }\r\n      @media only screen and (min-width:480px) {\r\n      .dys-column-per-90 {\r\n      \twidth: 90% !important;\r\n      \tmax-width: 90%;\r\n      }\r\n      }\r\n    </style>\r\n  </head>\r\n  <body>\r\n    <div>\r\n      <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:#f7f7f7;background-color:#f7f7f7;width:100%;'>\r\n        <tbody>\r\n          <tr>\r\n            <td>\r\n              <div style='margin:0px auto;max-width:600px;'>\r\n                <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>\r\n                  <tbody>\r\n                    <tr>\r\n                      <td style='direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;'>\r\n                        <!--[if mso | IE]>\r\n<table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top;width:540px;\">\r\n<![endif]-->\r\n                        <div class='dys-column-per-90 outlook-group-fix' style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>\r\n                          <table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'>\r\n                            <tbody>\r\n                              <tr>\r\n                                <td style='background-color:#ffffff;border:1px solid #ccc;padding:45px 75px;vertical-align:top;'>\r\n                                  <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='' width='100%'>\r\n                                    <tr>\r\n                                      <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>\r\n                                        <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>\r\n                                          <tbody>\r\n                                            <tr>\r\n                                              <td style='width:130px;'>\r\n                                                <img alt='Profile Picture' height='auto' src='https://assets.opensourceemails.com/imgs/oxygen/Ei7o4zRgT561k4rLfzTz_profile_pic.jpg' style='border:1px solid #ccc;border-radius:5px;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='130' />\r\n                                              </td>\r\n                                            </tr>\r\n                                          </tbody>\r\n                                        </table>\r\n                                      </td>\r\n                                    </tr>\r\n                                    <tr>\r\n                                      <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>\r\n                                        <div style='color:#777777;font-family:Oxygen, Helvetica neue, sans-serif;font-size:14px;line-height:21px;text-align:center;'>\r\n                                          <a href='#' style='display:block; color: #ff6f6f; font-weight: bold; text-decoration: none;'>\r\n                                            @First Name\r\n                                          </a>\r\n                                          <span>\r\n                                            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam sed nulla nibh. Ut hendrerit pellentesque justo, semper accumsan nisl venenatis ut.\r\n                                          </span>\r\n                                        </div>\r\n                                      </td>\r\n                                    </tr>\r\n                                    <tr>\r\n                                      <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;' vertical-align='middle'>\r\n                                        <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:separate;line-height:100%;'>\r\n                                          <tr>\r\n                                            <td align='center' bgcolor='#ff6f6f' role='presentation' style='background-color:#ff6f6f;border:none;border-radius:5px;cursor:auto;padding:10px 25px;' valign='middle'>\r\n                                              <a href='# Button URL' style='background:#ff6f6f;color:#ffffff;font-family:Oxygen, Helvetica neue, sans-serif;font-size:14px;font-weight:400;line-height:21px;margin:0;text-decoration:none;text-transform:none;' target='_blank'>\r\n                                                Button Text\r\n                                              </a>\r\n                                            </td>\r\n                                          </tr>\r\n                                        </table>\r\n                                      </td>\r\n                                    </tr>\r\n                                  </table>\r\n                                </td>\r\n                              </tr>\r\n                            </tbody>\r\n                          </table>\r\n                        </div>\r\n                        <!--[if mso | IE]>\r\n</td></tr></table>\r\n<![endif]-->\r\n                      </td>\r\n                    </tr>\r\n                  </tbody>\r\n                </table>\r\n              </div>\r\n            </td>\r\n          </tr>\r\n        </tbody>\r\n      </table>\r\n    </div>\r\n  </body>\r\n</html>";
            objTemp.AddActivity(strFromEmail: "ewalk@svsu.edu", strFromName: "Directers Portal", strReplayToEmail:"None@None.com", strSubject: "test number 1", strHTMLContent: html);

            string strJson = JsonConvert.SerializeObject(objTemp.objNewCampaign(), new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            this.PostJson(strJson, "/emails");

            this.UpdateEmailCampaigns();
            this.UpdateEmailCampaignActivities();

            EmailCampaignActivity t = null;
            foreach (EmailCampaignActivity objUpdatedActivity in this.FindCampaignByName(objTemp.name).Activities)
            {
                if (objUpdatedActivity.role == "primary_email")
                {
                    t = objUpdatedActivity;
                }
            }
            if (t != null)
            {
                Console.WriteLine("Updating Contact List");
                t.contact_list_ids.Add(this.FindListByName("Usable List").list_id);

                strJson = JsonConvert.SerializeObject(t.Update(), new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                this.PUTJson(strJson, $"/emails/activities/{t.campaign_activity_id}");
            }
        }
    
        public void SendActivity(EmailCampaignActivity objActivity, DateTime objTime)
        {
            print(objActivity.contact_list_ids[0]);
            if (objActivity.contact_list_ids.Count()>0)
            {
                string strUrl = $"emails/activities/{objActivity.campaign_activity_id}/schedules";
                string strdate = objTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);

                string strData = $"{{\"scheduled_date\": \"0\"}}";

                this.PostJson(strData, strUrl);
                Console.WriteLine("send activity");
            }
            else
            {
                throw new FormatException();
            }
        }

        private void save()
        {
            string strContacts = JsonConvert.SerializeObject(this.gdctContacts, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            string strContactLists = JsonConvert.SerializeObject(this.gdctContactLists, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            string strCustomFields = JsonConvert.SerializeObject(this.gdctCustomFields, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            string strCampaigns = JsonConvert.SerializeObject(this.gdctEmailCampaigns, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            string strActivities = JsonConvert.SerializeObject(this.gdctEmailCampaignActivities, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            string strActivityPreviews = JsonConvert.SerializeObject(this.glstEmailCampaignActivityPreviews, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            string fname = "CCSaveData.JSON";

            Dictionary<string, string> dctData = new Dictionary<string, string>()
            {
                { "contacts",strContacts },
                { "contactlists",strContactLists },
                { "customfields",strCustomFields },
                { "campaigns",strCampaigns },
                { "activities",strActivities },
                { "activitypreviews",strActivityPreviews }
            };

            string strData = JsonConvert.SerializeObject(dctData);

            File.WriteAllText(fname, strData, Encoding.UTF8);
        }

        private void load()
        {
            string strData = File.ReadAllText("CCSaveData.JSON", Encoding.UTF8);

            Dictionary<string, string> dctData = JsonConvert.DeserializeObject<Dictionary<string, string>>(strData);

            this.gdctContacts = JsonConvert.DeserializeObject<Dictionary<string, Contact>>(dctData["contacts"]);
            this.gdctContactLists = JsonConvert.DeserializeObject<Dictionary<string, ContactList>>(dctData["contactlists"]);
            this.gdctCustomFields = JsonConvert.DeserializeObject<Dictionary<string, CustomField>>(dctData["customfields"]);
            this.gdctEmailCampaigns = JsonConvert.DeserializeObject<Dictionary<string, EmailCampaign>>(dctData["campaigns"]);
            this.gdctEmailCampaignActivities = JsonConvert.DeserializeObject<Dictionary<string, EmailCampaignActivity>>(dctData["activities"]);
            this.glstEmailCampaignActivityPreviews = JsonConvert.DeserializeObject<List<EmailCampaignActivityPreview>>(dctData["activitypreviews"]);
            print("load done");
        }

        public void print(string s)
        {
            Console.WriteLine(s);
        }
    }
}
