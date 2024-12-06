using System;
using System.Text.Json.Serialization;

namespace OpenMeteo
{
    public class CurrentUnits2
    {
        public string Time { get; set; }
        public string Interval { get; set; }
        public string OceanCurrentVelocity { get; set; }
        public string OceanCurrentDirection { get; set; }
    }

    public class Current2
    {
        public string Time { get; set; }
        public int Interval { get; set; }
        public float OceanCurrentVelocity { get; set; }
        public float OceanCurrentDirection { get; set; }
    }
    public class OceanData
    {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double GenerationtimeMs { get; set; }
            public int UtcOffsetSeconds { get; set; }
            public string Timezone { get; set; }
            public string TimezoneAbbreviation { get; set; }
            public double Elevation { get; set; }
            public int? LocationId { get; set; } // Nullable, car non présent dans certains objets
            public CurrentUnits2 CurrentUnits { get; set; }
            public Current2 Current { get; set; }
        
    }
}
