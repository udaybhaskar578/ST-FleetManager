using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections;

namespace FleetManagementSystem.Utilities
{
    public static class Constants
    {
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";
        public const string Role_Technician = "Technician";

        public static readonly Dictionary<int, string> PassengerCapacity = new Dictionary<int, string>
        {
            { 24, "24 Maximum Capacity" },
            { 36, "36 Maximum Capacity" }
        };


        public static readonly Dictionary<int, string> NumberOfWheels = new Dictionary<int, string>
        {
            { 4, "4 Wheeler" },
            { 6, "6 Wheeler" },
            { 8, "8 Wheeler" },
            { 10, "10 Wheeler" },
            { 12, "12 Wheeler" }
        };
        public static readonly Dictionary<string, string> BusStatus = new Dictionary<string, string>
        {
            { "Scheduled for maintenance", "Scheduled for maintenance" },
            { "Undergoing repairs", "Undergoing repairs" },
            { "Ready for use", "Ready for use" }
        };
        
        public static readonly Dictionary<string, string> RequestStatus = new Dictionary<string, string>
        {
            { "Waiting for Technician", "Waiting for Technician" },
            { "In Progress", "In Progress" },
            { "Complete", "Complete" }
        };

        public const string ResaleViableStatus = "Ready for use";

        public static readonly Dictionary<int, double> BasePriceonCapacity = new Dictionary<int, double>
        {
            { 24, 120000 },
            { 36, 160000 }
        };

        public const double DollarDepreciationByMileage = 0.10;
        public const double DepreciationMileageMilestone = 100000;
        public const double ACAppreciationPercentage = 0.03;
        public const int HistoricYear = 1972;
        public const double HistoricAppreciationPercentage = 0.34;

        public static double? CalculateResaleValue(int Year, int PassengerCapacity,
            double OdomerterReading, bool IsAirConditioned, string CurrentStatus)
        {
            double? resaleValue = null;
            if (BusStatus[CurrentStatus] == BusStatus[ResaleViableStatus])
            {
                var startingSellingPrice = BasePriceonCapacity[PassengerCapacity];
                resaleValue = startingSellingPrice;
                double dollarDepreciation = (OdomerterReading / DepreciationMileageMilestone)
                                            * DollarDepreciationByMileage;
                if (Year <= 1972)
                {
                    resaleValue += startingSellingPrice * HistoricAppreciationPercentage;
                }
                if (IsAirConditioned)
                {
                    resaleValue += startingSellingPrice * ACAppreciationPercentage;
                }
                resaleValue -= dollarDepreciation;

            }

            return resaleValue != null? Math.Round(resaleValue.GetValueOrDefault(),2):resaleValue;

        }

    }
}
