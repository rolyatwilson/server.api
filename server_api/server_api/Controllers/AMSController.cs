﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace server_api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AMSController : ApiController
    {
        /// <summary>
        /// </summary>
        /// <param name="dataSet">AMSDataSet Model</param>
        /// <returns></returns>
        [Route("ams/data")]
        [HttpPost]
        public IHttpActionResult AddAMSDataSet([FromBody]DataPoint[] dataSet)
        {
            var db = new AirUDBCOE();

            db.DataPoints.AddRange(dataSet);

            db.SaveChanges();

            return Ok(dataSet);
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">*xml comment*</param>
        /// <returns></returns>
        [Route("ams/state")]
        [HttpPost]
        public IHttpActionResult UpdateAMSDeviceState([FromBody]StationState[] states)
        {
            var db = new AirUDBCOE();
            Station station = states[0].Station;

            if (station == null)
            {
                // Failed to add StationState.
                return Ok("Failed to add station state with Station with ID = " + states[0].Station + " not found.");                
            }

            db.DeviceStates.AddRange(states);

            db.SaveChanges();

            // Success.
            return Ok(states);
        }
        */

        /// <summary>
        /// *xml comment*
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("ams")]
        [HttpPut]
        public void PutAMSData(int id, [FromBody]string value)
        {

        }

        /// <summary>
        /// *xml comment*
        /// </summary>
        /// <param name="id"></param>
        [Route("ams")]
        [HttpDelete]
        public void Delete(int id)
        {

        }
    }
}
