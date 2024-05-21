using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using RetroFinder.Models;
using RetroFinder.Output;
using RetroFinder.Output.Interfaces;
using Spectre.Console;

namespace RetroFinder
{
    public class RetroFinder
    {
        public List<ISerializer> Printers;

        public FastaSequence sequence;
        public IEnumerable<FastaSequence> Domains;

        public SequenceAnalysis Result;

        public int numThreads = 1;

        static IEnumerable<FastaSequence> tryparse(string Path)
        {
            try
            {
                var Content = File.ReadAllText(Path);

                FastaUtils fastaUtils = new() { input = Content };
                if (!fastaUtils.Validate())
                    throw new Exception("Invalid format");

                return fastaUtils.Parse();
            } catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
                return new List<FastaSequence>();
            }
        }

        public void addDomains(string path)
        {
            Domains = tryparse(path);
        }

        public void Analyze(string path)
        {
            var inputs = tryparse(path);

            if (Domains == null)
            {
                AnsiConsole.MarkupLine("[red]Add domains first![/]");
                return;
            }
            AnsiConsole.MarkupLine("[green]Analyzing...[/]");
            ThreadPool.SetMaxThreads(numThreads, numThreads);
            var count = new CountdownEvent(inputs.Count());
            foreach (var sequence in inputs)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    SequenceAnalysis sequenceAnalysis = new() { Sequence = sequence, Domains = Domains };
                    sequenceAnalysis.Analyze();
                    Printers.ForEach(printer => printer.SerializeAnalysisResult(sequenceAnalysis));
                    count.Signal();
                });
            }
            count.Wait();
        }

    }
}
