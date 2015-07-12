using RoverConsole.Loggers;
using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace RoverConsoleClient.Helpers
{
  public static class NetworkHelper
  {
    #region "PUBLIC METHODS"

    public static HttpWebResponse Post(Uri uri, string data)
    {
      try
      {
        var request = WebRequest.Create(uri) as HttpWebRequest;
        request.Method = HttpMethod.Post.ToString();
        request.ContentType = "application/x-www-form-urlencoded";
        request.CookieContainer = new CookieContainer();

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        request.ContentLength = bytes.Length;
        using (Stream requestStream = request.GetRequestStream())
        {
          requestStream.Write(bytes, 0, bytes.Length);
        }

        return
          string.IsNullOrWhiteSpace(data)
            ? null
            : request.GetResponse() as HttpWebResponse;
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
      }
      return null;
    }

    #endregion "PUBLIC METHODS"
  }
}
