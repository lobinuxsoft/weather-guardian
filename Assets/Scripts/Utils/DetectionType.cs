using System;

namespace WeatherGuardian.Utils
{
    [Flags]
    public enum DetectionType
    {
        ENTER = 1 << 0,
        STAY = 1 << 1,
        EXIT = 1 << 2
    }
}