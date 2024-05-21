using RetroFinder.Models;
using RetroFinder.Output;
using System;
using System.Collections.Generic;
using System.IO;
using Spectre.Console;
using RetroFinder.Output.Interfaces;

namespace RetroFinder
{
    class Program
    {
        static RetroFinder finder = new();
        static Operation promptNext()
        {
            AnsiConsole.MarkupLine("Please enter the next operation!");
            var prompt = new SelectionPrompt<string>()
                .Title("Please choose an output type:")
                .PageSize(10)
                .AddChoices(new[] { "INPUT NEW FILE & ANALYZE", "INPUT DOMAINS", "EXIT" })
                .HighlightStyle(new Style(foreground: Color.Orange1));

            var result = AnsiConsole.Prompt(prompt);
            switch (result)
            {
                case "INPUT NEW FILE & ANALYZE":
                    AnsiConsole.MarkupLine("Please enter the path to the file:");
                    finder.Analyze(Console.ReadLine());
                    return Operation.NEW_FILE;

                case "INPUT DOMAINS":
                    AnsiConsole.MarkupLine("Please enter the path to the Domains:");
                    finder.addDomains(Console.ReadLine());
                    return Operation.NEW_DOMAIN;

                case "EXIT":
                    AnsiConsole.MarkupLine("Exiting...");
                    return Operation.EXIT;
            }
            return Operation.NO_OPERATION;
        }

        static bool setThreads()
        {
            AnsiConsole.MarkupLine("Please enter the number of threads to use:");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                finder.numThreads = result;
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            while (!setThreads()) { }

            var prompt = new SelectionPrompt<string>()
                .Title("Please choose an output type:")
                .PageSize(10)
                .AddChoices(new[] { "XML", "JSON", "BOTH" })
                .HighlightStyle(new Style(foreground: Color.Orange1));

            var result = AnsiConsole.Prompt(prompt);
            List<ISerializer> printers = new();
            switch (result)
            {
                case "XML":
                    printers.Add(new XMLPrinter());
                    break;
                case "JSON":
                    printers.Add(new JsonPrinter());
                    break;
                case "BOTH":
                    printers.Add(new XMLPrinter());
                    printers.Add(new JsonPrinter());
                    break;
            }
            
            finder.Printers = printers;
            while (promptNext() != Operation.EXIT) { }
        }


        void test()
        {
            SequenceAnalysis analysis = new SequenceAnalysis()
            {
                Sequence = new FastaSequence
        (
            "GAG 1",
            "ATCG"
        ),
                Transposons = new List<Transposon>()
                {
                    new Transposon()
                    {
                        Location = new() {Start = 1, End = 2},
                        Features =  new List<Feature>()
                        {
                            new Feature()
                            {
                                Type =  FeatureType.GAG,
                                Location = new() { Start = 1, End = 2 },
                                Score = 456
                            }
                        }
                    }
                }
            };

            JsonPrinter printer = new JsonPrinter();
            printer.SerializeAnalysisResult(analysis);

            XMLPrinter xmlPrinter = new XMLPrinter();
            xmlPrinter.SerializeAnalysisResult(analysis);
        }
    }
}
