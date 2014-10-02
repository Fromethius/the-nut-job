using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace The_Nut_Job
{
    class Sketcher
    {
        private readonly Texture2D whitePixel;
        private Vector2? previousPoint = null;

        public List<Path> Lines { get; private set; }

        public Sketcher(GraphicsDevice graphicsDevice)
        {
            whitePixel = new Texture2D(graphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });

            Lines = new List<Path>();
        }

        public void StartNewPath()
        {
            previousPoint = null;

            Lines.Add(new Path());
        }

        public void AddPoint(Vector2 point)
        {
            Path path = Lines.Last();

            if (previousPoint.HasValue && point != previousPoint)
            {
                path.Add(new LineSegment(previousPoint.Value, point));
            }

            previousPoint = point;
        }

        public void DrawPaths(SpriteBatch spriteBatch)
        {
            foreach (Path path in Lines)
            {
                foreach (LineSegment lineSegment in path)
                {
                    DrawLine(spriteBatch, lineSegment.Start, lineSegment.End);
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
