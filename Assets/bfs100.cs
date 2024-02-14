using System.Collections.Generic;
public class BFS100 {
    public static int CountPaths<NodeType>(
            IGraph<NodeType> graph, 
            NodeType startNode, NodeType endNode, 
            int maxIterations=1000)
    {
        Queue<NodeType> openQueue = new Queue<NodeType>();
        HashSet<NodeType> openSet = new HashSet<NodeType>();
        Dictionary<NodeType, NodeType> previous = new Dictionary<NodeType, NodeType>();
        openQueue.Enqueue(startNode);
        openSet.Add(startNode);
        int pathCount = 0;
        int i;
        for (i = 0; i < maxIterations; ++i) {
            if (openQueue.Count == 0) {
                break;
            } else {
                NodeType searchFocus = openQueue.Dequeue();

                if (searchFocus.Equals(endNode)) {
                    pathCount++; // Increment path count when target is found
                    while (previous.ContainsKey(searchFocus)) {
                        searchFocus = previous[searchFocus];
                    }
                    break;
                } else {
                    foreach (var neighbor in graph.Neighbors(searchFocus)) {
                        if (openSet.Contains(neighbor)) {
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
