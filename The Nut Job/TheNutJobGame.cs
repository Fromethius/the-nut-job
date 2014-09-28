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
        private Ball ball = new Ball(new Vector2(500, 0));//Change Vector2 based on level.
        private bool bounced = false;
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

                Vector2 circumferencePosition = new Vector2((int)(origin.X + xOffset), (int)(origin.Y + yOffset));

                foreach (var path in sketcher.lines)
                {
                    for (int i = 1; i < path.Count - 1; i++)
                    {
                        LinePoint point = path[i];

                        Vector2 p = origin;
                        Vector2 p2 = circumferencePosition;
                        Vector2 q = point.Previous.Position;
                        Vector2 q2 = point.Next.Position;

                        Vector2? intersectionPoint = LineSegmentIntersect(p, p2, q, q2);
                        
                        if(intersectionPoint.HasValue)
                        {
                            Vector2 connecting = Vector2.Normalize(q - q2);
                            float x = connecting.X;
                            float y = -1* connecting.Y;
                            ball.Bounce(new Vector2(y, x));
                            ball.Update(gameTime);
                            base.Update(gameTime);
                            return;
                        }
                    }
                    
                    if (path.Count > 0 && path[0].Position.X == circumferencePosition.X)
                    {
                        LinePoint startPoint = path[0];
                        Vector2 end = startPoint.Position;
                        Vector2 next = startPoint.Next.Position;
                        ball.Bounce(Vector2.Normalize(end - next));
                        Console.WriteLine("Edge Collision");
                        ball.Update(gameTime);
                        base.Update(gameTime);
                        return;
                    }
                    else if (path.Count > 0 && path[path.Count - 1].Position == circumferencePosition)
                    {
                        int count = path.Count - 1;
                        LinePoint endPoint = path[count];
                        Vector2 end = endPoint.Position;
                        Vector2 next = endPoint.Next.Position;
                        ball.Bounce(Vector2.Normalize(end - next));
                        Console.WriteLine("Edge Collision");
                        ball.Update(gameTime);
                        base.Update(gameTime);
                        return;
                    }
                    // New vector == -2 * (V dot N) * N + V
                }
            }

            //if (ball.Position.Y >= 100 && !bounced)
            //{
            //    ball.Bounce(Vector2.Normalize(new Vector2(-90, 100)));
            //    //ball.SetInitialPosition(ball.Position.X, 0);
            //    //ball.SetVelocity(new Vector2(0, 0));
            //    bounced = !bounced;
            //}
            ball.Update(gameTime);
            base.Update(gameTime);
        }

        //private bool DoLineSegmentsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2)
        //{
        Vector2? LineSegmentIntersect(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
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
        //var r = p2 - p;
        //var s = q2 - q;
        //var uNumerator = crossProduct(q - p, r);
        //var denominator = crossProduct(r, s);
        //if (uNumerator == 0 && denominator == 0) // colinear, do they overlap?
        //{
        //    return ((q.X - p.X < 0) != (q.X - p2.X < 0) != (q2.X - p.X < 0) != (q2.X - p2.X < 0)) ||
        //    ((q.Y - p.Y < 0) != (q.Y - p2.Y < 0) != (q2.Y - p.Y < 0) != (q2.Y - p2.Y < 0));
        //}
        //if (denominator == 0) // lines are parallel
        //{
        //    return false;
        //}
        //var u = uNumerator / denominator;
        //var t = crossProduct(q - p, s) / denominator;
        //return (t >= 0) && (t <= 1) && (u >= 0) && (u <= 1);
        //}

        private float crossProduct(Vector2 point1, Vector2 point2)
        {
            return point1.X * point2.Y - point1.Y * point2.X;
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
