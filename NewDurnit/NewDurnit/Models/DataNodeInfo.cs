using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewDurnit
{
    public class DataNodeInfo
    {
        public string URIAddress { get; set; }
        public int HowManyFriends { get; set; }
        public List<string> Files { get; set; }
        public List<string> connections { get; set; }

        public DataNodeInfo()
        {
            Files = new List<string>();
            connections = new List<string>();
            URIAddress = "";
        }
    }
}
