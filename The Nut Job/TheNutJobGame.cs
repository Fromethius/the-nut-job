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
            ball.Update(gameTime);

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
                    if (path.Contains(circumferencePosition))
                    {
                        int collisionIndex = path.IndexOf(circumferencePosition);
                        Vector2 previousPoint = path[collisionIndex - 1];
                        Vector2 nextPoint = path[collisionIndex + 1];

                        Vector2 connecting = nextPoint - previousPoint;
                        Vector2 normal = new Vector2();
                        ball.Bounce();
                    }


                }
            }

            if (ball.Position.Y > 1000)
            {
                ball.SetInitialPosition(ball.Position.X, 0);
                ball.SetVelocity(new Vector2(0, 10));
            }

            base.Update(gameTime);
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
