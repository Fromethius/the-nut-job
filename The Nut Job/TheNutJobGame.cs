using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Nut_Job
{
    public class TheNutJobGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Sketcher sketcher;
        private Ball ball;
        private CollisionSystem collisionSystem;

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

            collisionSystem.Update(ball, sketcher.lines, gameTime);

            ball.Update(gameTime);
            base.Update(gameTime);
        }

        private void HandlePossibleBallFall()
        {
            if (ball.Position.Y > 600)
            {
                ball.SetPosition(new Vector2(300, 0));
                ball.SetVelocity(Vector2.Zero);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            sketcher.DrawPaths(spriteBatch);

            ball.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
