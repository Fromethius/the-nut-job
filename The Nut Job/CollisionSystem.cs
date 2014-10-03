

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
            if (HandlePossibleLineBallCollisions(CollisionType.End, ball, lines, gameTime))
            {
                return;
            }

            HandlePossibleLineBallCollisions(CollisionType.Segment, ball, lines, gameTime);
        }

        private enum CollisionType
        {
            End,
            Segment
        }

        private bool HandlePossibleLineBallCollisions(CollisionType collisionType, Ball ball, List<Path> lines, GameTime gameTime)
        {
            Vector2 origin = ball.PositionByOrigin;

            for (int degrees = 0; degrees < 360; degrees++)
            {
                Vector2 circumferencePosition = CalculateCircumferencePosition(origin, ball.Radius, degrees);

                foreach (var path in lines)
                {
                    if (path.Count == 0) { continue; }

                    if (collisionType == CollisionType.End)
                    {
                        foreach (Vector2 point in new List<Vector2>() { path.First().Start, path.Last().End })
                        {
                            if (IsInsideCircle(point, origin, ball.Radius))
                            {
                                ball.Bounce(Vector2.Normalize(point - origin));
                                return true;
                            }
                        }
                    }
                    else if (collisionType == CollisionType.Segment)
                    {
                        foreach (LineSegment lineSegment in path)
                        {
                            Vector2? intersectionPoint = LineSegmentIntersect(origin, circumferencePosition, lineSegment.Start, lineSegment.End);

                            if (intersectionPoint.HasValue)
                            {
                                ball.Bounce(Perpendicular(Vector2.Normalize(lineSegment.End - lineSegment.Start)));
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsInsideCircle(Vector2 point, Vector2 origin, float radius)
        {
            return MathHelp.Pow(point.X - origin.X, 2) + MathHelp.Pow(point.Y - origin.Y, 2) < MathHelp.Pow(radius, 2);
        }

        private Vector2 CalculateCircumferencePosition(Vector2 origin, int radius, int degrees)
        {
            float xOffset = radius * (float)Math.Cos(degrees);
            float yOffset = radius * (float)Math.Sin(degrees);

            return new Vector2(origin.X + xOffset, origin.Y + yOffset);
        }

        private Vector2? LineSegmentIntersect(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            Vector2 s1 = p1 - p0;
            Vector2 s2 = p3 - p2;

            float s = (-s1.Y * (p0.X - p2.X) + s1.X * (p0.Y - p2.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
            float t = (s2.X * (p0.Y - p2.Y) - s2.Y * (p0.X - p2.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                return new Vector2(p0.X + (t * s1.X), p0.Y + (t * s1.Y));
            }

            return null; // No collision
        }

        private Vector2 Perpendicular(Vector2 original)
        {
            return new Vector2(-1 * original.Y, original.X);      
        }
    }
}


