export const isValidDag = (adjacencyList: Map<string, string[]>): boolean => {
  const visited = new Set<string>();
  const recursionStack = new Set<string>();

  const hasCycle = (node: string): boolean => {
    if (recursionStack.has(node)) return true;
    if (visited.has(node)) return false;

    visited.add(node);
    recursionStack.add(node);

    const neighbors = adjacencyList.get(node) || [];
    for (const neighbor of neighbors) {
      if (hasCycle(neighbor)) return true;
    }

    recursionStack.delete(node);
    return false;
  };

  for (const node of adjacencyList.keys()) {
    if (hasCycle(node)) return false;
  }
  return true;
};

export const topologicalSort = (adjacencyList: Map<string, string[]>): string[] => {
  const visited = new Set<string>();
  const result: string[] = [];

  const dfs = (node: string) => {
    visited.add(node);
    const neighbors = adjacencyList.get(node) || [];
    for (const neighbor of neighbors) {
      if (!visited.has(neighbor)) dfs(neighbor);
    }
    result.unshift(node);
  };

  for (const node of adjacencyList.keys()) {
    if (!visited.has(node)) dfs(node);
  }
  return result;
};
