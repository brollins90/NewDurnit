using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NewDurnit
{
    public class Client
    {
        //public static void Main(string[] args)
        //{
        //    Program program = new Program();
        //    FileStream fileStream = File.Create(@"C:\Users\Diego Suarez\Desktop\testfile.txt");
        //    program.StoreFile(fileStream);
        //}
        private string NameNodeURI;
        public Client(string Address, string Port)
        {
            NameNodeURI = "http://" + Address + ":" + Port + "/";
        }

        public void StoreFile(FileStream fileStream)
        {
            SendFile(GetDataNodesOp(), fileStream);
        }

        private string GetDataNodesOp()
        {
            WebRequest request = WebRequest.Create(NameNodeURI);

            request.Headers.Add("X-DurnitOp", "GetDatanodes");

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream stream = response.GetResponseStream();

            StreamReader sr = new StreamReader(stream);
            JsonTextReader jtr = new JsonTextReader(sr);

            JsonSerializer serializer = new JsonSerializer();
            string URI = (string)serializer.Deserialize(jtr);

            response.Close();

            return URI;
        }

        public void SendFile(string URI, FileStream fileStream)
        {
            Console.WriteLine(URI);
            WebRequest request = WebRequest.Create(URI);

            byte[] file = new byte[fileStream.Length];
            fileStream.Read(file, 0, (int)fileStream.Length);

            string[] s = fileStream.Name.Split('\\');
            string fileName = s[s.Length - 1];

            string contentDisposition = "attachment; filename=" + fileName;
            request.Headers.Add("X-DurnitOp", "DataCreation");
            request.Headers.Add("content-disposition", contentDisposition);
            request.Method = "POST";

            Stream stream = request.GetRequestStream();
            stream.Write(file, 0, file.Length);
            WebResponse response = request.GetResponse();

            response.Close();
        }

        public byte[] GetFile(string fileName)
        {
            WebRequest request = WebRequest.Create(NameNodeURI);

            request.Headers.Add("X-DurnitOp", "GetFile");

            string contentDisposition = "attachment; filename=" + fileName;
            request.Headers.Add("content-disposition", contentDisposition);
            request.Method = "GET";

            string URI = "";
            WebResponse response = request.GetResponse();
            using(Stream stream = response.GetResponseStream())
            using(StreamReader sr = new StreamReader(stream))
            {
                JsonTextReader jtr = new JsonTextReader(sr);

                JsonSerializer serializer = new JsonSerializer();
                URI = (string)serializer.Deserialize(jtr);

                response.Close();
            }

            WebRequest request2 = WebRequest.Create(URI);
            request2.Method = "GET";
            request2.Headers.Add("X-DurnitOp", "GetFile");
            request2.Headers.Add("content-disposition", contentDisposition);

            WebResponse response2 = request2.GetResponse();
            Stream stream2 = response2.GetResponseStream();

            byte[] file = new byte[response2.ContentLength];
            stream2.Read(file, 0, file.Length);

            response2.Close();

            return file;
        }
    }
}
