using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ArcaneDefense
{
    internal abstract class GameObject
    {
        internal abstract void Update(double gameTime, double elapsedTime);
        internal abstract void Draw(Graphics graphics);
    }
}
