using System.ComponentModel;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate
{
    /// <summary>
    /// Represents a geographical location that is determined by latitude and longitude coordinates
    /// </summary>
    [TypeConverter(typeof(GeoCoordinateTypeConverter))]
    public record GeoCoordinate
    {
        /// <summary>
        /// Gets or sets the latitude of the GeoCoordinate.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Gets or sets the longitude of the GeoCoordinate.
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Constructors
        /// </summary>
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString() => $"{Latitude},{Longitude}";

        /// <summary>
        /// Deconstructing a user-defined type with an extension method
        /// </summary>
        public void Deconstruct(out double latitude, out double longitude)
        {
            latitude = Latitude;
            longitude = Longitude;
        }

        /// <summary>
        /// Implicit conversions
        /// </summary>
        public static implicit operator string(GeoCoordinate geo) => geo.ToString();

        /// <summary>
        /// Explicit conversions
        /// </summary>
        public static explicit operator GeoCoordinate?(string str)
        {
            GeoCoordinate? geoCoordinate = null;

            if (!string.IsNullOrWhiteSpace(str))
            {
                double[] arr = Array.ConvertAll(str.Split(','), i => Convert.ToDouble(i));
                geoCoordinate = new GeoCoordinate(arr.First(), arr.Last());
            }

            return geoCoordinate;
        }
    }
}