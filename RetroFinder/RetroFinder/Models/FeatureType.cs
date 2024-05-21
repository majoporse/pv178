using System;

namespace RetroFinder.Models
{
    public enum FeatureType
    {
        LTRLeft,
        GAG,
        PROT,
        INT,
        RT,
        RH,
        LTRRight
    }

    public static class MyEnumExtensions
    {
        public static FeatureType FromString(string value)
        {
            foreach (FeatureType type in Enum.GetValues(typeof(FeatureType)))
            {
                if (value.StartsWith(type.ToString()))
                {
                    return type;
                }
            }

            throw new ArgumentException($"Invalid value for FeatureType: {value}");
        }
    }
}
