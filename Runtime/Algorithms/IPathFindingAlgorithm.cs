using System.Collections.Generic;
using UnityEngine;

namespace Navigation.Runtime.Algorithms
{
    public interface IPathFindingAlgorithm
    {
        List<NavigationCell> CalculateCellsPath(Vector2Int from, Vector2Int to);
    }
}