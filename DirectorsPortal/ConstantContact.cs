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
        readonly private HttpListener objLocalListener = new HttpListener();
        readonly private string strBaseURL = "https://api.cc.email/v3/";
        readonly private string strAccountEmail = "contacts";

        ConstantContactOAuth CCO = new ConstantContactOAuth();

        private string strTokenHeader
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

        public void GetEmailAccounts()
        {

            string ye = this.ReadJsonFromUrl(this.strBaseURL + this.strAccountEmail);

            Dictionary<string, string> dctDecodedJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(ye);

        }


        private string ReadJsonFromUrl(string strUrl)
        {

            HttpWebRequest objAccessTokenRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            objAccessTokenRequest.Headers["Authorization"] = this.strTokenHeader;
            Console.WriteLine(this.strTokenHeader);
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
            Console.WriteLine(strHttpResponse);
            return strHttpResponse;

        }

    }
}
