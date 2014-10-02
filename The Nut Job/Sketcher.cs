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
            if (lines.Peek().Any(c => c == point))
            {
                return;
            }

            lines.Peek().Add(point);
        }

        public void DrawPaths(SpriteBatch spriteBatch)
        {
            foreach (Path line in lines)
            {
                for (int i = 1; i < line.Count; i++)
                {
                    DrawLine(spriteBatch, line[i - 1], line[i]);
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
