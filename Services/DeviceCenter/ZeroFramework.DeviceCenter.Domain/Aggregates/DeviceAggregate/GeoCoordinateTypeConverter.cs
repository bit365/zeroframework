﻿using System.ComponentModel;
using System.Globalization;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate
{
    internal class GeoCoordinateTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is not null && (value is string @string) && @string is not null)
            {
                return (GeoCoordinate?)@string ?? throw new InvalidCastException();
            }

            return value is not null ? base.ConvertFrom(context, culture, value) : null;
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is GeoCoordinate coordinate)
            {
                return coordinate.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
