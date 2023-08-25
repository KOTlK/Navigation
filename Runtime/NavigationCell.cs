using System;
using UnityEngine;

namespace Navigation.Runtime
{
    [Serializable]
    public struct NavigationCell
    {
        public Vector2Int PositionInGraph;
        public Vector3 Position;
        public double Cost;
        public Vector3[] Vertices;

        public static bool operator ==(NavigationCell first, NavigationCell second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(NavigationCell first, NavigationCell second)
        {
            return !(first == second);
        }

        public bool Equals(NavigationCell other)
        {
            return other.PositionInGraph == PositionInGraph;
        }

        public override bool Equals(object obj)
        {
            if (obj is NavigationCell node)
            {
                return Equals(node);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PositionInGraph.GetHashCode());
        }
    }
}