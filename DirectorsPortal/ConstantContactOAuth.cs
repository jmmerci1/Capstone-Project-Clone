using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace DirectorsPortal
{
    class ConstantContactOAuth
        //keep an eye on testing this class. after relocated the methods, the returned JS would fail to close the current tab. no idea how, but i fixed it. 
    {

        readonly private HttpListener objLocalListener = new HttpListener();
        private string mstrAccessToken;
        private string mstrRefreshToken;
        private string mstrLocalRoute;
        private string mstrEncodedRedirect;
        readonly private string mstrOAuthUrlPart1 = "https://idfed.constantcontact.com/as/token.oauth2?code=";
        readonly private string mstrOAuthUrlPart2 = "&grant_type=authorization_code&redirect_uri=";
        public string mstrAppAPIKey;
        public string mstrAppAPISecret;

        public string LocalRoute
        {
            get
            {
                return this.mstrLocalRoute;
            }
            set
            {
                if (value.Contains("localhost"))
                {
                    this.mstrLocalRoute = value;
                    this.mstrEncodedRedirect = value.Remove(value.Length - 1, 1).Replace(":", "%3A").Replace("/", "%2F");
                }
                else
                {
                    throw new Exception("Invalid redirect uri for Constant Contact api V3 URL");
                }
            }
        }

        public string AccessToken
        {
            get
            {
                return this.mstrAccessToken;
            }
        }

        public string RefreshToken
        {
            get
            {
                return this.mstrRefreshToken;
            }
        }

        public void GetAccessToken()
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            }

            this.SetPrefixes();

            //other data validation
            if (!(this.LocalRoute == null | this.mstrAppAPIKey == null | this.mstrAppAPISecret == null))
            {
                this.OpenChromeToRedirect();
                this.ManageListener();
            }
            else
            {
                //handle not ready state
                Console.WriteLine("OOF");
            }

        }

        private void SetPrefixes()
        {
            if (this.mstrLocalRoute != null)
            {
                this.objLocalListener.Prefixes.Add(this.mstrLocalRoute);
            }
        }

        private void OpenChromeToRedirect()
        {
            //authorization link
            try
            {
                string strAccessCodeURL = $"https://api.cc.email/v3/idfed?client_id={this.mstrAppAPIKey}&response_type=code&scope=contact_data+campaign_data+account_read+account_update&redirect_uri={this.mstrEncodedRedirect}";
                System.Diagnostics.Process.Start(strAccessCodeURL);

            }
            catch (System.ComponentModel.Win32Exception)
            {
                Console.WriteLine("oof");
            };
        }

        private void ManageListener()
        {

            // Note: The GetContext method blocks while waiting for a request.
            this.objLocalListener.Start();
            HttpListenerContext objHttpContext = this.objLocalListener.GetContext();
            this.GetAccessCode(objHttpContext.Request);

            // Obtain a response object.
            this.HandleHttpConnectionResponse(objHttpContext.Response);


            this.objLocalListener.Stop();
        }

        private void GetAccessCode(HttpListenerRequest objRequest)
        {
            string strAccesscode = objRequest.QueryString["code"];

            string strOAuthUrl = this.mstrOAuthUrlPart1 + strAccesscode + this.mstrOAuthUrlPart2 + this.mstrEncodedRedirect;

            //Console.Write(OAuthUrl);
            string strHttpResponse = ReadOAuthContents(strOAuthUrl);
            this.ParseTokenFromHTTP(strHttpResponse);

        }

        private void HandleHttpConnectionResponse(HttpListenerResponse objRecvResponse)
        {
            // Construct a response.
            string strResponse = "<script>close();</script>";
            byte[] bytBufferArray = Encoding.UTF8.GetBytes(strResponse);

            // Get a response stream and write the response to it.
            objRecvResponse.ContentLength64 = bytBufferArray.Length;
            Stream objResponseStream = objRecvResponse.OutputStream;
            objResponseStream.Write(bytBufferArray, 0, bytBufferArray.Length);

            // You must close the output stream.
            objResponseStream.Close();

        }

        private string ReadOAuthContents(string strUrl)
        {

            HttpWebRequest objAccessTokenRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            objAccessTokenRequest.Headers["Authorization"] = this.GenerateHeader();

            objAccessTokenRequest.Method = "POST";
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

            return strHttpResponse;

        }

        private void ParseTokenFromHTTP(string strHttpResponse)
        {

            Dictionary<string, string> dctDecodedJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(strHttpResponse);
            this.mstrAccessToken = dctDecodedJson["access_token"];
            this.mstrRefreshToken = dctDecodedJson["refresh_token"];

        }

        private string GenerateHeader()
        {
            return "Basic " + this.Base64Encode($"{this.mstrAppAPIKey}:{this.mstrAppAPISecret}");
        }

        private string Base64Encode(string strValue)
        {
            UTF8Encoding objUTF8 = new UTF8Encoding();
            byte[] bytValueArray = objUTF8.GetBytes(strValue);
            return Convert.ToBase64String(bytValueArray);
        }

    }
}
