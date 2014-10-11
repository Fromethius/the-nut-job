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

        public List<Path> Paths { get; private set; }

        public Sketcher(GraphicsDevice graphicsDevice)
        {
            whitePixel = new Texture2D(graphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });

            Paths = new List<Path>();
        }

        public void StartNewPath()
        {
            previousPoint = null;

            Paths.Add(new Path());
        }

        public void AddPoint(Vector2 point)
        {
            Path path = Paths.Last();

            if (previousPoint.HasValue && point != previousPoint)
            {
                path.Add(new LineSegment(previousPoint.Value, point));
            }

            previousPoint = point;
        }

        public void DrawPaths(SpriteBatch spriteBatch)
        {
            foreach (Path path in Paths)
            {
                foreach (LineSegment lineSegment in path)
                {
                    DrawLine(spriteBatch, lineSegment.Start, lineSegment.End);
                }
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
        {
            float distance = Vector2.Distance(start, end);
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            float thickness = 3.0f;

            spriteBatch.Draw(whitePixel, start, null, Color.White, angle, Vector2.Zero, new Vector2(distance, thickness), SpriteEffects.None, 0);
        }
    }
}
