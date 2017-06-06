using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WeatherHistory.Web.Api
{
    [RoutePrefix("door")]
    public class DoorController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public DoorController() { }

        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok("{\"door\": \"door\"}");
        }
    }
}
