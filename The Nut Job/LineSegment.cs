using Microsoft.Xna.Framework;

namespace The_Nut_Job
{
    class LineSegment
    {
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }

        public LineSegment(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }
    }
}
