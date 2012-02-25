using System.Drawing;

namespace ArcaneDefense
{
    class Gauge:GameObject
    {
        internal PointF Velocity;
        internal PointF Location;
        internal SizeF MaxSize;
        internal SizeF CurrentSize;
        private Color insideColor;
        private Color damagedColor;
        private bool vertical;

        private Rectangle barOutline;
        private Rectangle bar;
        private Rectangle damagedBar;

        protected GameState _gameState;

        internal Gauge(GameState gameState, float x, float y, float length, float thickness, Color insideColor, Color damagedColor, bool vertical)
            : base()
        {
            this.vertical = vertical;

            float width;
            float height;
            if (vertical == false)
            {
                height = thickness;
                width = length;
            }
            else
            {
                width = thickness;
                height = length;
            }

            initialize(gameState, x, y, width, height, insideColor, damagedColor);

            gameState.GameObjects.Add(this);
        }

        internal override void Draw(Graphics graphics)
        {
            Graphics g = graphics;

            Pen outline = new Pen(Color.Black,1);
            g.DrawRectangle(outline, barOutline);

            SolidBrush inside = new SolidBrush(this.insideColor);
            g.FillRectangle(inside, bar);

            if (damagedBar.Width > 0)
            {
                SolidBrush damaged = new SolidBrush(this.damagedColor);
                g.FillRectangle(damaged, damagedBar);
            }
        }

        private void initialize(GameState gameState, float x, float y, float width, float height, Color insideColor, Color damagedColor)
        {
            _gameState = gameState;
            Location.X = x;
            Location.Y = y;
            MaxSize.Width = width + 1;
            MaxSize.Height = height + 1;
            CurrentSize.Width = width;
            CurrentSize.Height = height;
            this.insideColor = insideColor;
            this.damagedColor = damagedColor;
        }

        internal override void Update(double gameTime, double elapsedTime)
        {
            //Move the gauge
            Location.X += Velocity.X * (float)elapsedTime;
            Location.Y += Velocity.Y * (float)elapsedTime;
            
            barOutline = new Rectangle((int)Location.X - 1, (int)Location.Y - 1, (int)MaxSize.Width, (int)MaxSize.Height);

            if (vertical == false)
            {
                bar = new Rectangle((int)Location.X, (int)Location.Y, (int)CurrentSize.Width, (int)CurrentSize.Height);
                damagedBar = new Rectangle((int)Location.X + (int)CurrentSize.Width, (int)Location.Y,
                                           (int)MaxSize.Width - (int)CurrentSize.Width - 1, (int)CurrentSize.Height);
            }
            else
            {
                bar = new Rectangle((int)Location.X, (int)Location.Y + ((int)MaxSize.Height - (int)CurrentSize.Height - 1),
                                    (int)CurrentSize.Width, (int)CurrentSize.Height);
                damagedBar = new Rectangle((int)Location.X, (int)Location.Y,
                                           (int)CurrentSize.Width, (int)MaxSize.Height - (int)CurrentSize.Height - 1);
            }
        }

        internal void SetCurrent(int currentHealth, int maxAmount)
        {
            if (vertical == false)
            {
                if (currentHealth != 0)
                {
                    float amountPerTick = (MaxSize.Width - 1) / maxAmount;
                    CurrentSize.Width = (amountPerTick * (float)currentHealth);
                }
                else
                    CurrentSize.Width = 0;
            }
            else
            {
                if (currentHealth != 0)
                {
                    float amountPerTick = (MaxSize.Height - 1) / maxAmount;
                    CurrentSize.Height = (amountPerTick * (float)currentHealth);
                }
                else
                    CurrentSize.Height = 0;
            }
        }
    }
}
