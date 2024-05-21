using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace RetroFinder.Models
{
    public class Feature
    {
        public FeatureType Type { get; set; } = new();
        public Location Location { get; set; } = new();

        public int Score;
    }
}
