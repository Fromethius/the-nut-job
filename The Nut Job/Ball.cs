using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Nut_Job
{
    class Ball
    {
        int Radius { get; private set; }
        Skin BallSkin { get; private set; }
        int XCenter { get; private set; }
        int YCenter { get; private set; }

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
