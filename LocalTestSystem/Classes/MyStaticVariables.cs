using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalTestSystem.Classes
{
    public class MyStaticVariables
    {
        // static members can be accessed without instantiating the class and are always available.
        //Static members are shared (via the class name) between all objects derived from this class.
        public static bool RequireAckAfterEStopReleased = false;
        public static bool FlashBlueYellowLight;
        public static bool AnalogInitialized = false;
        public static bool Acknowledge { get; internal set; }

        public static double FahrenheitToCelsius(double Fahrenheit)
        {
            return Math.Round((Fahrenheit - 32) * 5 / 9, 2);
        }
        public static double CelsiusToFahrenheit(double Celcius)
        {
            return Math.Round((Celcius * 9 / 5 + 32));
        }
        public static double MilesToKilo(double miles)
        {
            // This method can be accessed through the class name MyStaticVariables not through an instance of MyStaticVariables and is always available
            // No instantiation is needed
            return miles * 1.609;
        }

        public static uint FlowCounter = 0;

     }

    public static class DateTimeExtensions
    {
        public static bool IsValidTimeFormat(this string input)
        {
            return TimeSpan.TryParse(input, out var dummyOutput);
        }
    }
}
