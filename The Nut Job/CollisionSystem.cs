using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace The_Nut_Job
{
    class CollisionSystem
    {
        public void Update(Ball ball, List<Path> lines, GameTime gameTime)
        {
            if (HandleBallEndPointCollisions(ball, lines, gameTime))
            {
                return;
            }

            HandlePossibleLineSegmentCollisions(ball, lines, gameTime);
        }

        private bool HandleBallEndPointCollisions(Ball ball, List<Path> paths, GameTime gameTime)
        {
            Vector2 origin = ball.PositionByOrigin;

            foreach (var path in paths)
            {
                if (path.Count == 0) { continue; }

                foreach (Vector2 point in new List<Vector2>() { path.First().Start, path.Last().End })
                {
                    if (IsInsideCircle(point, origin, ball.Radius))
                    {
                        ball.Bounce(Vector2.Normalize(point - origin));
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IntersectLineCircle(Vector2 circleOrigin, float radius, LineSegment lineSegment)
        {
            Vector2 ac = circleOrigin - lineSegment.Start;
            Vector2 ab = lineSegment.End - lineSegment.Start;

            // float t = Vector2.Dot(ac, ab) / Vector2.Dot(ab, ab);
            // t = t.Clamp(0, 1);
            // Vector2 h = (ab * t + lineSegment.Start) - circleOrigin;
            // return Vector2.Dot(h, h) <= (radius * radius); // h.Length <= radius

            Vector2 ad = Vector2.Dot(ac, ab) / Vector2.Dot(ab, ab) * ab;

            //Vector2 h = circleOrigin - ad;
            //return Vector2.Dot(ac - ad, ac - ad) <= radius * radius;
            int x = 5;

            return Vector2.Distance(ad, ac) <= radius;
        }

        private bool HandlePossibleLineSegmentCollisions(Ball ball, List<Path> paths, GameTime gameTime)
        {
            Vector2 origin = ball.PositionByOrigin;

            foreach (var path in paths)
            {
                if (path.Count == 0) { continue; }

                foreach (LineSegment lineSegment in path)
                {
                    if (IntersectLineCircle(origin, ball.Radius, lineSegment))
                    {
                        ball.Bounce(Perpendicular(Vector2.Normalize(lineSegment.End - lineSegment.Start)));
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsInsideCircle(Vector2 point, Vector2 origin, float radius)
        {
            return MathHelp.Pow(point.X - origin.X, 2) + MathHelp.Pow(point.Y - origin.Y, 2) <= MathHelp.Pow(radius, 2);
        }

        private Vector2 Perpendicular(Vector2 original)
        {
            return new Vector2(-1 * original.Y, original.X);
        }
    }
}
