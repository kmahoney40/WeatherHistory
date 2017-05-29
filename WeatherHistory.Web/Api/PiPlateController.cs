using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherHistory.Web.Api
{
    [Table("TEMP_FAN", Schema="public")]
    public class TEMP_FAN
    {
        public TEMP_FAN() 
        {
            //GMT = DateTime.UtcNow;
        }
        public TEMP_FAN(string s)
        {
            GMT = DateTime.UtcNow;
        }
        public TEMP_FAN(JArray update)
        {
            GMT = DateTime.UtcNow;
        }
        public TEMP_FAN(int id, double t1, double t2, double t3, bool fo, double voltage)
        {
            TEMP_FAN_ID = id;
            TEMP_1 = t1;
            TEMP_2 = t2;
            TEMP_3 = t1;
            FAN_ON = fo;
            VOLTAGE = voltage;
        }
        public TEMP_FAN(TEMP_FAN tempFan)
        {
            TEMP_FAN_ID = tempFan.TEMP_FAN_ID;
            TEMP_1 = tempFan.TEMP_1;
            TEMP_2 = tempFan.TEMP_2;
            TEMP_3 = tempFan.TEMP_3;
            FAN_ON = tempFan.FAN_ON;
            VOLTAGE = tempFan.VOLTAGE;
            GMT = tempFan.GMT;
        }

        [Key]
        [Column("TEMP_FAN_ID")]
        public int TEMP_FAN_ID { get; set; }
        public double TEMP_1 { get; set; }
        public double TEMP_2 { get; set; }
        public double TEMP_3 { get; set; }
        public bool FAN_ON { get; set; }
        public double VOLTAGE { get; set; }
        public DateTime GMT { get; set; }
    }

    public partial class db_Entities : DbContext
    {
        public db_Entities() : base("postgres") { }
        public DbSet<TEMP_FAN> TEMP_FAN { get; set; }
    }

    [RoutePrefix("piplates")]
    public class PiPlateController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public PiPlateController() { }
            
        [Route("")]
        public IHttpActionResult Get()
        {
            logger.Trace("PiPlates.Get");

            TEMP_FAN row;
            string jRow = string.Empty;
            using (var context = new db_Entities())
            {
                row = context.TEMP_FAN.ToList().OrderByDescending(r => r.TEMP_FAN_ID).FirstOrDefault();
                jRow = JsonConvert.SerializeObject(row);
            }
            logger.Info("Log Get");
            if (jRow != string.Empty) { return Ok(jRow); }
            else { return Ok("No Row Found"); }
        }

        [Route("toprows")]
        public IHttpActionResult Get(int rows = 1)
        {
            if (rows < 1) { rows = 1; }

            string retRows;
            using (var context = new db_Entities())
            {
                var row = context.TEMP_FAN.ToList().OrderByDescending(r => r.TEMP_FAN_ID).Take(rows);
                retRows = JsonConvert.SerializeObject(row);
            }
            logger.Info("Log Get");
            return Ok(retRows);
        }

        [Route("")]
        public IHttpActionResult Post([FromBody] string request)
        {
            logger.Trace("request: " + request);

            TEMP_FAN temp = JsonConvert.DeserializeObject<TEMP_FAN>(request);
            using (var context = new db_Entities())
            {
                context.Entry(temp).State = EntityState.Added;
                context.SaveChanges();
            }
            
            logger.Info(request);

            return Ok(request);
        }
    }
}
    