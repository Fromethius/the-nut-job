using Microsoft.Xna.Framework;
using System;

namespace The_Nut_Job
{
    public sealed class Camera
    {
        private Vector2 position = Vector2.Zero;
        private Rectangle? bounds = Rectangle.Empty;
        private Vector2 origin = Vector2.Zero;

        private float zoom = 1.0f;

        private Matrix transformationMatrixCache;

        private bool parameterChanged = true;

        public Camera(Rectangle? bounds, int cameraWidth, int cameraHeight, int screenWidth, int screenHeight)
        {
            Bounds = bounds;
            Origin = new Vector2(cameraWidth / 2, cameraHeight / 2);

            CameraWidth = cameraWidth;
            CameraHeight = cameraHeight;

            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
        }

        /// <summary>
        /// Gets or sets the position of the camera.
        /// The camera's view will be clamped if the position is incompatible with the camera's bounds.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;

                parameterChanged = true;

                ValidatePosition();
            }
        }

        public Rectangle ViewingRegion
        {
            get
            {
                Vector2 positionInScreenCoordinates = UniverseToScreen(position / zoom);

                return new Rectangle((int)(position.X - positionInScreenCoordinates.X), (int)(position.Y - positionInScreenCoordinates.Y), CameraWidth, CameraHeight);
            }
        }

        public Rectangle GetDrawingRegion(int tileSize)
        {
            Rectangle drawingRegion = new Rectangle();

            drawingRegion.X = (int)(ViewingRegion.X / tileSize);
            drawingRegion.Y = (int)(ViewingRegion.Y / tileSize);

            drawingRegion.Width = Math.Min((int)(ViewingRegion.Right / tileSize), Bounds.Value.Right / tileSize) - drawingRegion.X;
            drawingRegion.Height = Math.Min((int)(ViewingRegion.Bottom / tileSize), Bounds.Value.Bottom / tileSize) - drawingRegion.Y;

            return drawingRegion;
        }

        /// <summary>
        /// Gets or sets the camera's bounds.
        /// The camera's view will be clamped if the new bounds are incompatible with the camera's current position.
        /// </summary>
        public Rectangle? Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;

                parameterChanged = true;

                ValidateZoom();
                ValidatePosition();
            }
        }

        /// <summary>
        /// Gets or sets the camera's origin.
        /// By default it is the center of the camera (half of the screen resolution).
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;

                parameterChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the zoom amount of the camera.
        /// Magnification of the universe is positively correlated with zoom value.
        /// </summary>
        public float Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = MathHelper.Max(value, 0);

                parameterChanged = true;

                ValidateZoom();
                ValidatePosition();
            }
        }

        /// <summary>
        /// Gets the width of the camera.
        /// </summary>
        public int CameraWidth { get; private set; }

        /// <summary>
        /// Gets the height of the camera.
        /// </summary>
        public int CameraHeight { get; private set; }

        /// <summary>
        /// Gets the width of the screen.
        /// Typically is the same as the monitor resolution.
        /// </summary>
        public int ScreenWidth { get; private set; }

        /// <summary>
        /// Gets the height of the screen.
        /// Typically is the same as the monitor resolution.
        /// </summary>
        public int ScreenHeight { get; private set; }

        /// <summary>
        /// Pans the camera a specified amount.
        /// </summary>
        public void Pan(Vector2 amount)
        {
            Position += amount;
        }

        /// <summary>
        /// Centers the camera's focus at the specified position.
        /// Will still clamp the camera's position to ensure it maintains its bounds.
        /// </summary>
        public void LookAt(Vector2 newPosition)
        {
            Position = newPosition - Origin;
        }

        /// <summary>
        /// Clamps the camera's position to ensure it is within its specified bounds.
        /// </summary>
        private void ValidatePosition()
        {
            if (bounds.HasValue)
            {
                Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(Transformation));
                Vector2 cameraSize = new Vector2(CameraWidth, CameraHeight) / zoom;
                Vector2 limitWorldMin = new Vector2(bounds.Value.Left, bounds.Value.Top);
                Vector2 limitWorldMax = new Vector2(bounds.Value.Right, bounds.Value.Bottom);
                Vector2 positionOffset = position - cameraWorldMin;
                Vector2 sizeBuffer = new Vector2(CameraWidth - ScreenWidth > 0 ? CameraWidth - ScreenWidth : 0, CameraHeight - ScreenHeight > 0 ? CameraHeight - ScreenHeight : 0);

                position = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize + sizeBuffer) + positionOffset;

                parameterChanged = true;
            }
        }

        private void ValidateZoom()
        {
            if (bounds.HasValue)
            {
                float minZoomX = (float)CameraWidth / bounds.Value.Width;
                float minZoomY = (float)CameraHeight / bounds.Value.Height;

                zoom = MathHelper.Max(zoom, MathHelper.Max(minZoomX, minZoomY));

                parameterChanged = true;
            }
        }

        /// <summary>
        /// Translates universal coordinates to screen coordinates.
        /// Useful transformation for finding out which entities are within the current view.
        /// </summary>
        public Vector2 UniverseToScreen(Vector2 universePosition)
        {
            return Vector2.Transform(universePosition, Transformation);
        }

        /// <summary>
        /// Translates screen coordinates to universal coordinates.
        /// Useful transformation for picking objects on the screen with your mouse.
        /// </summary>
        public Vector2 ScreenToUniverse(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(Transformation));
        }

        /// <summary>
        /// Gets the matrix transformation of the camera using its origin, position, scale, and translation.
        /// </summary>
        public Matrix Transformation
        {
            get
            {
                if (parameterChanged) // Cache
                {
                    Matrix positionMatrix = Matrix.CreateTranslation(-position.X, -position.Y, 0);
                    Matrix originMatrix = Matrix.CreateTranslation(-origin.X, -origin.Y, 0);
                    Matrix scaleMatrix = Matrix.CreateScale(zoom, zoom, 1);
                    Matrix translationMatrix = Matrix.CreateTranslation(origin.X, origin.Y, 0);

                    transformationMatrixCache = positionMatrix * originMatrix * scaleMatrix * translationMatrix;

                    parameterChanged = false;
                }

                return transformationMatrixCache;
            }
        }
    }
}
