//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Web.Http;
//using Microsoft.Owin.Security.OAuth;
//using Newtonsoft.Json.Serialization;

//namespace WebApplication1
//{
//  public static class WebApiConfig
//  {
//    public static void Register(HttpConfiguration config)
//    {
//      config.MapHttpAttributeRoutes();

//      config.Routes.MapHttpRoute(
//          name: "Files",
//          routeTemplate: "{controller}/{filename}",
//          defaults: new { filename = RouteParameter.Optional }
//      );
//    }
//  }
//}
