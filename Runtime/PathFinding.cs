using System.Collections.Generic;
using Navigation.Runtime.Algorithms;
using UnityEngine;

namespace Navigation.Runtime
{
    public class PathFinding
    {
        private readonly NavigationGraph _navigationGraph;
        private readonly IPathFindingAlgorithm _algorithm;

        public PathFinding(NavigationGraph navigationGraph, IPathFindingAlgorithm algorithm)
        {
            _navigationGraph = navigationGraph;
            _algorithm = algorithm;
        }

        public bool CanReach(Vector3 point)
        {
            var position = new Vector2Int(
                (int)(point.x / _navigationGraph.NavigationData.Scale.x),
                (int)(point.z / _navigationGraph.NavigationData.Scale.z));

            return _navigationGraph.CanReach(position);
        }

        public NavigationPath FindPath(Vector3 from, Vector3 to)
        {
            var start = new Vector2Int(
                (int)(from.x / _navigationGraph.NavigationData.Scale.x),
                (int)(from.z / _navigationGraph.NavigationData.Scale.z));
            
            var end = new Vector2Int(
                (int)(to.x / _navigationGraph.NavigationData.Scale.x),
                (int)(to.z / _navigationGraph.NavigationData.Scale.z));

            var cellsPath = _algorithm.CalculateCellsPath(start, end);

            var closestToStart = cellsPath[0].Vertices[0];
            var distance = (closestToStart - from).sqrMagnitude;

            for (var i = 1; i < cellsPath[0].Vertices.Length; i++)
            {
                var current = cellsPath[0].Vertices[i];
                var length = (current - from).sqrMagnitude;

                if (length < distance)
                {
                    closestToStart = current;
                    distance = length;
                }
            }

            return GeneratePath(cellsPath, closestToStart);
        }

        //Could be moved into another class
        private NavigationPath GeneratePath(IReadOnlyList<NavigationCell> cellsPath, Vector3 firstVertex)
        {
            var corners = new List<Vector3>();
            var currentVertex = firstVertex;
            corners.Add(currentVertex);

            for (var i = 1; i < cellsPath.Count; i++)
            {
                var cell = cellsPath[i];
                var closest = Vector3.negativeInfinity;
                var distance = float.MaxValue;

                foreach (var node in cell.Vertices)
                {
                    var length = (node - currentVertex).sqrMagnitude;
                    
                    if (length < distance)
                    {
                        closest = node;
                        distance = length;
                    }
                }

                corners.Add(closest);
                currentVertex = closest;
            }

            return new NavigationPath()
            {
                Corners = corners.ToArray()
            };
        }
    }
}