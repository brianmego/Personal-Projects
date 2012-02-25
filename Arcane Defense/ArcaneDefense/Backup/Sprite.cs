using System.Collections.Generic;
using System.Drawing;

namespace ArcaneDefense
{
    internal class Sprite: GameObject
    {
        internal PointF Velocity;
        internal PointF Location;
        internal SizeF Size;
        internal int CurrentFrame;
        
        /// <summary>
        /// The time at which the sprite was spawned
        /// </summary>
        protected double SpawnTime;
        /// <summary>
        /// The length of time the sprite has been on the screen
        /// </summary>
        protected double LifeTime;
        /// <summary>
        /// The time at which the current animation began
        /// </summary>
        protected double currentFrameTime = 0;
        /// <summary>
        /// The amount of time each frame should last
        /// </summary>
        protected double frameLifetime;

        protected List<Bitmap> _frames = new List<Bitmap>();
        protected GameState _gameState;

        internal Sprite(GameState gameState, float x, float y, List<string> images)
        {
            LoadFrameList(gameState, x, y, images);
            
            //Add this Sprite to the list of objects to be drawn each frame
            gameState.GameObjects.Add(this);
        }

        protected void LoadFrameList(GameState gameState, float x, float y, List<string> images)
        {
            _frames.Clear();

            for (int i = 0; i <= (images.Count - 1); i++)
            {
                //Load the bitmap
                _frames.Add(new Bitmap(Image.FromFile(images[i])));
                _frames[i].MakeTransparent();

                //Set the location and use the height and width from the 1st frame
                initialize(gameState, x, y, _frames[0].Width, _frames[0].Height);
            }
        }

        internal static bool Collision(Sprite sprite1, Sprite sprite2)
        {
            //See if the sprite rectangles overlap
            return !(sprite1.Location.X > sprite2.Location.X + sprite2.Size.Width
                    || sprite1.Location.X + sprite1.Size.Width < sprite2.Location.X
                    || sprite1.Location.Y > sprite2.Location.Y + sprite2.Size.Height
                    || sprite1.Location.Y + sprite1.Size.Height < sprite2.Location.Y);
        }

        internal virtual void Destroy()
        {
            //Remove the sprite from objects to be drawn
            _gameState.ObjectsMarkedForDeletion.Add(this);
        }

        internal override void Draw(Graphics graphics)
        {
            //Draw the correct frame at the current point
            graphics.DrawImage(_frames[CurrentFrame], Location.X, Location.Y, Size.Width, Size.Height);
        }

        private void initialize(GameState gameState, float x, float y, float width, float height)
        {
            _gameState = gameState;
            SpawnTime = _gameState.gameTime;
            Location.X = x;
            Location.Y = y;
            Size.Width = width;
            Size.Height = height;
            CurrentFrame = 0;
        }

        internal override void Update(double gameTime, double elapsedTime)
        {
            //Move the sprite
            Location.X += Velocity.X * (float)elapsedTime;
            Location.Y += Velocity.Y * (float)elapsedTime;

            //Update background statistic counters
            LifeTime = _gameState.gameTime - this.SpawnTime;
        }
    }
}
