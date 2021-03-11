using System;
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
        public List<Contact> GlstMembers = new List<Contact>();

        public PUTContactList Update()
        {
            return new PUTContactList() {
                name = this.name,
                favorite = this.favorite,
                description = this.description
            };
        }

    }
}
