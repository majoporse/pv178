using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RetroFinder.DTOs;

[XmlType("Location")]
public class LocationDTO
{
    public int Start { get; set; }
    public int End { get; set; }

    public LocationDTO()
    {
    }

    public LocationDTO(Models.Location location)
    {
        Start = location.Start;
        End = location.End;
    }
}
