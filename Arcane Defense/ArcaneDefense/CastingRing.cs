using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArcaneDefense
{   
    class CastingRing:Sprite
    {
        private static List<string> images = new List<string>() { @"images\spells\CastRing.png" };
        internal Rectangle area;

        internal CastingRing(GameState gameState)
            : base(gameState, gameState.MouseLoc.X, gameState.MouseLoc.Y, images)
        {
            Cursor.Hide();
        }

        internal override void Destroy()
        {
            base.Destroy();
            Cursor.Show();
        }

        internal override void Draw(System.Drawing.Graphics graphics)
        {
            base.Draw(graphics);
        }
        internal override void Update(double gameTime, double elapsedTime)
        {
            base.Update(gameTime, elapsedTime);
            this.Location.X = (_gameState.MouseLoc.X - this.Size.Width/2);
            this.Location.Y = (_gameState.MouseLoc.Y - this.Size.Height / 2);
            area = new Rectangle((int)this.Location.X, (int)this.Location.Y, (int)this.Size.Width, (int)this.Size.Height);
        }
    }
}
