using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Nut_Job
{
    class Ball
    {
        private Texture2D image;
        private Vector2 previousPosition;

        public Ball(Texture2D image, Vector2 position)
        {
            this.image = image;

            Position = new Vector2(position.X, position.Y);
            Velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 200);
            Radius = image.Width / 2;
        }

        public int Radius { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 PositionByOrigin { get { return new Vector2(Position.X + Radius, Position.Y + Radius); } }
        public Vector2 Velocity { get; private set; }
        public Vector2 Acceleration { get; private set; }

        public void SetPosition(Vector2 pos)
        {
            Position = pos;
        }

        public void SetVelocity(Vector2 vel)
        {
            Velocity = vel;
        }

        public void Update(GameTime gameTime)
        {
            previousPosition = Position;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += dt * Velocity;
            Velocity += dt * Acceleration;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, Color.White);
        }

        public void Bounce(Vector2 normal)
        {
            Position = previousPosition;

            Vector2 u = Vector2.Dot(Velocity, normal) * normal;
            Vector2 w = Velocity - u;
            Velocity = w - .9f * u;
            
            // float dot = Vector2.Dot(Velocity, normal);
            // Velocity = -normal * dot;
            // Velocity = .5f * (Velocity + (-2 * normal * Vector2.Dot(Velocity, normal)));         
            // SetAcceleration(new Vector2(0,0));
        }
    }
}
