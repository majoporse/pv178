using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetroFinder.Output.Interfaces;
using RetroFinder.Models;
using System.Text.Json;
using System.Collections;
using RetroFinder.DTOs;

namespace RetroFinder.Output;

public class JsonPrinter : ISerializer
{
    public void SerializeAnalysisResult(SequenceAnalysis analysis)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        var wrapper = new { Transposons = analysis.Transposons.Select(e => new TransposonDTO(e)).ToList() };

        var json = JsonSerializer.Serialize(wrapper, options);
        System.IO.File.WriteAllText($"{analysis.Sequence.ID}.json", json);
    }
}
