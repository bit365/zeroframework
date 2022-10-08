﻿using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Measurements
{
    public class DevicePropertyReport
    {
        [AllowNull]
        public string Date { get; set; }

        public double? MinValue { get; set; }

        public double? AverageValue { get; set; }

        public double? MaxValue { get; set; }
    }
}
