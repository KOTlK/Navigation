using UnityEngine;

namespace Navigation.Runtime
{
    public abstract class NavigationArea : MonoBehaviour
    {
        public abstract Vector3[] Corners { get; }
        public abstract double Cost { get; }

        public abstract bool Contains(Vector3 point);
    }
}