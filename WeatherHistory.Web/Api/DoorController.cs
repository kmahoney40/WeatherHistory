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
        public DoorController() 
        {
            logger.Trace("request: " + "DoorController()");
        }


        [Route("")]
        public IHttpActionResult Get()
        {
            logger.Trace("request: " + "Get() api/door()");
            string retVal = "{\"door\": \"fasle\"}";
            using (var context = new db_Entities())
            {
                logger.Trace("request: " + "Get() inside context");
                var row = context.door.ToList().FirstOrDefault();
                if (row.cmd == true)
                {
                    row.cmd = false;
                    retVal = "{\"door\": \"true\"}";
                }
                context.SaveChanges();
            }
            var vv = Json(retVal);
            logger.Trace("Get() api/door retVal: " + retVal);
            return Json(retVal);
            //return Ok(retVal);
        }

        [Route("")]
        public IHttpActionResult Post([FromBody] string requset)
        {
            logger.Trace("request: " + requset);
            
            //DOOR row;
            using (var context = new db_Entities())
            {
                var row = context.door.ToList().FirstOrDefault();
                row.cmd = true;
                context.SaveChanges();
            }
            return Ok("WOOT");
        }
    }
}
