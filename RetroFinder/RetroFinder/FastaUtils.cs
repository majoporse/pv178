using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using RetroFinder.Output;
using RetroFinder.Models;

namespace RetroFinder
{
    public class FastaUtils
    {
        public string input;
        static bool ValidateFormat(string Line1, string Line2)
        {
            if (Line1.Length < 2)
                return false;
            if (!Line1.StartsWith(">"))
                return false;

            if (Line2.Length == 0)
                return false;
            if (!Regex.IsMatch(Line2, @"[ACTGN]+"))
                return false;

            return true;
        }

        public bool Validate()
        {
            HashSet<string> IDs = new();
            var reader = new StringReader(input);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                string line2 = reader.ReadLine();

                if (!ValidateFormat(line, line2))
                    return false;

                var ID = line.Substring(1);

                if (ID == "" || IDs.Contains(ID))
                    return false;

                IDs.Add(ID);
            }
            return IDs.Count > 0;
        }

        static FastaSequence ParseSequence(TextReader reader)
        {
            string line2, line1;
            line1 = reader.ReadLine();
            line2 = reader.ReadLine();

            if (!ValidateFormat(line1, line2))
                throw new Exception("Invalid format");

            return new FastaSequence(line1.Substring(1), line2);
        }

        public IEnumerable<FastaSequence> Parse()
        {
            List<FastaSequence> sequences = new();
            var reader = new StringReader(input);
            while (reader.Peek() != -1)
            {
                sequences.Add(ParseSequence(reader));
            }
            return sequences;
        }
    }
}
