namespace WebApplication1.Controllers
{
  using NewDurnit;
  using System;
  using System.Linq;
  using System.Web;
  using System.Web.Mvc;
  using WebApplication1.Models;

  [RoutePrefix("files")]
  public class FilesController : Controller
  {
    FileDictionary fileDictionary = FileDictionary.Instance;

    // GET /files
    [HttpGet]
    [Route("")]
    public ActionResult GetAll()
    {
      return Json(fileDictionary.GetAllFiles().ToList(), JsonRequestBehavior.AllowGet);
    }

    // GET /files/filename
    [HttpGet]
    [Route("{filename}")]
    public FileResult GetByFilename(string filename)
    {
      var file = fileDictionary.GetFile(filename);
      Client client = new Client("localhost", "2020");
      byte[] array = client.GetFile(file.FileId + ".txt");
      return new FileContentResult(array, file.ContentType);
    }

    // POST /files/filename
    [HttpPost]
    [Route("{filename}")]
    public ActionResult Create(string filename, HttpPostedFileBase upload)
    {
      // http://www.mikesdotnetting.com/article/259/asp-net-mvc-5-with-ef-6-working-with-files
      try
      {
        if (ModelState.IsValid)
        {
          if (upload != null && upload.ContentLength > 0)
          {
            Guid id = Guid.NewGuid();
            var filePath = $@"C:\_\temp\{id}.txt";

            var file = new FileObject
            {
              FileId = id,
              FileName = filename,
              FilePath = filePath,
              ContentType = upload.ContentType
            };
            using (var reader = new System.IO.BinaryReader(upload.InputStream))
            {
              file.Content = reader.ReadBytes(upload.ContentLength);
            }

            fileDictionary.Add(filename, file);
            System.IO.File.WriteAllBytes(filePath, file.Content);
            Client client = new Client("localhost", "2020");
            client.StoreFile(System.IO.File.OpenRead(filePath));
          }
        }
      }
      catch (Exception)
      {
        throw;
      }
      return RedirectToAction("index", "home", null);
    }

  }
}
