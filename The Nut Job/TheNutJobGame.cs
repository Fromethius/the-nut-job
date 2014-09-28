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
        private Ball ball = new Ball(new Vector2(200, 0));//Change Vector2 based on level.

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
            ball.LoadImage(Content.Load<Texture2D>("ball.png"));
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

            for (int degrees = 0; degrees < 360; degrees++)
            {
                float radius = ball.Image.Width / 2;
                Vector2 origin = new Vector2(ball.Position.X + radius, ball.Position.Y + radius);

                float xOffset = radius * (float)Math.Cos(degrees);
                float yOffset = radius * (float)Math.Sin(degrees);

                Vector2 circumferencePosition = new Vector2(origin.X + xOffset, origin.Y + yOffset);

                foreach (var path in sketcher.lines)
                {
                    for (int i = 1; i < path.Count - 1; i++)
                    {
                        LinePoint point = path[i];

                        Vector2 p = origin;
                        Vector2 p2 = circumferencePosition * 1.1f;
                        Vector2 q = point.Previous.Position;
                        Vector2 q2 = point.Next.Position;

                        Vector2? intersectionPoint = GetLineIntersectionPoint(p, p2, q, q2);

                        // bool intersectionHappened = Intersection(q2 - q, p, ball.Image.Width / 2);

                        if (intersectionPoint.HasValue)
                        {
                            p2 = intersectionPoint.Value;

                            Vector2 directionP = p2 - p;
                            Vector2 directionQ = q2 - q;

                            Vector2 u = (Vector2.Dot(directionP, directionQ) / Vector2.Dot(directionQ, directionQ)) * directionQ;
                            Vector2 w = directionP - u;

                            Vector2 newVelocity = w - u;

                            ball.SetVelocity(newVelocity);
                        }
                    }

                    // New vector == -2 * (V dot N) * N + V
                }
            }

            if (ball.Position.Y > 1000)
            {
                ball.SetInitialPosition(ball.Position.X, 0);
                ball.SetVelocity(new Vector2(0, 10));
            }

            ball.Update(gameTime);
            base.Update(gameTime);
        }

        public bool Intersection(Vector2 d, Vector2 f, float r)
        {
            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - r * r;

            float discriminant = b * b - 4 * a * c;
            if (discriminant >= 0)
            {
                discriminant = (float)Math.Sqrt(discriminant);

                float t1 = (-b - discriminant) / (2 * a);
                float t2 = (-b + discriminant) / (2 * a);

                return (t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1);
            }

            return false;
        }

        Vector2? GetLineIntersectionPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            Vector2 s1 = new Vector2(p1.X - p0.X, p1.Y - p0.Y);
            Vector2 s2 = new Vector2(p3.X - p2.X, p3.Y - p2.Y);

            float s = (-s1.Y * (p0.X - p2.X) + s1.X * (p0.Y - p2.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
            float t = (s2.X * (p0.Y - p2.Y) - s2.Y * (p0.X - p2.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                return new Vector2(p0.X + (t * s1.X), p0.Y + (t * s1.Y));
            }

            return null; // No collision
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
