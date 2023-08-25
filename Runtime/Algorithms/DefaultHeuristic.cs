using System;
using UnityEngine;

namespace Navigation.Runtime.Algorithms
{
    public class DefaultHeuristic : IHeuristic
    {
        public float Value(Vector2Int v1, Vector2Int v2)
        {
            return Math.Abs(v1.x - v2.x) + Math.Abs(v1.y - v2.y);
        }
    }
}