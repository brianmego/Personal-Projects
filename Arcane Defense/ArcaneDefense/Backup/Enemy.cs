using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArcaneDefense
{
    internal class Enemy: Sprite
    {
        internal int MaxHealth;
        internal int Health;
        protected int speed;
        private Gauge HealthBar;
        protected int damage;
        protected State currentState;

        internal Enemy(GameState gameState, float x, float y, List<string> images) :
            base(gameState, x, y, images)
        {
            initialize(gameState);

            gameState.Enemies.Add(this);
        }

        private void initialize(GameState gameState)
        {
            speed = gameState.randNumberGenerator.Next(StartingValues.EnemyMinSpeed, StartingValues.EnemyMaxSpeed);
            CalculateVelocity(gameState, this.speed);
            this.HealthBar = new Gauge(gameState,
                                       this.Location.X,
                                       this.Location.Y + 38 + 3,
                                       38,
                                       38/ 6,
                                       Color.Yellow,
                                       Color.Red,
                                       false);
            this.HealthBar.Velocity = this.Velocity;
        }

        protected void CalculateVelocity(GameState gameState, int speed)
        {
            PointF univLoc = _gameState.University.Location;
            univLoc.X += _gameState.University.Size.Width / 2;
            float xDistance = univLoc.X - this.Location.X;
            float yDistance = univLoc.Y - this.Location.Y;
            float hypotenuseDistance = (float)Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));

            this.Velocity = new PointF(((univLoc.X - this.Size.Width/2 - this.Location.X) / hypotenuseDistance) * speed,
                                       ((univLoc.Y - this.Size.Height/2 - this.Location.Y) / hypotenuseDistance) * speed);
        }

        internal override void Destroy()
        {
            base.Destroy();
            _gameState.ObjectsMarkedForDeletion.Add(this);
            _gameState.ObjectsMarkedForDeletion.Add(this.HealthBar);
            _gameState.Enemies.Remove(this);
            _gameState.numberOfVanquishedEnemies += 1;
        }

        internal override void Draw(Graphics graphics)
        {
            //Draw the correct frame at the current point
            graphics.DrawImage(_frames[CurrentFrame], Location.X, Location.Y, Size.Width, Size.Height);

        }

        protected enum State
        {
            moving,
            attacking
        }

        internal override void Update(double gameTime, double elapsedTime)
        {
            this.HealthBar.Velocity = this.Velocity;

            if (Health <= 0)
                this.Destroy();
            else
            {
                HealthBar.SetCurrent(Health, MaxHealth);
            }

            base.Update(gameTime, elapsedTime);
        }
    }
}
