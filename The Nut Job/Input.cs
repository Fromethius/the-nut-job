using Microsoft.Xna.Framework.Input;

namespace The_Nut_Job
{
    static class Input
    {
        private static MouseState previousMouseState, currentMouseState;

        public static void Update()
        {
            previousMouseState = currentMouseState;

            currentMouseState = Mouse.GetState();
        }

        public static int MouseX { get { return currentMouseState.Position.X; } }
        public static int MouseY { get { return currentMouseState.Position.Y; } }

        public static bool IsMouseLeftClick()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }

        public static bool IsMouseRightClick()
        {
            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }

        public static bool IsMouseLeftDown()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
    }
}
