using NewDurnit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyStartupApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(args[0]);
            //Console.WriteLine(args[1]);
            Initialization r = new Initialization(args[0], "1010");
            new Thread(() => r.Start("dusmmy.xml")).Start();
            
            Initialization b = new Initialization(args[0], "3030");
            new Thread(() => b.Start("dusmmy.xml")).Start();
            
            Initialization a = new Initialization(args[0], "4040");
            new Thread(() => a.Start("dusmmy.xml")).Start();
            
            Initialization t = new Initialization(args[0], "2020");
            new Thread(() => t.Start("dummy.xml")).Start();


            Thread.Sleep(5000);
            Client client = new Client(args[0], "2020");
            client.StoreFile(File.OpenRead(@"C:\Users\Elijah Segura\Desktop\testingText.txt"));
            byte[] array = client.GetFile(@"testingText.txt");
            File.WriteAllBytes(@"C:\Users\Elijah Segura\Desktop\RECEIVED.txt", array);
            //Console.WriteLine("WOAH!");
        }
    }
}
