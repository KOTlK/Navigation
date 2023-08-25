using UnityEngine;

namespace Navigation.Runtime
{
    public abstract class RectangleNavigationArea : NavigationArea
    {
        [SerializeField] private Color _color;
        [SerializeField] private Vector3 _highPointOffset = Vector3.one;
        [SerializeField] private Vector3 _lowPointOffset = -Vector3.one;

        public abstract override double Cost { get; }

        public override Vector3[] Corners
        {
            get
            {
                var max = Vector3.zero;
                var min = Vector3.zero;

                if (_highPointOffset.x > _lowPointOffset.x)
                {
                    max.x = _highPointOffset.x;
                    min.x = _lowPointOffset.x;
                }
                else
                {
                    max.x = _lowPointOffset.x;
                    min.x = _highPointOffset.x;
                }

                if (_highPointOffset.z > _lowPointOffset.z)
                {
                    max.z = _highPointOffset.z;
                    min.z = _lowPointOffset.z;
                }
                else
                {
                    max.z = _lowPointOffset.z;
                    min.z = _highPointOffset.z;
                }


                return new Vector3[]
                {
                    new(min.x, 0, min.z),
                    new(min.x, 0, max.z),
                    new(max.x, 0, max.z),
                    new(max.x, 0, min.z)
                };
            }
        }

        public override bool Contains(Vector3 point)
        {
            var max = Max;
            var min = Min;

            return point.x < max.x &&
                   point.y < max.y &&
                   point.z < max.z &&
                   point.x > min.x &&
                   point.y > min.y &&
                   point.z > min.z;
        }

        private Vector3 Min
        {
            get
            {
                var highest = transform.position + _highPointOffset;
                var lowest = transform.position + _lowPointOffset;

                return new Vector3(lowest.x < highest.x ? lowest.x : highest.x,
                    lowest.y < highest.y ? lowest.y : highest.y,
                    lowest.z < highest.z ? lowest.z : highest.z);
            }
        }

        private Vector3 Max
        {
            get
            {
                var highest = transform.position + _highPointOffset;
                var lowest = transform.position + _lowPointOffset;

                return new Vector3(lowest.x > highest.x ? lowest.x : highest.x,
                    lowest.y > highest.y ? lowest.y : highest.y,
                    lowest.z > highest.z ? lowest.z : highest.z);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_highPointOffset.y < _lowPointOffset.y)
            {
                Debug.LogError("Highest point can not be lower than lowest");
            }

            var point0 = transform.position + _highPointOffset;
            var point7 = transform.position + _lowPointOffset;
            var point1 = new Vector3(point7.x, point0.y, point0.z);
            var point2 = new Vector3(point0.x, point7.y, point0.z);
            var point3 = new Vector3(point0.x, point0.y, point7.z);
            var point4 = new Vector3(point0.x, point7.y, point7.z);
            var point5 = new Vector3(point7.x, point0.y, point7.z);
            var point6 = new Vector3(point7.x, point7.y, point0.z);
            

            Gizmos.color = _color;

            Gizmos.DrawLine(point0, point1);
            Gizmos.DrawLine(point0, point2);
            Gizmos.DrawLine(point0, point3);
            Gizmos.DrawLine(point7, point4);
            Gizmos.DrawLine(point7, point5);
            Gizmos.DrawLine(point7, point6);
            Gizmos.DrawLine(point1, point5);
            Gizmos.DrawLine(point2, point6);
            Gizmos.DrawLine(point1, point6);
            Gizmos.DrawLine(point2, point4);
            Gizmos.DrawLine(point3, point4);
            Gizmos.DrawLine(point3, point5);
        }
    }
}