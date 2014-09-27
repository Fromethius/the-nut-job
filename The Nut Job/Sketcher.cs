using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Path = System.Collections.Generic.HashSet<Microsoft.Xna.Framework.Vector2>;

namespace The_Nut_Job
{
    class Sketcher
    {
        private readonly Texture2D whitePixel;
        private readonly Stack<Path> lines = new Stack<Path>();

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
            lines.Peek().Add(point);
        }

        public void DrawPaths(SpriteBatch spriteBatch)
        {
            foreach (Path line in lines)
            {
                Vector2 previousPoint = Vector2.Zero;

                foreach (Vector2 point in line)
                {
                    if (previousPoint != Vector2.Zero)
                    {
                        DrawLine(spriteBatch, previousPoint, point);
                    }

                    previousPoint = point;
                }
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;

            int distance = (int)edge.Length();
            int lineWidth = 1;

            Rectangle stretchLine = new Rectangle((int)start.X, (int)start.Y, distance, lineWidth);
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(whitePixel, stretchLine, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
