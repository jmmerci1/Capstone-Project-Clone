using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Collections;

namespace DirectorsPortalConstantContact
{
	public class ConstantContactQueue
	{
        /// <summary>
        /// This class is for processing various forms of requests and sending them to
        /// Constant Contact. Additionally this maintains the request limit.
        /// </summary>
        readonly private string gstrBaseURL = "https://api.cc.email/v3/";
        //May need to be instantiated elsewhere
        private List<Request> lstRequests = new List<Request>();
        private ConstantContactOAuth gobjCCAuth = new ConstantContactOAuth()
        {
            MstrLocalRoute = "http://localhost:40000/",
            mstrAppAPIKey = "08d80131-0c76-4829-83fc-be50e14bf0b4",
            mstrAppAPISecret = "HvdbdEaUYXhVYQcUV2XEXg"
        };
        private string mstrTokenHeader => $"Bearer {this.gobjCCAuth.MstrAccessToken}";
        private Thread RequestThread;

        public ConstantContactQueue()
        {
            this.RequestThread = new Thread(new ThreadStart(this.RequestDequeueThreadTask))
            {
                IsBackground = true
            };
            this.RequestThread.Start();

        }


        private async void RequestDequeueThreadTask()
        {
            List<Request> lstWindow = new List<Request>();
            //initial time of [0] task in window
            int intSecondPointer = 0;

            while (true)
            {
                if (this.lstRequests.Count == 0)
                {
                    Thread.Sleep(150);
                }
                else
                {
                    //validate win size and time
                    if (lstWindow.Count == 4)
                    {
                        //req sent at .5, now is .75
                        // dif = .25
                        //sleep = 1 - dif = .75
                        //sleep till onesecond after intSecondPointer request
                        int intDifference = Convert.ToInt32((DateTime.Now - lstWindow[intSecondPointer].objRequestTime).TotalMilliseconds);

                        Thread.Sleep(1-intDifference);
                        intSecondPointer++;

                    }

                    //once there is time
                    lstWindow.Add(this.RequestDequeueAndSend());

                }
                //have to iterate in reverse so the shifting values do not messup the loop
                for (int i = lstWindow.Count - 1; i >= 0; i--)
                {
                    Request objRequest = lstWindow[i];

                    if (objRequest.objTask.IsCompleted)
                    {
                        objRequest.strResponse = await objRequest.objTask.Result.Content.ReadAsStringAsync();
                        objRequest.bolState = true;
                        lstWindow.RemoveAt(i);
                        if (i < intSecondPointer)
                        {
                            intSecondPointer--;
                        }
                    }
                }

            }

        }



        /// <summary>
        /// Records requests as they are sent to the list in order with priority given to GET Requests
        /// </summary>
        /// <param name="GObjRequest"></param>
        public void SendRequest(Request GObjRequest)
        {
            if (GObjRequest.strFlag.Equals("Get") && lstRequests.Count > 0)
            {
                for(int i = 0; i < lstRequests.Count; i++)
                {
                    if (!lstRequests[i].strFlag.Equals(Request.MstrGET))
                        lstRequests.Insert(i, GObjRequest);
                }
            }
            else
                lstRequests.Add(GObjRequest);
        }

        /// <summary>
        /// Dequeues the list sending each request to Constant Contact as it goes
        /// </summary>
        private Request RequestDequeueAndSend()
        {
            gobjCCAuth.ValidateAuthentication();

            Request objReq = lstRequests[0];
            lstRequests.RemoveAt(0);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", this.mstrTokenHeader);

            objReq.objTask = client.GetAsync(this.gstrBaseURL + objReq.strURL);
            objReq.objRequestTime = DateTime.Now;
            return objReq;

            /*if (lstRequests[0].strFlag.Equals(Request.MstrGET))
            {
                return Task.Run(() =>
                {
                    return client.GetAsync(this.gstrBaseURL + objReq.strURL);
                });
            }
            else if (lstRequests[0].strFlag.Equals(Request.MstrPOST))
            {
                PostRequestAsync(objReq);
            }
            else if (lstRequests[0].strFlag.Equals(Request.MstrPUT))
            {
                PutRequestAsync(objReq);
            }
            else if (lstRequests[0].strFlag.Equals(Request.MstrDELETE))
            {
                DeleteRequestAsync(objReq);
            }*/

        }


        private async void PostRequestAsync(Request objPostRequest)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", this.mstrTokenHeader);

            var data = new StringContent(objPostRequest.strJSON, Encoding.UTF8, "application/json");

            HttpResponseMessage objResponse = client.PostAsync(this.gstrBaseURL + objPostRequest.strURL, data).Result;
            objPostRequest.strResponse = await objResponse.Content.ReadAsStringAsync();
        }

        private async void PutRequestAsync(Request objPullRequest)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", this.mstrTokenHeader);

            var data = new StringContent(objPullRequest.strJSON, Encoding.UTF8, "application/json");

            HttpResponseMessage objResponse = client.PutAsync(this.gstrBaseURL + objPullRequest.strURL, data).Result;
            objPullRequest.strResponse = await objResponse.Content.ReadAsStringAsync();
        }

        public async void DeleteRequestAsync(Request objDelRequest)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", this.mstrTokenHeader);

            HttpResponseMessage objResponse = client.DeleteAsync(this.gstrBaseURL + objDelRequest.strURL).Result;
            objDelRequest.strResponse = await objResponse.Content.ReadAsStringAsync();
        }

    }
}
