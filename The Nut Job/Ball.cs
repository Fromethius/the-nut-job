using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Nut_Job
{
    class Ball
    {
        private readonly Texture2D image;
        private Vector2 previousPosition;

        public Ball(Texture2D image)
        {
            this.image = image;
            Radius = image.Width / 2;
            InitializeMotionVectors();
        }

        public int Radius { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 PositionByOrigin { get { return new Vector2(Position.X + Radius, Position.Y + Radius); } }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; private set; }

        public void Update(GameTime gameTime)
        {
            previousPosition = Position;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += (0.5f * Acceleration * dt + Velocity) * dt;
            Velocity += Acceleration * dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, Color.White);
        }

        public void Bounce(Vector2 normal, bool revertPosition = true)
        {
            if (revertPosition)
            {
                Position = previousPosition;
            }
            else 
            {
                Position = Position - .25f*Vector2.Normalize(Vector2.Dot(Velocity, normal) * normal);
            }
            Vector2 height = Vector2.Dot(Velocity, normal) * normal;
            Vector2 roll = Velocity - height;
            float friction = 1f;
            float elasticity = .6f;
            Velocity = friction * roll - elasticity * height;
            //Velocity = Vector2.Reflect(Velocity, normal) * 0.9f;
        }

        public void InitializeMotionVectors()
        {
            Position = new Vector2(500, 0);
            Velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 200);
        }
    }
}
