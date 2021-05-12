using System;
using CoreLib.CORE.Types;

namespace CoreLib.CORE.Helpers.GeoCoordinateHelpers
{
    public static class GeoCoordinateExtensions
    {
        /// <summary>
        /// Converts angle to radians
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <returns>Angle value in radians</returns>
        private static double ConvertAngleToRadians(double angle)
        {
            return Math.PI * angle / 180;
        }

        /// <summary>
        /// Calculates the distance between two geographical coordinates in meters
        /// </summary>
        /// <param name="fromGeoCoordinate">First geographical coordinate</param>
        /// <param name="toGeoCoordinate">Second geographical coordinate</param>
        /// <returns>Destination in meters between two geographical coordinates</returns>
        public static double DistanceTo(this GeoCoordinate fromGeoCoordinate, GeoCoordinate toGeoCoordinate)
        {
            if (fromGeoCoordinate == null)
            {
                throw new ArgumentNullException(nameof(fromGeoCoordinate));
            }

            if (toGeoCoordinate == null)
            {
                throw new ArgumentNullException(nameof(toGeoCoordinate));
            }

            const double earthRadius = 6371008.8;

            var p1Rad = ConvertAngleToRadians(fromGeoCoordinate.Latitude);
            var p2Rad = ConvertAngleToRadians(toGeoCoordinate.Latitude);

            var deltaRad = ConvertAngleToRadians(fromGeoCoordinate.Longitude - toGeoCoordinate.Longitude);

            var result = earthRadius * Math.Atan(
                Math.Sqrt(Math.Pow(Math.Cos(p2Rad) * Math.Sin(deltaRad), 2) + Math.Pow(
                    Math.Cos(p1Rad) * Math.Sin(p2Rad) - Math.Sin(p1Rad) * Math.Cos(p2Rad) * Math.Cos(deltaRad), 2)) /
                (Math.Sin(p1Rad) * Math.Sin(p2Rad) + Math.Cos(p1Rad) * Math.Cos(p2Rad) * Math.Cos(deltaRad)));

            return result;
        }
    }
}