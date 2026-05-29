import type { Module } from '../types/path.types';

export const buildAdjacencyList = (modules: Module[]): Map<string, string[]> => {
  const adjacencyList = new Map<string, string[]>();
  modules.forEach(m => adjacencyList.set(m.id, m.prerequisiteIds));
  return adjacencyList;
};

export const getUnlockedModules = (modules: Module[], completedIds: Set<string>): Module[] => {
  return modules.filter(m =>
    m.prerequisiteIds.length === 0 ||
    m.prerequisiteIds.every(prereqId => completedIds.has(prereqId))
  );
};
