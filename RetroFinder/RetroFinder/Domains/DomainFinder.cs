using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder.Domains
{
    public class DomainFinder
    {
        public string input;

        public IEnumerable<FastaSequence> sequences;

        public int Pos = 0;

        int score(char a, char b) => a == b ? 3 : -3;
        const int GAP_PENALTY = 2;

        (int, int, int) SmithWaterman(string original, string domain)
        {
            var matrix = new int[original.Length + 1, domain.Length + 1];

            int maxIndexX = 0;
            int maxIndexY = 0;
            int maxVal = 0;

            //build the matrix and find the max value
            for (int y = 1; y <= original.Length; y++)
            {
                for (int x = 1; x <= domain.Length; x++)
                {
                    var max = new int[]
                        {
                            matrix[y-1, x-1] + score(original[y-1], domain[x-1]),
                            matrix[y-1, x] - GAP_PENALTY,
                            matrix[y, x-1] - GAP_PENALTY,
                            0
                        }.Max();

                    if (max > maxVal)
                    {
                        maxIndexX = x;
                        maxIndexY = y;
                        maxVal = max;
                    }
                    matrix[y, x] = max;
                }
            }

            //backtrace
            int curX = maxIndexX, curY = maxIndexY;
            while (matrix[curY, curX] > 0)
            {
                var opt = new (Func<int, int, int>, (int, int))[]
                    {
                       ((y, x) => matrix[y-1, x-1] + score(original[y - 1], domain[x - 1]), (-1, -1)),
                       ((y, x) => matrix[y, x-1] - GAP_PENALTY, (0 , -1)),
                       ((y, x) => matrix[y-1, x]  - GAP_PENALTY, (-1 , 0))
                    }.MaxBy(tuple => tuple.Item1(curY, curX));

                (curY, curX) = (curY + opt.Item2.Item1, curX + opt.Item2.Item2);
            }
            return (Pos + curY, Pos + maxIndexY, maxVal);
        }


        public IEnumerable<Feature> IdentifyDomains()
        {
            List<Feature> features = new();
            foreach (var seq in sequences)
            {
                var (min, max, maxVal) = SmithWaterman(input, seq.Sequence);
                
                if (max - min < seq.Sequence.Length / 2)
                    continue;

                features.Add(new Feature
                {
                    Type = MyEnumExtensions.FromString(seq.ID),
                    Location = new() { Start = min, End = max },
                    Score = maxVal
                });
            }
            return features;

        }
    }
}
