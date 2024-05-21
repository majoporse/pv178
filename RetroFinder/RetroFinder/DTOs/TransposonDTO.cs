using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RetroFinder.DTOs;

[XmlType("Transposon")]
public class TransposonDTO
{
    [JsonPropertyName("Location")]
    public LocationDTO Location { get; set; }

    [XmlArray("Features")]
    [JsonPropertyName("Features")]
    public List<FeatureDTO> Features { get; set; }

    public TransposonDTO()
    {
    }

    public TransposonDTO(Models.Transposon transposon)
    {
        Location = new LocationDTO(transposon.Location);
        Features = transposon.Features.Select(f => new FeatureDTO(f)).ToList();
    }
}
