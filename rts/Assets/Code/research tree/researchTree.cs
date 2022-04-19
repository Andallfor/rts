using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public static class researchTree {
    private static Dictionary<int, List<researchNode>> processedNodes = new Dictionary<int, List<researchNode>>();
    private static List<(string, List<string>)> incomplete = new List<(string, List<string>)>();
    private static List<researchNode> nodes = new List<researchNode>();

    public static void addNode(string name, List<string> dependencies) {
        incomplete.Add((name, dependencies));
    }

    public static void addNode(researchNode rn) {
        nodes.Add(rn);
    }

    public static void processNodes() {
        // format incomplete into proper research nodes
        foreach ((string name, List<string> dependencies) in incomplete) {
            nodes.Add(new researchNode(name, dependencies, incomplete
                .Where(x => x.Item2.Contains(name))
                .Select(x => x.Item1)
                .ToList()));
        }


        List<researchNode> toCheck = new List<researchNode>() {nodes.First(x => x.dependencies.Count == 0)};
        int index = 0;
        while (true) {
            List<researchNode> nextFrontier = new List<researchNode>();
            foreach (researchNode rn in toCheck) {
                addNodeToDict(index, rn);

                foreach (string s in rn.dependents) {
                    nextFrontier.Add(nodes.First(x => x.name == s));
                }
            }

            if (nextFrontier.Count == 0) break;
            toCheck = nextFrontier;

            index++;
        }

        foreach (KeyValuePair<int, List<researchNode>> kvp in processedNodes) {
            StringBuilder sb = new StringBuilder();

            foreach (researchNode rn in kvp.Value) {
                sb.Append(rn.name + ' ');
            }

            Debug.Log($"{kvp.Key}: " + sb.ToString());
        }
    }

    private static void addNodeToDict(int level, researchNode rn) {
        if (!processedNodes.ContainsKey(level) || processedNodes[level] is null) {
            processedNodes[level] = new List<researchNode>() {rn};
        } else processedNodes[level].Add(rn);
    }
}



public readonly struct researchNode {
    public readonly string name;
    public readonly List<string> dependents, dependencies;

    public researchNode(string name, List<string> dependencies, List<string> dependents) {
        this.name = name;
        this.dependents = dependents;
        this.dependencies = dependencies;
    }

    public static bool operator==(researchNode n1, researchNode n2) => n1.name == n2.name;
    public static bool operator!=(researchNode n1, researchNode n2) => n1.name != n2.name;

    public override bool Equals(object obj)
    {
        if (obj is researchNode) {
            return (researchNode) obj == this;
        }
        return false;
    }

    public override int GetHashCode() => this.name.GetHashCode();
}