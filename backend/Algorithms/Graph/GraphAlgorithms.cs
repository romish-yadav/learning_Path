namespace LearnPath.API.Algorithms.Graph;

public static class GraphAlgorithms
{
    public static bool IsValidDag(Dictionary<Guid, List<Guid>> adjacencyList)
    {
        var visited = new HashSet<Guid>();
        var recursionStack = new HashSet<Guid>();

        foreach (var node in adjacencyList.Keys)
        {
            if (HasCycleDfs(node, adjacencyList, visited, recursionStack))
                return false;
        }

        return true;
    }

    private static bool HasCycleDfs(
        Guid node,
        Dictionary<Guid, List<Guid>> adjacencyList,
        HashSet<Guid> visited,
        HashSet<Guid> recursionStack)
    {
        if (recursionStack.Contains(node)) return true;
        if (visited.Contains(node)) return false;

        visited.Add(node);
        recursionStack.Add(node);

        if (adjacencyList.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (HasCycleDfs(neighbor, adjacencyList, visited, recursionStack))
                    return true;
            }
        }

        recursionStack.Remove(node);
        return false;
    }

    public static List<Guid> TopologicalSort(Dictionary<Guid, List<Guid>> adjacencyList)
    {
        var visited = new HashSet<Guid>();
        var result = new Stack<Guid>();

        foreach (var node in adjacencyList.Keys)
        {
            if (!visited.Contains(node))
                TopologicalSortDfs(node, adjacencyList, visited, result);
        }

        return result.ToList();
    }

    private static void TopologicalSortDfs(
        Guid node,
        Dictionary<Guid, List<Guid>> adjacencyList,
        HashSet<Guid> visited,
        Stack<Guid> result)
    {
        visited.Add(node);

        if (adjacencyList.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                    TopologicalSortDfs(neighbor, adjacencyList, visited, result);
            }
        }

        result.Push(node);
    }

    public static List<Guid> Bfs(Guid startNode, Dictionary<Guid, List<Guid>> adjacencyList)
    {
        var visited = new HashSet<Guid>();
        var queue = new Queue<Guid>();
        var result = new List<Guid>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);

            if (adjacencyList.TryGetValue(current, out var neighbors))
            {
                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return result;
    }

    public static List<Guid> Dfs(Guid startNode, Dictionary<Guid, List<Guid>> adjacencyList)
    {
        var visited = new HashSet<Guid>();
        var result = new List<Guid>();
        DfsHelper(startNode, adjacencyList, visited, result);
        return result;
    }

    private static void DfsHelper(
        Guid node,
        Dictionary<Guid, List<Guid>> adjacencyList,
        HashSet<Guid> visited,
        List<Guid> result)
    {
        visited.Add(node);
        result.Add(node);

        if (adjacencyList.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                    DfsHelper(neighbor, adjacencyList, visited, result);
            }
        }
    }
}
