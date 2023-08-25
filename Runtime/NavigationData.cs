using UnityEngine;

namespace Navigation.Runtime
{
    public class NavigationData : ScriptableObject
    {
        public NavigationCell[] Cells;
        public Vector2Int Size;
        public Vector3 Scale;
    }
}