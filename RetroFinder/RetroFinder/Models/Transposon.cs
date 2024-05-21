using System.Collections.Generic;

namespace RetroFinder.Models;

public class Transposon
{
    public Location Location { get; set; } = new();

    public List<Feature> Features { get; set; } = new List<Feature>();

}
