using System;
using System.Collections.Generic;
using System.Text;

namespace RetroFinder.Models;

public class Graph
{
    bool[,] G;
    public Graph(List<Feature> inputFeatures)
    {
        G = BuildGraph(inputFeatures);
    }
    public void PrintMatrix()
    {
        // Print the matrix G on command line
        Console.Write("    ");
        for (int j = 0; j < G.GetLength(1); j++)
        {
            Console.Write($"{j,2} ");
        }
        Console.WriteLine();

        Console.WriteLine("  /" + new string('-', G.GetLength(1) * 3) + "\\");

        for (int i = 0; i < G.GetLength(0); i++)
        {
            Console.Write($"{i,2} |");
            for (int j = 0; j < G.GetLength(1); j++)
            {
                Console.Write(G[i, j] ? " 1 " : " . ");
            }
            Console.WriteLine("|");
        }

        Console.WriteLine("  \\" + new string('-', G.GetLength(1) * 3) + "/");
    }

    public void PrintSuccessors(List<Feature> features)
    {
        for (int i = 0; i < G.GetLength(0); i++)
        {
            var successors = succs(i);
            Console.Write($"{features[i].Type, 5} {features[i].Score, 5} ({features[i].Location.Start,4}, {features[i].Location.End,4}) Node {i}: ");
            foreach (var succ in successors)
            {
                Console.Write($"{succ} ");
            }
            Console.WriteLine();
        }
    }

    bool isBefore(Feature f1, Feature f2)
    {
        return f1.Location.End < f2.Location.Start;
    }

    FeatureType[] featurePositions =
    [
        FeatureType.GAG,
        FeatureType.PROT,
        FeatureType.INT,
        FeatureType.RT,
        FeatureType.RH
    ];

    bool isCorrectType(Feature from, Feature to)
    {
        return Array.IndexOf(featurePositions, from.Type) <= Array.IndexOf(featurePositions, to.Type);
    }

    void BuildNode(bool[,] Graph, List<Feature> inputFeatures, int from)
    {
        for (int to = 0; to < inputFeatures.Count; to++)
        {
            if (!isBefore(inputFeatures[from], inputFeatures[to]) ||
                !isCorrectType(inputFeatures[from], inputFeatures[to]))
                continue;

            Graph[from, to] = true;
        }
    }

    bool[,] BuildGraph(List<Feature> inputFeatures)
    {
        var Graph = new bool[inputFeatures.Count, inputFeatures.Count];

        for (int from = 0; from < inputFeatures.Count; from++)
        {
            BuildNode(Graph, inputFeatures, from);
        }

        return Graph;
    }


    public List<int> succs(int node)
    {
        List<int> succ = new();
        for (int i = 0; i < G.GetLength(0); i++)
        {
            if (G[node, i])
                succ.Add(i);
        }
        return succ;
    }

    public List<int> preds(int node)
    {
        List<int> pred = new();
        for (int i = 0; i < G.GetLength(0); i++)
        {
            if (G[i, node])
                pred.Add(i);
        }
        return pred;
    }

    public bool this[int from, int to]
    {
        get => G[from, to];
    }

}
