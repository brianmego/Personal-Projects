using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ArcaneDefense
{
    internal class University:Sprite
    {
        private Gauge HealthBar;
        internal int Health;
        private int MaxHealth;

        internal University(GameState gameState, float x, float y, float width, float height)
            : base(gameState, x, y, new List<string>() { @"images\University.jpg"})
        {
            this.Size = new SizeF(width, height);
            initialize();
            gameState.University = this;
        }

        private void initialize()
        {
            HealthBar = new Gauge(_gameState,
                                  this.Location.X + this.Size.Width,
                                  this.Location.Y,
                                  100,
                                  10,
                                  Color.Yellow,
                                  Color.Red, 
                                  true);

            this.MaxHealth = StartingValues.UniversityMaxHealth;
            this.Health = this.MaxHealth;
        }

        internal void Damaged()
        {
            if (Health <= 0)
            {

                if (HealthBar.CurrentSize.Height != 0)
                    HealthBar.SetCurrent(Health, MaxHealth);
            }
            else
            {
                HealthBar.SetCurrent(Health, MaxHealth);
            }
        }
    }
}
