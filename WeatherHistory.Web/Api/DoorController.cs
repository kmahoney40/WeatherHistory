using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
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

        [Route("")]
        public IHttpActionResult Post([FromBodyAttribute] bool requset)
        {
            logger.Trace("request: " + requset);
            
            //DOOR row;
            using (var context = new db_Entities())
            {
                var row = context.DOOR.ToList().FirstOrDefault();
                row.CMD = true;
                context.SaveChanges();
            }
            return Ok("WOOT");
        }
    }
}
