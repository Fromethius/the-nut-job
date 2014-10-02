using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Path = System.Collections.Generic.List<The_Nut_Job.LinePoint>;

namespace The_Nut_Job
{
    class Sketcher
    {
        private readonly Texture2D whitePixel;
        public readonly Stack<Path> lines = new Stack<Path>();

        public Sketcher(GraphicsDevice graphicsDevice)
        {
            whitePixel = new Texture2D(graphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });
        }

        public void StartNewPath()
        {
            lines.Push(new Path());
        }

        public void AddPoint(Vector2 point)
        {
            LinePoint newPoint = new LinePoint();

            if (lines.Peek().Any(c => c.Position == point))
            {
                return;
            }

            if (lines.Peek().Count > 0)
            {
                LinePoint previousPoint = lines.Peek().Last();
                previousPoint.Next = newPoint;
                newPoint.Previous = previousPoint;
            }

            newPoint.Position = point;

            lines.Peek().Add(newPoint);
        }

        public void DrawPaths(SpriteBatch spriteBatch)
        {
            foreach (Path line in lines)
            {
                foreach (LinePoint point in line)
                {
                    if (point.Previous != null)
                    {
                        DrawLine(spriteBatch, point.Previous.Position, point.Position);
                    }
                }
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;

            int distance = (int)edge.Length();
            int lineWidth = 3;

            Rectangle stretchLine = new Rectangle((int)start.X, (int)start.Y, distance, lineWidth);
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(whitePixel, stretchLine, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
