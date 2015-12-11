using NewDurnit.Interfaces;
using NewDurnit;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NewDurnit
{
    public class NameNode : INode
    {
        private string ourDurnitOp = "X-DurnitOp";
        private List<DataNodeInfo> log;
        private string URI;

        public NameNode(string Address, string Port)
        {
            URI = "http://" + Address + ":" + Port + "/";
            log = new List<DataNodeInfo>();
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(URI);
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(handleRequest), listener);
            //while (true) { }
        }

        private void handleRequest(IAsyncResult ar)
        {
            HttpListener listener = (HttpListener)ar.AsyncState;
            listener.BeginGetContext(new AsyncCallback(handleRequest), listener);
            Console.WriteLine("NameNode : handling request");
            Console.WriteLine("Queued up another one: " + this.URI);

            HttpListenerContext context = listener.EndGetContext(ar);
            HttpListenerRequest request = context.Request;
            NameValueCollection requestHeaders = context.Request.Headers;
            HttpListenerResponse response = context.Response;


            string durnitOp = requestHeaders.Get(ourDurnitOp).ToLower();

            switch (durnitOp)
            {
                case "getdatanodes":
                    handleGetDataNodes(request, response);
                    break;
                case "heartbeat":
                    handleHeartBeat(request, response);
                    break;
                case "getfile":
                    handleGetFile(request, response);
                    break;
                case "init":
                    response.StatusCode = 200;
                    break;
                default:
                    response.StatusCode = 404;
                    break;
            }
            response.Close();
        }

        private void handleGetFile(HttpListenerRequest request, HttpListenerResponse response)
        {
            JsonSerializer serializer = new JsonSerializer();
            string contentDisposition = request.Headers["content-disposition"];
            string file = contentDisposition.Split('=')[1];

            string URIToReturn = "";

            foreach (var dataNode in log)
            {
                if (dataNode.Files.Contains(file))
                {
                    URIToReturn = dataNode.URIAddress;
                    break;
                }
            }

            if (URIToReturn.Equals(""))
                response.StatusCode = 404;

            JSONWriteToStream(response.OutputStream, URIToReturn);
            response.Close();            
        }

        private void handleHeartBeat(HttpListenerRequest request, HttpListenerResponse response)
        {
            JsonSerializer serializer = new JsonSerializer();
            DataNodeInfo sentInfo;
            using (StreamReader reader = new StreamReader(request.InputStream))
            using (JsonReader JsonRead = new JsonTextReader(reader))
            {
                sentInfo = (DataNodeInfo)serializer.Deserialize(JsonRead, typeof(DataNodeInfo));
            }

            lock (log)
            {
                DataNodeInfo correspondingInfo = log.FirstOrDefault(x => x.URIAddress == sentInfo.URIAddress);
                if (correspondingInfo != null)
                {
                    Console.WriteLine(correspondingInfo.URIAddress + " HEART BEAT HANDLED");
                    correspondingInfo.Files = sentInfo.Files;
                    correspondingInfo.HowManyFriends = sentInfo.HowManyFriends;
                }
                else
                    log.Add(sentInfo);
            }

            response.StatusCode = 200;
            response.Close();
            sendOverNewFriends(sentInfo);
        }

        private void sendOverNewFriends(DataNodeInfo currentDataNode)
        {
            Console.WriteLine(currentDataNode.URIAddress + " : " + currentDataNode.HowManyFriends);
            if (needMoreFriends(currentDataNode))
            {
                DataNodeInfo[] newFriends = determineNewFriends(currentDataNode);
                List<string> urisToSend = new List<string>();
                foreach (DataNodeInfo friend in newFriends)
                {
                    urisToSend.Add(friend.URIAddress);
                }
                HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(currentDataNode.URIAddress);
                newRequest.Headers.Add(ourDurnitOp, "NewFriends");
                newRequest.Method = "POST";
                JSONWriteToStream(newRequest.GetRequestStream(), urisToSend);
                WebResponse response = newRequest.GetResponse();
                response.Close();
                response.Dispose();
            }
        }

        private void JSONWriteToStream(Stream stream, object whatToWrite)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(stream))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, whatToWrite);
            }
        }

        private bool needMoreFriends(DataNodeInfo info)
        {
            int properAmountOfFriends = log.Count / 2 + 1;
            return info.HowManyFriends <= properAmountOfFriends;
        }

        private DataNodeInfo[] determineNewFriends(DataNodeInfo currentDataNode)
        {
            int howMany = log.Count / 2 + 1;
            return log.Where(x => x.URIAddress != currentDataNode.URIAddress).OrderBy(x => x.HowManyFriends).Take(howMany).ToArray();
            //for (int i = 0; i < howMany; i++)
            //{
            //    if(!sorted[i].URIAdress.Equals(currentDataNode.URIAdress))
            //        friends[]
            //}

            //HashSet<int> indecies = new HashSet<int>();
            //Random generator = new Random();
            //while (indecies.Count != 4)
            //{
            //    indecies.Add(generator.Next(log.Count));
            //}
            //List<DataNodeInfo> returningList = new List<DataNodeInfo>();
            //foreach (int index in indecies)
            //{
            //    returningList.Add(log[index]);
            //}
            //return returningList.ToArray();
        }

        //expecting GetDatanodes:(number)
        private void handleGetDataNodes(HttpListenerRequest request, HttpListenerResponse response)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            DataNodeInfo[] nodesToSend = getDataNodesFromCount(1);

            string UrisToSend = nodesToSend[0].URIAddress;

            JSONWriteToStream(response.OutputStream, UrisToSend);
            response.StatusCode = 200;
        }

        private DataNodeInfo[] getDataNodesFromCount(int howManyToReturn)
        {
            return log.OrderBy(x => x.Files.Count).Take(howManyToReturn).ToArray();
        }
    }
}
