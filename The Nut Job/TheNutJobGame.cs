using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace The_Nut_Job
{
    public class TheNutJobGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Sketcher sketcher;
        private Ball ball;

        public TheNutJobGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sketcher = new Sketcher(GraphicsDevice);
            ball = new Ball(Content.Load<Texture2D>("ball.png"), new Vector2(500, 0));
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (Input.IsMouseLeftClick())
            {
                sketcher.StartNewPath();
            }
            else if (Input.IsMouseLeftDown())
            {
                sketcher.AddPoint(new Vector2(Input.MouseX, Input.MouseY));
            }

            if (!HandlePossibleEndPointCollisions(gameTime))
            {
                HandlePossibleEdgeCollisions(gameTime);
            }

            if (sketcher.lines.Count > 0)
            {
                Console.WriteLine(sketcher.lines.Peek().Count);
            }

            if (ball.Position.Y > 600)
            {
                ball.SetPosition(new Vector2(300, 0));
                ball.SetVelocity(Vector2.Zero);
            }

            ball.Update(gameTime);
            base.Update(gameTime);
        }

        private bool HandlePossibleEndPointCollisions(GameTime gameTime)
        {
            Vector2 origin = new Vector2(ball.Position.X + ball.Radius, ball.Position.Y + ball.Radius);

            for (int degrees = 0; degrees < 360; degrees++)
            {
                Vector2 circumferencePosition = CalculateCircumferencePosition(origin, ball.Radius, degrees);

                foreach (var path in sketcher.lines)
                {
                    if (path.Count <= 1) { return false; }

                    Vector2? endPointIntersect = EndPointIntersect(origin, circumferencePosition, path.First()) ?? EndPointIntersect(origin, circumferencePosition, path.Last());

                    if (endPointIntersect.HasValue)
                    {
                        Vector2 lineIntersect = Vector2.Normalize(endPointIntersect.Value - origin);
                        ball.Bounce(lineIntersect);
                        ball.Update(gameTime);
                        base.Update(gameTime);
                        return true;
                    }
                }
            }

            return false;
        }

        private void HandlePossibleEdgeCollisions(GameTime gameTime)
        {
            Vector2 origin = new Vector2(ball.Position.X + ball.Radius, ball.Position.Y + ball.Radius);

            for (int degrees = 0; degrees < 360; degrees++)
            {
                Vector2 circumferencePosition = CalculateCircumferencePosition(origin, ball.Radius, degrees);

                foreach (var path in sketcher.lines)
                {
                    for (int i = 1; i < path.Count - 1; i++)
                    {
                        LinePoint point = path[i];

                        Vector2? intersectionPoint = LineSegmentIntersect(origin, circumferencePosition, point.Previous.Position, point.Next.Position);

                        if (intersectionPoint.HasValue)
                        {
                            Vector2 connecting = Vector2.Normalize(point.Previous.Position - point.Next.Position);
                            ball.Bounce(Perpindicular(connecting));
                            ball.Update(gameTime);
                            base.Update(gameTime);
                            return;
                        }
                    }
                }
            }
        }

        private Vector2 CalculateCircumferencePosition(Vector2 origin, int radius, int degrees)
        {
            float xOffset = radius * (float)Math.Cos(degrees);
            float yOffset = radius * (float)Math.Sin(degrees);

            return new Vector2(origin.X + xOffset, origin.Y + yOffset);
        }

        private Vector2? EndPointIntersect(Vector2 p0, Vector2 p1, LinePoint linePoint)
        {
            Vector2 p2 = linePoint.Position;
            Vector2 p3 = linePoint.Previous == null ? linePoint.Next.Position : linePoint.Previous.Position;

            return LineSegmentIntersect(p0, p1, p2, p3);
        }

        Vector2? LineSegmentIntersect(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
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

        Vector2 Perpindicular(Vector2 original)
        {
            float x = original.X;
            float y = -1 * original.Y;
            return new Vector2(y, x);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            sketcher.DrawPaths(spriteBatch);

            ball.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
