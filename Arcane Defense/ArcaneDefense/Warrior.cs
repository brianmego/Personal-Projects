using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ArcaneDefense
{
    class Warrior:Enemy
    {
        private double lastAttack;
        private double attackSpeed = StartingValues.WarriorAttackSpeed;
        private static List<string> image = new List<string>() {@"images\Enemies\Warrior.bmp"};
        //private static List<string> movingImages = new List<string>() { @"images\enemies\warrior1.jpg", @"images\enemies\warrior2.jpg", @"images\enemies\warrior3.jpg"};
        //private List<string> attackImages = new List<string>() { @"images\enemies\warrior4.jpg", @"images\enemies\warrior5.jpg", @"images\enemies\warrior6.jpg" };

        internal Warrior(GameState gameState, float x, float y)
            : base(gameState, x, y, image)
        {
            base.MaxHealth = StartingValues.WarriorMaxHealth;
            this.Health = StartingValues.WarriorMaxHealth;
            this.damage = StartingValues.WarriorDamage;

            //Start off in the moving state
            setMovingState(gameState.gameTime);
        }

        private void attack(double gameTime)
        {

            if (gameTime >= lastAttack + attackSpeed)
            {
                if (_gameState.University.Health - this.damage <= 0)
                    _gameState.University.Health = 0;
                else
                    _gameState.University.Health -= this.damage;

                _gameState.University.Damaged();
                lastAttack = gameTime;
            }
        }        
        
        private void setAttackingState()
        {
            //Update the sprite's frame images
            //LoadFrameList(_gameState, this.Location.X, this.Location.Y, attackImages);
            this.Size.Width = 38;
            this.Size.Height = 38;

            this.speed = 0;
            CalculateVelocity(_gameState, speed);
            this.frameLifetime = attackSpeed/(float)_frames.Count;
            currentState = State.attacking;
        }

        private void setMovingState(double gameTime)
        {
            //Update the sprite's frame images
            //LoadFrameList(_gameState, this.Location.X, this.Location.Y, movingImages);
            this.Size.Width = 38;
            this.Size.Height = 38;

            this.speed = _gameState.randNumberGenerator.Next(StartingValues.EnemyMinSpeed, StartingValues.EnemyMaxSpeed);
            CalculateVelocity(_gameState, speed);
            this.frameLifetime = 5f / speed;
            currentState = State.moving;
        }

        internal override void Update(double gameTime, double elapsedTime)
        {
            base.Update(gameTime, elapsedTime);
            currentFrameTime += elapsedTime;

            if (Collision(this, _gameState.University))
            {
                if (currentState != State.attacking)
                    setAttackingState();
                attack(gameTime);
            }

            updateFrame();


        }

        private void updateFrame()
        {
            if (currentFrameTime > frameLifetime)
            {
                if (CurrentFrame == (_frames.Count - 1))
                    CurrentFrame = 0;
                else
                    CurrentFrame += 1;
                currentFrameTime = 0;
            }
        }

    }
}
