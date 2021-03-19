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
using System.Collections;

namespace DirectorsPortalConstantContact
{
    /// <summary>
    /// The request is an object type that will be used for sending and recieveing information
    /// from Costant Contact
    /// </summary>
    public class Request
    {
        /// <summary>
        /// This is a placeholder flag value to determine the request type
        /// </summary>
        public string strFlag { get; set; }
        public string strJSON { get; set; }
        public string strURL { get; set; }
        public string strResponse { get; set; }
        public Request(string strFlag, string strJSON, string strURL)
        {
            this.strFlag = strFlag;
            this.strJSON = strJSON;
            this.strURL = strURL;
        }



    }
}
