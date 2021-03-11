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
	public class ConstantContactQueue
	{
        /// <summary>
        /// This class is for processing various forms of requests and sending them to
        /// Constant Contact. Additionally this maintains the request limit.
        /// </summary>
        private int intLimitReq = 0;
        //May need to be instantiated elsewhere
        private List<Request> lstRequests = new List<Request>();

        /// <summary>
        /// Counts the requests being sent to ensure the waiting period between multiple 
        /// requests is accounted for
        /// </summary>
        public void CountRequests()
        {
            if (intLimitReq >= 4) 
            { 
                System.Threading.Thread.Sleep(1000);
                intLimitReq = 0;
            }
            intLimitReq++;
        }

        /// <summary>
        /// Records requests as they are sent to the list in order with priority given to GET Requests
        /// </summary>
        /// <param name="GObjRequest"></param>
        public void RequestEnqueue(Request GObjRequest)
        {
            if (GObjRequest.strFlag.Equals("Get") && lstRequests.Count > 0)
            {
                for(int i = 0; i < lstRequests.Count; i++)
                {
                    if (!lstRequests[i].Equals("Get"))
                        lstRequests.Insert(i, GObjRequest);
                }
            }
            else
                lstRequests.Add(GObjRequest);
        }

        /// <summary>
        /// Dequeues the list sending each request to Constant Contact as it goes
        /// </summary>
        public void RequestDequeue()
        {
            while (lstRequests.Count > 0)
            {
                if (lstRequests[0].strFlag.Equals("Get"))
                {
                    GetRequest(lstRequests[0]);
                }
                else if (lstRequests[0].strFlag.Equals("Post"))
                {
                    PostRequest(lstRequests[0]);
                }
                else if (lstRequests[0].strFlag.Equals("Pull"))
                {
                    PullRequest(lstRequests[0]);
                }
                else if (lstRequests[0].strFlag.Equals("Delete"))
                {
                    DeleteRequest(lstRequests[0]);
                }
                lstRequests.RemoveAt(0);
            }
        }

        //TODO

        public void GetRequest(Request objGetRequest)
        {
            CountRequests();
        }

        public void PostRequest(Request objPostRequest)
        {
            CountRequests();
        }

        public void PullRequest(Request objPullRequest)
        {
            CountRequests();
        }

        public void DeleteRequest(Request objDelRequest)
        {
            CountRequests();
        }

    }
}
