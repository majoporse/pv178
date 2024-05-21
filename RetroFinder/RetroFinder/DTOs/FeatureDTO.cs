using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RetroFinder.DTOs;

[XmlType("Feature")]
public class FeatureDTO
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FeatureType Type { get; set; }

    public LocationDTO Location { get; set; }

    public FeatureDTO()
    {
    }

    public FeatureDTO(Feature feature)
    {
        Type = feature.Type;
        Location = new LocationDTO(feature.Location);
    }
}
