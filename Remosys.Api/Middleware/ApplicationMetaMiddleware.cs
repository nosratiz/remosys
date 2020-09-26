using System;
using System.Linq;
using System.Threading.Tasks;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Http;
using Remosys.Common.Helper;

namespace Remosys.Api.Middleware
{
     public class ApplicationMetaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRequestMeta _requestMeta;

        public ApplicationMetaMiddleware(RequestDelegate next, IRequestMeta requestMeta)
        {
            _next = next;
            _requestMeta = requestMeta;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var userAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault();
            try
            {
                var userAgentInfo = DeviceDetector.GetInfoFromUserAgent(userAgent);

                _requestMeta.Ip = httpContext.Connection.RemoteIpAddress.ToString();

                if (userAgentInfo.Match.BrowserFamily != "Unknown")
                {
                    _requestMeta.Browser = userAgentInfo.Match.Client.Name;
                    _requestMeta.Os = userAgentInfo.Match.Os.Name;
                    _requestMeta.Device = userAgentInfo.Match.DeviceType;
                    _requestMeta.UserAgent = userAgent;
                }
                else
                {
                    _requestMeta.Browser = "";
                    _requestMeta.Os = "";
                    _requestMeta.Device = "";
                    _requestMeta.UserAgent = userAgent;
                }
            }
            catch (Exception)
            {
                _requestMeta.Browser = "";
                _requestMeta.Os = "";
                _requestMeta.Device = "";
                _requestMeta.UserAgent = userAgent;
            }

            return _next(httpContext);
        }
    }

}