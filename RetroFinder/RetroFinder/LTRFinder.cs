using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RetroFinder.Models;

namespace RetroFinder
{
    public class LTRFinder
    {
        public FastaSequence Sequence { get; set; }

        readonly static Regex LTRRegex = new(@"([ACGT]{100,300})([ACGTN]{1000,3500})\1");

        public IEnumerable<Transposon> IdentifyElements() =>
            LTRRegex
            .Matches(Sequence.Sequence)
            .Select(match => new Transposon
            {
                Location = new() 
                { 
                    Start = match.Index, 
                    End = match.Index + match.Length 
                },
                Features = new List<Feature>
                    {
                        new Feature
                        {
                            Type = FeatureType.LTRLeft,
                            Location = new()
                            { 
                                Start = match.Groups[1].Captures[0].Index,
                                End = match.Groups[1].Index + match.Groups[1].Captures[0].Length 
                            }
                        },
                        new Feature
                        {
                            Type = FeatureType.LTRRight,
                            Location = new ()
                            {
                                Start = match.Groups[2].Index + match.Groups[2].Length,
                                End = match.Groups[2].Index + match.Groups[2].Length + match.Groups[1].Length
                            }

                        }
                    }
            });
    }
}
