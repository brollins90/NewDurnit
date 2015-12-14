using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
  public class FileDictionary
  {
    private static Dictionary<string, string> inside = new Dictionary<string, string>();
    private static FileDictionary _instance = new FileDictionary();
    public static FileDictionary Instance
    {
      get { return _instance; }
    }

    public IEnumerable<string> GetAllFiles()
    {
      return inside.Keys;
    }

    public string GetFilePath(string file)
    {
      return inside[file];
    }

    public void Add(string s1, string s2)
    {
      if (inside.ContainsKey(s1))
      {
        inside[s1] = s2;
      }
      else
      {
        inside.Add(s1, s2);
      }
    }
  }
}