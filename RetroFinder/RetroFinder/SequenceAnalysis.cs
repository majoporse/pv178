using RetroFinder.Domains;
using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder;

public class SequenceAnalysis
{
    public FastaSequence Sequence { get; set; }

    public IEnumerable<FastaSequence> Domains { get; set; }

    public List<Transposon> Transposons { get; set; }

    public void Analyze()
    {
        LTRFinder ltrFinder = new() { Sequence = Sequence };
        Transposons = ltrFinder.IdentifyElements().ToList();

        foreach(var transposon in Transposons)
        {
            DomainFinder domainFinder = new() 
            {
                Pos = transposon.Location.Start,
                input = Sequence.Sequence.Substring(transposon.Location.Start, transposon.Location.End - transposon.Location.Start),
                sequences = Domains
            };
            var features = domainFinder.IdentifyDomains();
            DomainPicker domainPicker = new()
            {
                inputFeatures = features.ToList(),
            };

            transposon.Features.AddRange(domainPicker.PickDomains());
            transposon.Features.Sort((a, b) => a.Location.Start.CompareTo(b.Location.Start));
        }
    }
}
