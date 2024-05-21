using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;

namespace RetroFinder.Domains;
public class DomainPicker
{
    public List<Feature> inputFeatures;
    List<int> FindPrevMax(Graph Graph, int[] values, int pos)
    {
        List<int> prevPositions = [];
        foreach (var from in Graph.preds(pos))
        {
            if (values[from] == values[pos] - 1)
            {
                prevPositions.Add(from);
            }
        }
        return prevPositions;
    }

    int[] CountValues(Graph Graph, List<Feature> inputFeatures)
    {
        var values = new int[inputFeatures.Count];

        for (int pos = 0; pos < inputFeatures.Count; pos++)
        {
            foreach (var backwardsNode in Graph.preds(pos))
            {
                if (values[backwardsNode] + 1 > values[pos])
                {
                    values[pos] = values[backwardsNode] + 1;
                }
            }
        }
        return values;
    }

    List<List<int>> FindPossiblePaths(Graph Graph, int[] values, IEnumerable<int> pos)
    {
        List<List<int>> Paths = new();
        List<List<int>> BuildingPaths = new();
        BuildingPaths.AddRange(pos.Select(e => new List<int>(){e} ));

        while (BuildingPaths.Count != 0)
        {
            //pop the last path
            var path = BuildingPaths.Last();
            BuildingPaths.RemoveAt(BuildingPaths.Count - 1);

            //check if the path is complete
            if (values[path.Last()] == 0)
            {
                Paths.Add(path);
                continue;
            }

            //find the prev nodes
            var prevs = FindPrevMax(Graph, values, path.Last());
            foreach (var prev in prevs)
            {
                List<int> newPath = new(path);
                newPath.Add(prev);
                BuildingPaths.Add(newPath);
            }
        }
        return Paths;
    }

    void printPath(List<int> path)
    {
        foreach (var node in path)
        {
            Console.Write(node);
            Console.Write(" ");
        }
        Console.WriteLine();
    }

    public IEnumerable<Feature> PickDomains()
    {
        //sort the features by start location
        inputFeatures.Sort((f1, f2) => f1.Location.Start - f2.Location.Start);

        Graph Graph = new(inputFeatures);
        //Graph.PrintMatrix();
        //Graph.PrintSuccessors(inputFeatures);

        //find the longest path in the graph
        var values = CountValues(Graph, inputFeatures);
        var maxVal = values.Max();

        var max_positions = Enumerable.Range(0, values.Length).Where(Index => values[Index] == maxVal);

        //backtrace the longest path
        var Paths = FindPossiblePaths(Graph, values, max_positions);

        var bestPaths = Paths.OrderBy(path => path.Sum(node => inputFeatures[node].Score)).ToList();
        var bestPath = bestPaths.Last();
        //printPath(bestPath);
        return bestPath.Select(index => inputFeatures[index]);
    }
}

