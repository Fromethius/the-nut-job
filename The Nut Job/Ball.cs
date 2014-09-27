using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Nut_Job
{
    class Ball
    {
        public int Radius { get; private set; }
        public Skin BallSkin { get; private set; }
        public int XCenter { get; private set; }
        public int YCenter { get; private set; }

        public Ball()
        {
            Radius = 50; //pixels.
            XCenter = 0;
            YCenter = 0;
            BallSkin = Skin.Default;
        }

        public void SetLocation(int x, int y)
        {
            XCenter = x;
            YCenter = y;
        }

        public void SetSkin(Skin skin)
        {
            BallSkin = skin;
        }
    }
}
