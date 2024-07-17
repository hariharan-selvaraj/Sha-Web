<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using System.IO;

public class ImageHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string imagePath = context.Request.QueryString["imagePath"];
        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
        {
            context.Response.ContentType = "image/jpeg"; // Adjust the content type based on your image type
            context.Response.TransmitFile(imagePath);
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
