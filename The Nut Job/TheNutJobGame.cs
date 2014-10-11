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
        private Camera camera;
        private const int mapWidh = 2000, mapHeight = 2000;

        public TheNutJobGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
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

            camera = new Camera(new Rectangle(0, 0, mapWidh, mapHeight), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, mapWidh, mapHeight);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            HandlePossibleBallFall();
            HandlePathDrawing();
            HandleCameraInput();

            collisionSystem.Update(ball, sketcher.Paths, gameTime);
            fps.Update(gameTime);

            ball.Update(gameTime);
            base.Update(gameTime);
        }

        private void HandlePathDrawing()
        {
            if (Input.IsMouseLeftClick())
            {
                sketcher.StartNewPath();
            }
            else if (Input.IsMouseLeftDown())
            {
                Vector2 mousePosition = camera.ScreenToUniverse(new Vector2(Input.MouseX, Input.MouseY));
                sketcher.AddPoint(mousePosition);
            }

            if (Input.IsMouseRightClick())
            {
                ball.InitializeMotionVectors();
            }
        }

        private void HandleCameraInput()
        {
            Vector2 pan = Vector2.Zero;
            float distance = 10f;

            pan.X += Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) ? distance : 0;
            pan.X -= Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) ? distance : 0;
            pan.Y -= Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) ? distance : 0;
            pan.Y += Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) ? distance : 0;

            camera.Pan(pan);

            if (Input.DeltaWheelValue > 0 && camera.Zoom < 2)
            {
                camera.Zoom += .1f;

                Vector2 position = camera.ScreenToUniverse(new Vector2(Input.MouseX, Input.MouseY));
                camera.LookAt(position);
            }
            else if (Input.DeltaWheelValue < 0)
            {
                camera.Zoom -= .1f;
            }
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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, camera.Transformation);

            sketcher.DrawPaths(spriteBatch);

            ball.Draw(spriteBatch);
            // fps.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
