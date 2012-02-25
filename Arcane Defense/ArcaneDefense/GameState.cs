using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace ArcaneDefense
{
    internal class GameState
    {
        //Game Objects
            /// <summary>
            /// A list of all the objects being drawn on the screen
            /// </summary>
            internal List<GameObject> GameObjects = new List<GameObject>();
            internal List<GameObject> ObjectsMarkedForDeletion = new List<GameObject>();
            internal List<Enemy> Enemies = new List<Enemy>();
            internal University University;
            private Gauge manaBar;

        //Reported Statistics
        private bool displayStats = false;
        private int reportedFps = 0;
        internal int RoundNumber = 1;
        internal int numberOfVanquishedEnemies;
        internal int numberOfBurns;
        internal int numberOfConjureWalls;
        internal int numberOfDustClouds;
        internal int numberOfHeals;
        internal int numberOfIncreaseManaRegens;
        internal int numberOfIncreaseMaxManas;
        internal int numberOfStorms;
        internal int numberOfSummonArchers;
        internal int numberOfSummonMages;
        internal int numberOfSummonWarriors;
        internal int numberOfTurnEnemies;
        internal int numberOfUpgradeSpells;

        internal double gameTime;
        private int roundTimer = 60;
        internal float currentMana = StartingValues.CurrentMana;
        internal int maxMana = StartingValues.MaxMana;
        internal int manaRegen = StartingValues.ManaRegen;
        internal Spell selectedSpell;
        internal SizeF GameArea = new SizeF(451, 510);
        internal SizeF TotalWindowArea;
        internal Graphics Graphics;
        internal PointF MouseLoc;
        
    
        //Diagnostic Tools
        private double lastFPSReportTime = 0;
        private int dynamicFps = 0;
        private Font _font = new Font("Arial", 10, FontStyle.Bold);
        private Brush _brushWhite = new SolidBrush(Color.White);
        private Brush _brushBlack = new SolidBrush(Color.Black);
        private Brush _brushDarkSeaGreen = new SolidBrush(Color.DarkSeaGreen);
        internal Stopwatch _timer = new Stopwatch();
        private double _lastTime;
        private char[] hotkeys = new char[20] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p',
                                              '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};
        private SpellButton spellDescription; 
        private char statsHotkey = 's';
        private double lastSpawn;
        private int minSpawnTime = StartingValues.EnemyMinSpawnTime;
        private int maxSpawnTime = StartingValues.EnemyMaxSpawnTime;
        internal Random randNumberGenerator = new Random();
        private int randNumber;
        internal Spell[] SpellAssignments = new Spell[20];
        internal SpellButton[] SpellButtons = new SpellButton[20];
        internal Form arcaneDefenseWindow;



        internal GameState(Form window)
        {
            arcaneDefenseWindow = window;
        }
        
        private void CreateHotbar()
        {
            // Assign Images
            string[] spellImages = new string[20];
            string[] spells = new string[20] { "", "", "", "", "", "", "", "", "", "Increase_Mana_Regen", "Burn", "Storm", 
                                               "Dust_Cloud", "Turn_Enemy", "Heal", "Conjure_Wall", "Summon_Warrior", 
                                               "Summon_Archer", "Summon_Mage", "Increase_Max_Mana" };
            string[] spellNames = new string[20] {"Upgrade Burn", "Upgrade Storm", "Upgrade Dust Cloud", "Upgrade Turn Enemy", "Upgrade Heal",
                                                  "Upgrade Conjure Wall", "Upgrade Summon Warrior", "Upgrade Summon Archer", 
                                                  "Upgrade Summon Mage", "Increase Mana Regen", "Burn", "Storm", "Dust Cloud", "Turn Enemy",
                                                  "Heal", "Conjure Wall", "Summon Warrior", "Summon Archer", "Summon Mage", "Increase Max Mana"};
            
            string spellpaths = @"images\Spells\";
            for (byte i = 0; i <= 8; i++)
                spells[i] = "Upgrade_Spell";
            for (byte i = 0; i < spells.Length; i++)
                spellImages[i] = spellpaths + spells[i];

            // Assign Spells
            for (int i = 0; i <= 8; i++)
                SpellAssignments[i] = new Spell.UpgradeSpell(this);

            SpellAssignments[9] = new Spell.IncreaseManaRegen(this);
            SpellAssignments[10] = new Spell.Burn(this);
            SpellAssignments[11] = new Spell.Storm(this);
            SpellAssignments[12] = new Spell.DustCloud(this);
            SpellAssignments[13] = new Spell.TurnEnemy(this);
            SpellAssignments[14] = new Spell.Heal(this);
            SpellAssignments[15] = new Spell.ConjureWall(this);
            SpellAssignments[16] = new Spell.SummonWarrior(this);
            SpellAssignments[17] = new Spell.SummonArcher(this);
            SpellAssignments[18] = new Spell.SummonMage(this);
            SpellAssignments[19] = new Spell.IncreaseMaxMana(this);


            // Create Buttons
            foreach (SpellButton sb in SpellButtons)
                GameObjects.Remove(sb);
            
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    int listSelection = j + ((i - 1) * 9) + (i - 1);
                    SpellButtons[listSelection] =
                    new SpellButton(this, spellNames[listSelection],
                                    ((GameArea.Width / 10) * j)+((TotalWindowArea.Width/2)-(GameArea.Width/2)), 
                                    (GameArea.Height - (GameArea.Width / 10) * i) + ((TotalWindowArea.Height/2)-((GameArea.Height/2))),
                                    new List<string>(){spellImages[listSelection] + ".jpg"}, spellImages[listSelection] + "_Selected.jpg",
                                    hotkeys[listSelection],
                                    SpellAssignments[listSelection]);
                }
            }

            // If a spell cost is more than the current MaxMana, hide that spell's button
            foreach (SpellButton sb in this.SpellButtons)
                if (sb.Spell.GetManaCost() > this.maxMana)
                    sb.Hide();
        }

        internal void DisplayStats(Graphics graphics)
        {
            //Fps
            double elapsedTime = gameTime - lastFPSReportTime;
            dynamicFps += 1;
            if (elapsedTime >= .5)
            {
                lastFPSReportTime = gameTime;
                reportedFps = dynamicFps * 2;
                dynamicFps = 0;
            }

            graphics.DrawString("Frames/Second: " + reportedFps.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, 5));

            //Total Enemies
            graphics.DrawString("Enemies Vanquished: " + numberOfVanquishedEnemies.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, 20));

            //Total Spells cast
            int spellY = 100;
            graphics.DrawString("Spells Cast", _font, _brushWhite, new Point((int)TotalWindowArea.Width - 120, spellY));
            spellY += 20;
            graphics.DrawString("Burns: " + numberOfBurns.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Conjured Walls: " + numberOfConjureWalls.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Dust Clouds: " + numberOfDustClouds.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Heals: " + numberOfHeals.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Increase Mana Regens: " + numberOfIncreaseManaRegens.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Increase Max Manas: " + numberOfIncreaseMaxManas.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Storms: " + numberOfStorms.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Summoned Archers: " + numberOfSummonArchers.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Summoned Mages: " + numberOfSummonMages.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Summoned Warrior: " + numberOfSummonWarriors.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Turned Enemies: " + numberOfTurnEnemies.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));
            spellY += 15;
            graphics.DrawString("Upgraded Spells: " + numberOfUpgradeSpells.ToString(), _font, _brushWhite, new Point((int)TotalWindowArea.Width - 160, spellY));

        }

        internal void Draw(Graphics graphics)
        {
            //Update the graphics variable to allow other classes to use it
            Graphics = graphics;

            //Draw the background of the playing area
            graphics.FillRectangle(_brushDarkSeaGreen, (TotalWindowArea.Width/2)-(GameArea.Width/2), (TotalWindowArea.Height/2)-(GameArea.Height/2), GameArea.Width, GameArea.Height);

            //Draw the game objects
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Draw(graphics);
            }

            // Draw the hotkeys
            DrawHotkeys(graphics);

            // Draw the manabar's numerical indices
                // Current
                graphics.DrawString(((int)(currentMana)).ToString(), _font, _brushBlack, 
                                    new Point((int)manaBar.Location.X - 30, (int)manaBar.Location.Y - 3));
                // Max
                graphics.DrawString(((int)(maxMana)).ToString(), _font, _brushBlack, 
                                    new Point((int)manaBar.Location.X + (int)manaBar.MaxSize.Width + 5, (int)manaBar.Location.Y - 3));

            // Draw the timer
                // Round
                SizeF roundWidth = graphics.MeasureString("Round 1", _font);
                graphics.DrawString("Round " + RoundNumber.ToString(),_font,_brushWhite,
                                    new Point((int)(TotalWindowArea.Width / 2 - roundWidth.Width/2), (int)((TotalWindowArea.Height / 2) - (GameArea.Height / 2))));
                // Time Left
                string timeLeftInRound = "Time left: " + roundTimer.ToString();
                SizeF timerWidth = graphics.MeasureString(timeLeftInRound, _font);
                graphics.DrawString(timeLeftInRound.ToString(), _font, _brushWhite,
                                    new Point((int)((TotalWindowArea.Width / 2) - timerWidth.Width/2), (int)((TotalWindowArea.Height / 2) - (GameArea.Height/2) + (GameArea.Height/30))));

            // If the mouse is over a hotbar button, show that spell's information
            if (spellDescription != null)
            {
                spellDescription.ShowInformation(graphics);
            }

            //Display FPS, Enemies Vanquished, Spells cast, and other statistics
            if (displayStats == true)
            { 
                DisplayStats(graphics); 
            }
        }

        private void DrawHotkeys(Graphics graphics)
        {
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    graphics.DrawString(hotkeys[j + ((i - 1) * 9) + (i - 1)].ToString(),
                                        _font,
                                        _brushWhite,
                                        new Point((int)(((GameArea.Width / 10) * j) + 3) + (int)((TotalWindowArea.Width / 2) - (GameArea.Width / 2)),
                                                  (int)(((GameArea.Height - (GameArea.Width / 10) * i) + 3) + (int)((TotalWindowArea.Height / 2) - (GameArea.Height / 2)))));
                }
            }
        }

        internal void MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // If the mouse is clicked in the main playing area cast the selected spell
            
            if (e.X > (TotalWindowArea.Width/2 - GameArea.Width/2) && 
                e.X < (TotalWindowArea.Width/2 + GameArea.Width/2) &&
                e.Y < (TotalWindowArea.Height/2 + GameArea.Height/2) - (GameArea.Width / 10) * 2 && 
                e.Y > (TotalWindowArea.Height/2 - GameArea.Height/2))
            {
                if (selectedSpell != null)
                {
                    this.selectedSpell.Cast(e.X, e.Y);
                    RedrawHotBar();
                }
            }
            else
            {
                // Highlight the button that is clicked on
                foreach (SpellButton sb in this.SpellButtons)
                    if (e.X >= sb.Location.X && e.X < (sb.Location.X + sb.Size.Width)
                        && e.Y >= sb.Location.Y && e.Y < (sb.Location.Y + sb.Size.Height))
                    {
                        if (sb.Spell.GetManaCost() <= this.maxMana)
                            sb.Select();

                        RedrawHotBar();
                        break;
                    }
            }   
        }

        internal void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // If the mouse is moved over a hotbar button, mark that spell's information to be shown
            foreach (SpellButton sb in this.SpellButtons)
            {
                if (sb != null)
                {
                    if (e.X >= sb.Location.X && e.X < (sb.Location.X + sb.Size.Width)
                        && e.Y >= sb.Location.Y && e.Y < (sb.Location.Y + sb.Size.Height))
                    {
                        spellDescription = sb;
                        break;
                    }
                    else
                        spellDescription = null;
                }
            }

            this.MouseLoc = new PointF(e.X, e.Y);
        }

        internal void KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            foreach (SpellButton sb in this.SpellButtons)
                if (e.KeyChar == sb.Hotkey)
                {
                    if (sb.Spell.GetManaCost() <= this.maxMana)
                    {
                        sb.Select();
                        RedrawHotBar();
                    }
                    break;
                }
            if (e.KeyChar == 's')
            {
                if (displayStats == false)
                    displayStats = true;
                else
                    displayStats = false;
            }
        }

        internal void Initialize()
        {
            //Initialize and start the timer
            _lastTime = 0.0;
            _timer.Reset();
            _timer.Start();

            //Generate the first random number used to spawn enemies
            randNumber = randNumberGenerator.Next(this.minSpawnTime, this.maxSpawnTime);

            //Create all the main gameobjects
            GameObjects.Clear();

            //Create Arcane University
            float universitySize = 150;
            University university = new University(this, TotalWindowArea.Width/2 - universitySize/2, 
                                                   TotalWindowArea.Height/2 + TotalWindowArea.Height/50, 
                                                   universitySize, 
                                                   universitySize);
            
            //Create Mana Bar
            float manabarSize = universitySize;
            manaBar = new Gauge(this, TotalWindowArea.Width / 2 - manabarSize/2, 
                                University.Location.Y + (University.Size.Height/1.3f), 
                                manabarSize, 10, Color.Blue, Color.Transparent, false);

        }

        internal void RedrawHotBar()
        {
            foreach (SpellButton sb in this.SpellButtons)
                if (this.selectedSpell != sb.Spell)
                {
                    if (sb.Spell.GetManaCost() > this.maxMana)
                        sb.Hide();
                    else
                        sb.Deselect();
                }
        }

        internal void Resize()
        {
            TotalWindowArea = arcaneDefenseWindow.ClientSize;
            Initialize();

            CreateHotbar();
        }

        private void SpawnEnemies(double gameTime)
        {
            if ((gameTime > this.lastSpawn + randNumber) && (Enemies.Count < StartingValues.MaxEnemies))
            {
                float x;
                float y;

                int randSpawn = randNumberGenerator.Next(1, 5);

                // Spawn on the left
                if (randSpawn == 1)
                {
                    x = (TotalWindowArea.Width/2 - GameArea.Width/2);
                    y = randNumberGenerator.Next((int)(TotalWindowArea.Height / 2 - GameArea.Height / 2), (int)(TotalWindowArea.Height / 2));
                }

                // Spawn on the right
                else if (randSpawn == 2)
                {
                    x = (TotalWindowArea.Width/2 + GameArea.Width/2);
                    y = randNumberGenerator.Next((int)(TotalWindowArea.Height / 2 - GameArea.Height / 2), (int)(TotalWindowArea.Height / 2));
                        
                }

                // Spawn on the top
                else
                {
                    x = randNumberGenerator.Next((int)(TotalWindowArea.Width / 2 - GameArea.Width / 2), (int)(TotalWindowArea.Width / 2 + GameArea.Width / 2));
                    y = (TotalWindowArea.Height / 2) - (GameArea.Height / 2);
                }

                new Warrior(this, x, y);
                lastSpawn = gameTime;
                this.randNumber = this.randNumberGenerator.Next(this.minSpawnTime, this.maxSpawnTime);
            }
        }

        internal void Update()
        {
            //Work out how long since we were last here in seconds
            gameTime = _timer.ElapsedMilliseconds / 1000.0;
            double elapsedTime = gameTime - _lastTime;
            _lastTime = gameTime;

            //Updates all the game objects
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update(gameTime, elapsedTime);
            }

            //Deletes objects marked for deletion
            foreach (GameObject gameObject in ObjectsMarkedForDeletion)
            {
                GameObjects.Remove(gameObject);
            }

            //Update Mana Bar
            if (elapsedTime != 0)
            {
                if (currentMana + (float)(manaRegen * elapsedTime) <= maxMana)
                    currentMana += (float)(manaRegen * elapsedTime);
                else
                    currentMana = maxMana;
            }

            manaBar.SetCurrent((int)currentMana, (int)maxMana);

            //Increase Round Number
            if (60 - (int)gameTime % 60 != roundTimer)
                roundTimer -= 1;
            if (roundTimer == 0)
            {
                roundTimer = 60;
                RoundNumber += 1;
            }

            //Check to see if an enemy should be spawned
            SpawnEnemies(gameTime);
        }
    }
}
