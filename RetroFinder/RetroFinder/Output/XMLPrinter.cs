using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroFinder.Output.Interfaces;
using RetroFinder.Models;
using RetroFinder.DTOs;
using System.Xml.Serialization;

namespace RetroFinder.Output;

public class XMLPrinter: ISerializer
{
    public void SerializeAnalysisResult(SequenceAnalysis analysis)
    {
        var xml = new System.Xml.Serialization.XmlSerializer(typeof(TransposonDTO[]));
        using (var writer = new System.IO.StreamWriter($"{analysis.Sequence.ID}.xml"))
        {
            xml.Serialize(writer, analysis.Transposons.Select(e => new TransposonDTO(e)).ToArray());
        }
    }
}
