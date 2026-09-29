using System;

namespace Meanders.Tools.Core
{
    public static class ME_UnitConverter
    {
        public enum LengthUnit
        {
            Millimeter,
            Centimeter,
            Meter,
            Inch,
            Foot
        }


        public static double ConvertLength(
            double value,
            LengthUnit from,
            LengthUnit to)
        {
            if (from == to)
                return value;


            // Convert source to millimeter
            double millimeters = value * ToMillimeterFactor(from);


            // Convert millimeter to target
            return millimeters / ToMillimeterFactor(to);
        }


        private static double ToMillimeterFactor(
            LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.Millimeter:
                    return 1.0;

                case LengthUnit.Centimeter:
                    return 10.0;

                case LengthUnit.Meter:
                    return 1000.0;

                case LengthUnit.Inch:
                    return 25.4;

                case LengthUnit.Foot:
                    return 304.8;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}