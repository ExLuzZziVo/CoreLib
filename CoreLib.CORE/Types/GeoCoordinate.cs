using System;

namespace CoreLib.CORE.Types
{
    /// <summary>
    /// Geographical coordinates
    /// </summary>
    public class GeoCoordinate
    {
        /// <summary>
        /// Geographical coordinates
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        public GeoCoordinate(double latitude, double longitude)
        {
            if (latitude > 90 || latitude < -90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude));
            }

            if (longitude > 180 || longitude < -180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude));
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; }
    }
}