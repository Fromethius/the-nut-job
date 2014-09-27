using System;

namespace The_Nut_Job
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TheNutJobGame())
            {
                game.Run();
            }
        }
    }
#endif
}
