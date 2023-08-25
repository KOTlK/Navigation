using UnityEngine;

namespace Navigation.Runtime
{
    public class PassableNavigationArea : RectangleNavigationArea
    {
        [SerializeField] private double _cost;

        public override double Cost => _cost;
    }
}