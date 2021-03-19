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

        public readonly static string MstrPUT = "PUT";
        public readonly static string MstrGET = "GET";
        public readonly static string MstrPOST = "POST";
        public readonly static string MstrDELETE = "DELETE";


        public string strFlag { get; set; }
        public string strJSON { get; set; }
        public string strURL { get; set; }
        public string strResponse { get; set; }

        public bool bolState = false;
        public DateTime objRequestTime;

        public Task<HttpResponseMessage> objTask;

        public Request(string strFlag, string strURL, string strJSON="")
        {
            this.strFlag = strFlag;
            this.strJSON = strJSON;
            this.strURL = strURL;
        }


    }
}
