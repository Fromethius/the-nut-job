using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Nut_Job
{
    class Ball
    {
        public Ball(Vector2 position)
        {
            Position = new Vector2(position.X, position.Y);
            Velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 500);
            Radius = 25; //pixels.
            BallSkin = Skin.Default;
        }

        public int Radius { get; private set; }
        public Skin BallSkin { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Acceleration { get; private set; }
        public Texture2D Image { get; private set; }

        public void SetInitialPosition(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public void SetVelocity(Vector2 vel)
        {
            Velocity = vel;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity += dt * Acceleration;
            Position += dt * Velocity;
        }

        public void LoadImage(Texture2D image)
        {
            Image = image;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, Color.White);
        }

        public void SetSkin(Skin skin)
        {
            BallSkin = skin;
        }
    }
}
