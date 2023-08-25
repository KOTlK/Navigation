using UnityEngine;

namespace Navigation.Runtime.Algorithms
{
    public interface IHeuristic
    {
        float Value(Vector2Int v1, Vector2Int v2);
    }
}