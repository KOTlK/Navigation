namespace Navigation.Runtime
{
    public class NavigationObstacle : RectangleNavigationArea
    {
        public override double Cost => double.MaxValue;
    }
}