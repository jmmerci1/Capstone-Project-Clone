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
    {

        readonly private HttpListener LocalListener = new HttpListener();
        private string _accessToken;
        private string _refreshToken;
        private string _localRoute;
        private string _encodedRedirect;
        readonly private string _oAuthUrl_1 = "https://idfed.constantcontact.com/as/token.oauth2?code=";
        readonly private string _oAuthUrl_2 = "&grant_type=authorization_code&redirect_uri=";
        public string AppAPIKey;
        public string AppAPISecret;


        public string LocalRoute
        {
            get
            {
                return this._localRoute;
            }
            set
            {
                if (value.Contains("localhost"))
                {
                    this._localRoute = value;
                    this._encodedRedirect = value.Remove(value.Length - 1, 1).Replace(":", "%3A").Replace("/", "%2F");
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
                return this._accessToken;
            }
        }

        public string RefreshToken
        {
            get
            {
                return this._refreshToken;
            }
        }

        private string Base64Encode(string value)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedValue = utf8.GetBytes(value);
            return Convert.ToBase64String(encodedValue);
        }

        private void SetPrefixes()
        {
            if (this._localRoute != null)
            {
                this.LocalListener.Prefixes.Add(this._localRoute);
            }
        }

        private string GenerateAccessCodeUrl()
        {
            return $"https://api.cc.email/v3/idfed?client_id={this.AppAPIKey}&response_type=code&redirect_uri={this._encodedRedirect}";
        }
        
        private void OpenChromeToRedirect()
        {
            try
            {
                System.Diagnostics.Process.Start(this.GenerateAccessCodeUrl());

            }
            catch (System.ComponentModel.Win32Exception)
            {
                Console.WriteLine("oof");
            };
        }

        private void ParseTokenFromHTTP(string httpdata)
        {
            Dictionary<string, string> JSON = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpdata);
            this._accessToken = JSON["access_token"];
            this._refreshToken = JSON["refresh_token"];
        }

        private void GetAccessCode(HttpListenerRequest request)
        {
            string access_code = request.QueryString["code"];

            string OAuthUrl = this._oAuthUrl_1 + access_code + this._oAuthUrl_2 + this._encodedRedirect;

            //Console.Write(OAuthUrl);
            string httpdata = ReadOAuthContents(OAuthUrl);
            this.ParseTokenFromHTTP(httpdata);

        }

        private string GenerateHeader()
        {
            string value = "Basic " + this.Base64Encode($"{this.AppAPIKey}:{this.AppAPISecret}");
            return value;
        }

        private string ReadOAuthContents(string url)
        {

            HttpWebRequest accessTokenRequest = (HttpWebRequest)WebRequest.Create(url);
            accessTokenRequest.Headers["Authorization"] = this.GenerateHeader();

            accessTokenRequest.Method = "POST";
            HttpWebResponse responce = (HttpWebResponse)accessTokenRequest.GetResponse();

            Stream streamResponse = responce.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            Char[] readBuff = new Char[256];
            int count = streamRead.Read(readBuff, 0, 256);

            string contents = "";

            while (count > 0)
            {
                String outputData = new String(readBuff, 0, count);
                contents += outputData;
                count = streamRead.Read(readBuff, 0, 256);
            }

            return contents;

        }

        private void HandleHttpConnectionResponse(HttpListenerResponse response)
        {
            // Construct a response.
            string responseString = "<script>close();</script>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            // You must close the output stream.
            output.Close();

        }

        private void ManageListener()
        {

            // Note: The GetContext method blocks while waiting for a request.
            this.LocalListener.Start();
            HttpListenerContext context = this.LocalListener.GetContext();
            this.GetAccessCode(context.Request);

            // Obtain a response object.
            this.HandleHttpConnectionResponse(context.Response);


            this.LocalListener.Stop();
        }

        public void GetAccessToken()
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            }

            this.SetPrefixes();

            //other data validation
            if (!(this.LocalRoute ==null | this.AppAPIKey ==null | this.AppAPISecret == null))
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
    }
}
