using UnityEngine;

namespace Navigation.Runtime
{
    public struct NavigationPath
    {
        public Vector3[] Corners;
        public float TotalCost;
    }
}