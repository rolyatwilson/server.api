﻿using System;
namespace server_api.Utilities
{
    public class No2Aqi
    {
        public static Tuple<int, int> CalculateAQIAndCategory(double value)
        {
            var breakPoints = BreakPoints(value);

            var aqi = AQIFormula.CalculateAqi(breakPoints, value);

            var category = AQIFormula.CalculateCategory(aqi);

            return new Tuple<int, int>(aqi, category);
        }

        public static Tuple<Tuple<double, double>, Tuple<int, int>> BreakPoints(double value)
        {
            // AQI values are only generated for values above 200.
            // "NO2 has no short-term NAAQS and can generate an AQI only above a value of 200. "
            // Pg. 14 - https://www3.epa.gov/ttn/caaa/t1/memoranda/rg701.pdf

            if (value <= 1.24)
            {
                Tuple<double, double> BP = new Tuple<double, double>(0.65, 1.24);
                Tuple<int, int> I = new Tuple<int, int>(201, 300);
                return new Tuple<Tuple<double, double>, Tuple<int, int>>(BP, I);
            }

            Tuple<double, double> bp = new Tuple<double, double>(1.24, 2.04);
            Tuple<int, int> i = new Tuple<int, int>(301, 400);
            return new Tuple<Tuple<double, double>, Tuple<int, int>>(bp, i);
        }
    }
}