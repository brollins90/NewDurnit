namespace WebApplication1.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;
  using WebApplication1.Models;

  public class FileDictionary
  {
    static Dictionary<string, FileObject> _inside = new Dictionary<string, FileObject>();
    static FileDictionary _instance = new FileDictionary();

    public static FileDictionary Instance
    {
      get { return _instance; }
    }

    public IEnumerable<string> GetAllFiles()
    {
      return _inside.Keys;
    }

    public string GetFilePath(string filename)
    {
      return _inside[filename].FilePath;
    }

    public FileObject GetFile(string filename)
    {
      return _inside[filename];
    }

    public void Add(string s1, FileObject s2)
    {
      if (_inside.ContainsKey(s1))
      {
        _inside[s1] = s2;
      }
      else
      {
        _inside.Add(s1, s2);
      }
    }
  }
}