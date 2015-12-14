using Microsoft.Owin;
using NewDurnit;
using Owin;
using System.Threading;

[assembly: OwinStartup(typeof(WebApplication1.Startup))]

namespace WebApplication1
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      Initialization r = new Initialization("127.0.0.1", "1010");
      new Thread(() => r.Start(@"C:\_\temp\dusmmy.xml")).Start();

      Initialization b = new Initialization("127.0.0.1", "3030");
      new Thread(() => b.Start(@"C:\_\temp\dusmmy.xml")).Start();

      Initialization a = new Initialization("127.0.0.1", "4040");
      new Thread(() => a.Start(@"C:\_\temp\dusmmy.xml")).Start();

      Initialization t = new Initialization("127.0.0.1", "2020");
      new Thread(() => t.Start(@"C:\_\temp\dummy.xml")).Start();

      Thread.Sleep(2000);
    }
  }
}