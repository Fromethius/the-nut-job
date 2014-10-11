using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace The_Nut_Job
{
    public class TheNutJobGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Sketcher sketcher;
        private Ball ball;
        private CollisionSystem collisionSystem;
        private FPS fps = new FPS();

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
            ball = new Ball(Content.Load<Texture2D>("ball.png"));
            collisionSystem = new CollisionSystem();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            HandlePossibleBallFall();

            if (Input.IsMouseLeftClick())
            {
                sketcher.StartNewPath();
            }
            else if (Input.IsMouseLeftDown())
            {
                sketcher.AddPoint(new Vector2(Input.MouseX, Input.MouseY));
            }

            if (Input.IsMouseRightClick())
            {
                ball.InitializeMotionVectors();
            }

            collisionSystem.Update(ball, sketcher.Paths, gameTime);
            fps.Update(gameTime);

            ball.Update(gameTime);
            base.Update(gameTime);
        }

        private void HandlePossibleBallFall()
        {
            if (ball.Position.Y > 600)
            {
                ball.InitializeMotionVectors();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            sketcher.DrawPaths(spriteBatch);

            ball.Draw(spriteBatch);
            // fps.Draw(gameTime);

            Console.WriteLine(sketcher.Paths.Count);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
