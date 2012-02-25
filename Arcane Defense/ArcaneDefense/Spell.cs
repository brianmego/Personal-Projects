using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArcaneDefense
{
    internal class Spell
    {
        /// <summary>
        /// Amount of mana it takes to cast the spell
        /// </summary>
        protected int ManaCost;
        internal bool IsSelected = false;
        protected Random randNumberGenerator = new Random();
        protected GameState _gameState;


        internal Spell(GameState gameState)
        {
            _gameState = gameState;
        }

        internal virtual bool Cast(int x, int y)
        {
            return UseMana();
        }

        internal virtual void Deselect()
        {
            IsSelected = false;
        }

        internal int GetManaCost()
        {
            return ManaCost;
        }

        internal virtual void Select()
        {
            IsSelected = true;
        }

        internal bool UseMana()
        {
            if (_gameState.currentMana >= ManaCost)
            {
                _gameState.currentMana -= ManaCost;
                return true;
            }
            return false;
        }

        internal class Burn : Spell
        {
            private int minDamage = StartingValues.BurnMinDamage;
            private int maxDamage = StartingValues.BurnMaxDamage;
            private int initialManaCost = StartingValues.GetManaCost("Burn");

            internal Burn(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                if (UseMana() == true)
                {
                    
                    Enemy enemyToHit = null;

                    foreach (Enemy enemy in _gameState.Enemies)
                    {
                        //Console.WriteLine("Mouse X: " + x.ToString() + " Mouse Y: " + y.ToString());
                        //Console.WriteLine("Enemy X: " + enemy.Location.X.ToString() + " Enemy Y: " + enemy.Location.Y.ToString());

                        if (x >= enemy.Location.X && x < (enemy.Location.X + enemy.Size.Width) &&
                            y >= enemy.Location.Y && y < (enemy.Location.Y + enemy.Size.Height))
                        {
                            enemyToHit = enemy;
                            break;
                        }
                    }

                    if (enemyToHit != null)
                    {
                        int damage = randNumberGenerator.Next(this.minDamage, this.maxDamage);

                        enemyToHit.Health -= damage;
                    }
                    _gameState.numberOfBurns += 1;
                    return true;
                }
                return false;
            }

            private Enemy HitEnemy(int x, int y)
            {
                foreach (Enemy enemy in _gameState.Enemies)
                {
                    if (x >= enemy.Location.X && x < (enemy.Location.X + enemy.Size.Width) &&
                        y >= enemy.Location.Y && y < (enemy.Location.Y + enemy.Size.Height))
                    {
                        return enemy;
                    }
                }

                return null;
            }
        }

        internal class ConjureWall : Spell
        {
            private int initialManaCost = StartingValues.GetManaCost("ConjureWall");
            
            internal ConjureWall(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                if (UseMana() == true)
                {
                    _gameState.numberOfConjureWalls += 1;
                    return true;
                }
                return false;
            }
        }

        internal class DustCloud : Spell
        {
            private int initialManaCost = StartingValues.GetManaCost("DustCloud");

            internal DustCloud(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                _gameState.currentMana = _gameState.maxMana;
 
                if (UseMana() == true)
                {
                    _gameState.numberOfConjureWalls += 1;
                    return true;
                }
                return false;
             }
        }

        internal class Heal : Spell
        {
            private int initialManaCost = StartingValues.GetManaCost("Heal");

            internal Heal(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                if (UseMana() == true)
                {
                    _gameState.numberOfHeals += 1;
                    return true;
                }
                return false;
            }
        }

        internal class IncreaseManaRegen : Spell
        {

            internal IncreaseManaRegen(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = gameState.maxMana;
            }

            internal override bool Cast(int x, int y)
            {
                this.ManaCost = _gameState.maxMana;
                if (base.Cast(x, y) == true)
                {
                    _gameState.manaRegen = (_gameState.manaRegen + (int)System.Math.Round(_gameState.manaRegen * .15, 0));
                    return true;
                }
                return false;
            }
        }

        internal class IncreaseMaxMana : Spell
        {

            internal IncreaseMaxMana(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = gameState.maxMana;
            }

            internal override bool Cast(int x, int y)
            {
                if (base.Cast(x, y) == true)
                {
                    _gameState.maxMana += 20;
                    this.ManaCost = _gameState.maxMana;
                    return true;
                }

                return false;
            }
        }

        internal class Storm : Spell
        {
            private CastingRing castRing;
            private int initialManaCost = StartingValues.GetManaCost("Storm");
            private int minDamage = StartingValues.StormMinDamage;
            private int maxDamage = StartingValues.StormMaxDamage;

            internal Storm(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                if (base.Cast(x, y) == true)
                {
                    foreach (Enemy enemy in _gameState.Enemies)
                    {
                        if (castRing.area.Contains((int)enemy.Location.X, (int)enemy.Location.Y))
                        {
                            int damage = randNumberGenerator.Next(this.minDamage, this.maxDamage);
                            enemy.Health -= damage;

                        }
                    }
                    _gameState.numberOfStorms += 1;
                    return true;
                }
                return false;

            }

            internal override void Deselect()
            {
                base.Deselect();
                if (castRing != null)
                {
                    castRing.Destroy();
                    castRing = null;
                }
            }

            internal override void Select()
            {
                base.Select();
                if (castRing == null)
                {
                    castRing = new CastingRing(_gameState);
                }
            }

        }

        internal class SummonArcher : Spell
        {
            private int initialManaCost = StartingValues.GetManaCost("SummonArcher");

            internal SummonArcher(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                if (UseMana() == true)
                {
                    _gameState.numberOfSummonArchers += 1;
                    return true;
                }
                return false;
            }
        }

        internal class SummonMage : Spell
        {
            private int initialManaCost = StartingValues.GetManaCost("SummonMage");

            internal SummonMage(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                if (UseMana() == true)
                {
                    _gameState.numberOfSummonMages += 1;
                    return true;
                }
                return false;
            }
        }

        internal class SummonWarrior : Spell
        {
            private int initialManaCost = StartingValues.GetManaCost("SummonWarrior");

            internal SummonWarrior(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                if (UseMana() == true)
                {
                    _gameState.numberOfSummonWarriors += 1;
                    return true;
                }
                return false;
            }
        }

        internal class TurnEnemy : Spell
        {
            private int initialManaCost = StartingValues.GetManaCost("TurnEnemy");

            internal TurnEnemy(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                foreach (Enemy enemy in _gameState.Enemies)
                    enemy.Health -= enemy.Health;
                _gameState.numberOfTurnEnemies += 1;
                return true;
            }
        }

        internal class UpgradeSpell : Spell
        {
            private int initialManaCost = 0;

            internal UpgradeSpell(GameState gameState)
                : base(gameState)
            {
                this.ManaCost = initialManaCost;
            }

            internal override bool Cast(int x, int y)
            {
                if (UseMana() == true)
                {
                    _gameState.numberOfUpgradeSpells += 1;
                    return true;
                }
                return false;
            }
        }

    }
}
