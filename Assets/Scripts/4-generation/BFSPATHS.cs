using System.Collections.Generic;

/**
 * A generic implementation of the BFS algorithm.
 * @author Erel Segal-Halevi
 * @since 2020-02
 */
public class BFSPATHS
{
    public static int CountPaths<NodeType>(
            IGraph<NodeType> graph,
            NodeType startNode, NodeType endNode,
            int maxiterations = 1000)
    {
        Queue<NodeType> openQueue = new Queue<NodeType>();
        HashSet<NodeType> openSet = new HashSet<NodeType>();
        Dictionary<NodeType, NodeType> previous = new Dictionary<NodeType, NodeType>();
        openQueue.Enqueue(startNode);
        openSet.Add(startNode);
        int pathCount = 0;

        for (int i = 0; i < maxiterations; ++i)
        { // After maxiterations, stop and return an empty path
            if (openQueue.Count == 0)
            {
                break;
            }
            else
            {
                NodeType searchFocus = openQueue.Dequeue();

                if (searchFocus.Equals(endNode))
                {
                    // We found the target -- increment path count
                    pathCount++;
                }
                else
                {
                    // We did not found the target yet -- develop new nodes.
                    foreach (var neighbor in graph.Neighbors(searchFocus))
                    {
                        if (openSet.Contains(neighbor))
                        {
                            continue;
                        }
                        openQueue.Enqueue(neighbor);
                        openSet.Add(neighbor);
                        previous[neighbor] = searchFocus;
                    }
                }
            }
        }

        return pathCount;
    }
}
