using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Navigation.Runtime.Algorithms
{
    public class AStar : IPathFindingAlgorithm
    {
        private readonly NavigationGraph _navigationGraph;
        private readonly IHeuristic _heuristic;

        public AStar(NavigationGraph navigationGraph, IHeuristic heuristic)
        {
            _navigationGraph = navigationGraph;
            _heuristic = heuristic;
        }

        public List<NavigationCell> CalculateCellsPath(Vector2Int from, Vector2Int to)
        {
            var start = _navigationGraph.Cell(from);
            var end = _navigationGraph.Cell(to);

            var frontier = new PriorityQueue<NavigationCell, double>();
            frontier.Enqueue(start, 0);
            var cameFrom = new Dictionary<NavigationCell, NavigationCell>();
            var costSoFar = new Dictionary<NavigationCell, double>();
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.PositionInGraph == to)
                    break;

                foreach (var next in _navigationGraph.Neighbours(current.PositionInGraph))
                {
                    var newCost = costSoFar[current] + next.Cost;

                    if (costSoFar.ContainsKey(next) == false ||
                        newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        var priority = newCost + _heuristic.Value(next.PositionInGraph, to);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            
            var cellsPath = new List<NavigationCell>();
            var currentCell = end;

            while (currentCell != start)
            {
                cellsPath.Add(currentCell);
                currentCell = cameFrom[currentCell];
            }

            cellsPath.Add(start);
            cellsPath.Reverse();

            return cellsPath;
        }
    }
}