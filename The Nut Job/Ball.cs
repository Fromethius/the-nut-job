using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace The_Nut_Job
{
    class Ball
    {
        public Ball()
        {
            Position = new Vector2(50f,50f);
            Velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);
            Radius = 25; //pixels.
            BallSkin = Skin.Default;
        }

        public int Radius { get; private set; }
        public Skin BallSkin { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Acceleration { get; private set; }

        public void SetPosition(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public void SetVelocity(float velX, float velY)
        {
            Velocity = new Vector2(velX, velY);
        }

        public void SetAcceleration(float accX, float accY)
        {
            Acceleration = new Vector2(accX, accY);
        }

        public void SetSkin(Skin skin)
        {
            BallSkin = skin;
        }
    }
}
