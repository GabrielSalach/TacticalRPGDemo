using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder 
{
    CellMatrix cellMatrix;
    public List<Cell> path;
    public DijkstraData dijkstraData;


    public Pathfinder(CellMatrix matrix) {
        cellMatrix = matrix;
    }

    Dictionary<Cell, List<Cell>> ConstructGraph(CellMatrix cellMatrix) {
        // Creates a graph from a cellMatrix
        Dictionary<Cell, List<Cell>> graph;
        graph = new Dictionary<Cell, List<Cell>>();

        foreach(Cell cell in cellMatrix.matrix) {
            //Adds the cell to the graph only if there's no unit on top or if it's a player unit
            int x = cell.GetCellCoords().x;
            int y = cell.GetCellCoords().y;
            graph.Add(cell, new List<Cell>());
            List<Cell> neighbors = cell.GetNeighbors();
            foreach(Cell neighbor in neighbors) {
                graph[cell].Add(neighbor);
            }
        }

        return graph;
    }

    public struct DijkstraData {
        public List<Cell> cells;
        public Dictionary<Cell, int> distances;
        public Dictionary<Cell, Cell> cameFrom;
    }

    void PrintGraph(Dictionary<Cell, List<Cell>> graph) {
        foreach(KeyValuePair<Cell, List<Cell>> duo in graph) {
            Debug.Log("Cell : " + duo.Key.ToString());
            Debug.Log("Neighbors :");
            foreach(Cell cell in duo.Value) {
                Debug.Log("     " + cell.ToString());    
            }
            Debug.Log("-------------------------------------");
        }
    }

    public DijkstraData Dijkstra(Cell start) {
        Dictionary<Cell, List<Cell>> graph = ConstructGraph(cellMatrix);
        Dictionary<Cell, int> distances = new Dictionary<Cell, int>();
        foreach(Cell cell in graph.Keys) {
            if(cell == start) {
                distances[cell] = 0;
            } else {
                distances[cell] = int.MaxValue;
            }
        }

        // Initialize the unvisited cells queue and add the start cell
        Queue<Cell> unvisited = new Queue<Cell>();
        unvisited.Enqueue(start);
        List<Cell> visited = new List<Cell>();

        // Initialize the previous cell dictionary
        Dictionary<Cell, Cell> previous = new Dictionary<Cell, Cell>();

        while (unvisited.Count > 0)
        {
            // Dequeue the cell with the smallest distance
            Cell current = unvisited.Dequeue();
            visited.Add(current);
            // Update the distances of the current cell's neighbors
            foreach (Cell neighbor in graph[current])
            {
                // Skip this neighbor if there is a unit blocking it
                if (neighbor.GetUnitOnTop() != null)
                {
                    continue;
                }

                // Calculate the distance to this neighbor
                int distance = distances[current] + 1;

                // If this distance is shorter than the current distance, update it
                if (distance < distances[neighbor])
                {
                    distances[neighbor] = distance;
                    previous[neighbor] = current;

                    // Add the neighbor to the unvisited queue
                    unvisited.Enqueue(neighbor);
                }
            }
        }

        // Constructs the list of path
        DijkstraData data = new DijkstraData();
        data.cells = visited;
        data.distances = distances;
        data.cameFrom = previous;
        return data;
    }


    public void InitializeDijkstra(Cell origin) {
        dijkstraData = Dijkstra(origin);
    }

    public List<Cell> GetPath(Cell origin, Cell destination) {
        path = new List<Cell>();
        Cell current = destination;
        bool originFound = false;
        while(originFound == false) {
            path.Add(current);
            if(current == origin) {
                originFound = true;
                break;
            }
            current = dijkstraData.cameFrom[current];
        }
        path.Reverse();
        return path;
    }

    public void ClearPath() {
        path = null;
    }


}