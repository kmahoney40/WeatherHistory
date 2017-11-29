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
using System.Globalization;
using System.Web.Script.Serialization;

namespace WeatherHistory.Web.Api
{
    [Table("temp_fan", Schema="public")]
    public class temp_fan
    {
        public temp_fan() 
        {
            //GMT = DateTime.UtcNow;
        }
        public temp_fan(string s)
        {
            dt_created = DateTime.UtcNow;
        }
        public temp_fan(JArray update)
        {
            dt_created = DateTime.UtcNow;
        }
        public temp_fan(int id, double t1, double t2, double t3, bool fo, bool co, double voltage, string gmt)
        {
            this.id = id;
            temp_1 = t1;
            temp_2 = t2;
            temp_3 = t1;
            fan_on = fo;
            charger_on = co;
            this.voltage = voltage;
            dt_created = Convert.ToDateTime(gmt);
            //dt_created = gmt;
        }
        public temp_fan(temp_fan tempFan)
        {
            id = tempFan.id;
            temp_1 = tempFan.temp_1;
            temp_2 = tempFan.temp_2;
            temp_3 = tempFan.temp_3;
            fan_on = tempFan.fan_on;
            charger_on = tempFan.charger_on;
            voltage = tempFan.voltage;
            dt_created = tempFan.dt_created;
        }

        [Key]
        [Column("id")]
        public int id { get; set; }
        public double temp_1 { get; set; }
        public double temp_2 { get; set; }
        public double temp_3 { get; set; }
        public bool fan_on { get; set; }
        public bool charger_on { get; set; }
        public double voltage { get; set; }
        public DateTime dt_created { get; set; }
    }

    [Table("door", Schema = "public")]
    public class door
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        public bool cmd { get; set; }
    }

    public partial class db_Entities : DbContext
    {
        public db_Entities() : base("awsautomation") { }
        //public db_Entities() : base("postgres") { }
        public DbSet<temp_fan> temp_fan { get; set; }
        public DbSet<door> door { get; set; }
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

            temp_fan row;
            string jRow = string.Empty;
            using (var context = new db_Entities())
            {
                row = context.temp_fan.ToList().OrderByDescending(r => r.id).FirstOrDefault();
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
                var row = context.temp_fan.ToList().OrderByDescending(r => r.id).Take(rows);
                retRows = JsonConvert.SerializeObject(row);
            }
            logger.Info("Log Get");
            return Ok(retRows);
        }

        [Route("")]
        public IHttpActionResult Post([FromBody] string request)
        {
            try
            {
                logger.Info("request: " + request);

                var jss = new JavaScriptSerializer();
                dynamic data = jss.Deserialize<dynamic>(request);

                temp_fan temp = JsonConvert.DeserializeObject<temp_fan>(request);
                // data["GMT"] = "yyyy-MM-dd hh:mm:ss.ffffff offset"
                var dtCreated = (string)data["GMT"];
                logger.Debug("PiPlateController.Post dtCreated = " + dtCreated);
                int idx = dtCreated.IndexOf('.');
                dtCreated = dtCreated.Substring(0, idx);
                logger.Debug("PiPlateController.Post dtCreated SHORT = " + dtCreated);
                //temp.dt_created = DateTime.ParseExact(dtCreated, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                temp.dt_created = DateTime.Parse(dtCreated);
                using (var context = new db_Entities())
                {
                    context.Entry(temp).State = EntityState.Added;
                    context.SaveChanges();
                }

                logger.Info(request);
            }
            catch (Exception ex)
            {
                logger.Info("exception: " + ex.ToString());
            }

            return Ok(request);
        }
    }
}
    