using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorsPortal
{
    class Contact
    {
        ///this will be the main contact obj that everything will interact with
        ///this will have a method that will translate a GETContact to Contact 
        ///this will also have a toPUT and toPOST to return the object that be serialized for the coresponding request

        public GETContact contact;

        public static Contact FillFromGET(GETContact contact)
        {
            Contact c = new Contact();
            //this will assign all of the properties/variables to this obj, not to a GETContact obj
            c.contact = contact;
            return c;
        }
    }
}
