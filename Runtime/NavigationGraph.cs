using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation.Runtime
{
    public class NavigationGraph
    {
        private readonly NavigationData _navigationData;
        private readonly float _angle;

        private readonly Dictionary<Vector2Int, NavigationCell[]> _neighbours = new();

        public NavigationGraph(NavigationData navigationData, float angle)
        {
            _navigationData = navigationData;
            _angle = angle;
            CacheNeighbours();
        }

        public NavigationData NavigationData => _navigationData;

        private static readonly Vector2Int[] Directions = new Vector2Int[]
        {
            new(1, 0),
            new(0, 1),
            new(-1, 0),
            new(0, -1)
        };

        public bool CanReach(Vector2Int position)
        {
            return _neighbours.ContainsKey(position);
        }

        public NavigationCell Cell(Vector2Int position)
        {
            var node = _navigationData.Cells[position.y + position.x * _navigationData.Size.x];

            return node;
        }

        public NavigationCell[] Neighbours(Vector2Int position)
        {
            return _neighbours[position];
        }

        public void Visualize(bool drawNeighbours, bool drawCells)
        {
            if (!drawNeighbours && !drawCells)
                return;
            
            var cells = _navigationData.Cells;
            
            foreach (var cell in cells)
            {
                if (drawNeighbours == false) 
                    continue;

                var neighbours = Neighbours(cell.PositionInGraph);
                
                foreach (var neighbour in neighbours)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(cell.Position, neighbour.Position);
                }
            }

            if (drawCells)
            {
                foreach (var cell in cells)
                {
                    for (var i = 0; i < cell.Vertices.Length; i++)
                    {
                        var next = i + 1;

                        if (next == cell.Vertices.Length)
                        {
                            next = 0;
                        }

                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(cell.Vertices[i], cell.Vertices[next]);
                    }

                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(cell.Position, Vector3.up);
                }
            }
        }

        private void CacheNeighbours()
        {
            _neighbours.Clear();
            
            foreach (var cell in _navigationData.Cells)
            {
                if(cell.Cost == 0)
                    continue;
                
                var neighbours = FindNeighbours(cell.PositionInGraph);
                
                if(neighbours == Array.Empty<NavigationCell>())
                    continue;

                _neighbours[cell.PositionInGraph] = neighbours;
            }
        }

        private NavigationCell[] FindNeighbours(Vector2Int position)
        {
            var result = new List<NavigationCell>();
            var index = position.y + position.x * _navigationData.Size.x;

            if (index < 0)
                return Array.Empty<NavigationCell>();

            if (index > _navigationData.Cells.Length - 1)
                return Array.Empty<NavigationCell>();


            foreach (var direction in Directions)
            {
                var targetPosition = position + direction;

                if (targetPosition.x < 0)
                    continue;

                if (targetPosition.y < 0)
                    continue;

                if (targetPosition.x >= _navigationData.Size.x)
                    continue;

                if (targetPosition.y >= _navigationData.Size.y)
                    continue;

                var targetIndex = targetPosition.y + targetPosition.x * _navigationData.Size.x;
                var targetNode = _navigationData.Cells[targetIndex];

                var node = _navigationData.Cells[index];
                
                var directionToTarget = (targetNode.Position - node.Position).normalized;
                var axis = Vector3.up * Mathf.Sign(directionToTarget.y);

                if (Vector3.Angle(directionToTarget, axis) > _angle)
                {
                    result.Add(_navigationData.Cells[targetIndex]);
                }
            }

            return result.ToArray();
        }
    }
}