using NewDurnit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
  public class FilesController : ApiController
  {
    private FileDictionary fileDictionary = FileDictionary.Instance;

    //public FilesController()
    //{
    //  Client client = new Client(args[0], "2020");
    //  client.StoreFile(File.OpenRead(@"C:\Users\Elijah Segura\Desktop\testingText.txt"));
    //  byte[] array = client.GetFile(@"testingText.txt");
    //  File.WriteAllBytes(@"C:\Users\Elijah Segura\Desktop\RECEIVED.txt", array);

    //}
    // GET api/values
    public IEnumerable<string> Get()
    {
      return fileDictionary.GetAllFiles();
      //Client client = new Client("localhost", "2020");
      //client.GetFile("");
      //return new string[] { "value1", "value2" };
    }

    // GET api/values/5
    public string Get(string file)
    {
      var path = fileDictionary.GetFilePath(file);
      Client client = new Client("localhost", "2020");
      byte[] array = client.GetFile(path);
      return array.ToString();
    }

    // POST api/values
    public void Post([FromBody]string value, string file)
    {
      Guid id = Guid.NewGuid();
      var filePath = $@"C:\_\temp\{id}.txt";
      fileDictionary.Add(file, $"{id}.txt");

      File.WriteAllText(filePath, value);
      Client client = new Client("localhost", "2020");
      client.StoreFile(File.OpenRead(filePath));
    }

  }
}
