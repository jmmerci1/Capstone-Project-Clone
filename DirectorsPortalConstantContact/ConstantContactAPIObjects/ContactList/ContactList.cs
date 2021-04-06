﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorsPortalConstantContact
{
    /// <summary>
    /// the main reference point for ContactLists from Constant Contact. 
    /// This will have a few helper properties and meathods, but will 
    /// mainly be here for holding the data that has been retrieved from constant contact 
    /// </summary>
    public class ContactList : GETContactList
    {
        [Newtonsoft.Json.JsonIgnore]
        public List<Contact> glstMembers = new List<Contact>();

        public ContactList(string strName=null,string strDesc=null)
        {
            this.name = strName;
            this.description = strDesc;
        }
        public ContactList()
        {
            this.name = null;
        }

        public PUTContactList Update()
        {
            return new PUTContactList() {
                name = this.name,
                favorite = this.favorite,
                description = this.description
            };
        }

        public POSTContactList Create()
        {
            if (String.IsNullOrEmpty(this.name))
            {
                throw new Exception();
            }

            POSTContactList objTempList = new POSTContactList()
            {
                name = this.name,
                favorite = this.favorite,
                description = this.description
            };


            return objTempList;
        }

    }


}
