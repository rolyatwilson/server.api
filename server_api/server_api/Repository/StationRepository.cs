﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server_api {
    public class StationsRepository : IDisposable {
        private AirUDBCOE db;

        public StationsRepository() {
            db = new AirUDBCOE();
        }

        public StationsRepository(string connectionString)
        {
            db = new AirUDBCOE(connectionString);
        }

        public bool StationExists(string stationID) {
            if (db.Stations.Find(stationID) == null) {
                return false;
            }
            return true;
        }

        public IEnumerable<Station> StationLocations(decimal latMin, decimal latMax, decimal lngMin, decimal lngMax) {
            IEnumerable<Station> data = from station in db.Stations
                                        where station.Lat >= latMin && station.Lat <= latMax && station.Lng >= lngMin && station.Lng <= lngMax
                                        select station;
            return data;
        }

        public IQueryable GetLatestDataPointsFromStation(string stationID)
        {
            var data = (from points in db.DataPoints
                                          where points.Station.Id == stationID
                                          group points by points.Parameter.Name into paramPoints
                                          select new
                                          {
                                              dataPoints = paramPoints.OrderByDescending(a => a.Time).FirstOrDefault()
                                          });
            return data;
        }

        public IEnumerable<DataPoint> GetDataPointsFromStation(string stationID) {
            IEnumerable<DataPoint> data = from point in db.DataPoints
                                          where point.Station.Id == stationID
                                          select point;
            return data;
        }

        public void Dispose() {
            db.Dispose();
        }
    }
}