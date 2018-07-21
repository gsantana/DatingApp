﻿namespace Microsoft.AspNetCore.Http
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
        }
        public static void AddErrorHeaders(this HttpResponse response)
        {
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

    }
}
