using RoverConsole.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace RoverConsole.Helpers
{
  public static class NetworkHelper
  {
    #region "PUBLIC METHODS"

    public static HttpWebResponse Post(Uri uri)
    {
      return
        Post(uri, new KeyValuePair<string, string>(), string.Empty, false);
    }

    public static HttpWebResponse Post(Uri uri, KeyValuePair<string, string> header, string data, bool waitResponse)
    {
      try
      {
        var request = WebRequest.Create(uri) as HttpWebRequest;
        request.Method = HttpMethod.Post.ToString();
        request.ContentType = "application/x-www-form-urlencoded";
        request.CookieContainer = new CookieContainer();

        if (!string.IsNullOrWhiteSpace(header.Key))
          request.Headers.Add(header.Key, header.Value);

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        request.ContentLength = bytes != null ? bytes.Length : 0;
        using (Stream requestStream = request.GetRequestStream())
        {
          requestStream.Write(bytes, 0, bytes.Length);
        }

        return
          waitResponse
            ? request.GetResponse() as HttpWebResponse
            : null;
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
