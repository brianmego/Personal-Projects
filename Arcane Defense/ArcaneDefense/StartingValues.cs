using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcaneDefense
{
    internal static class StartingValues
    {
 
        // Mana
        internal static int CurrentMana = 100;
        internal static int MaxMana = 100;
        internal static int ManaRegen = 10;

        // Spells
        internal static int GetManaCost(string spellName)
        {
            SortedList<string, int> ManaCost = new SortedList<string, int>();
            ManaCost.Add("Burn", 25);
            ManaCost.Add("Storm", 70);
            ManaCost.Add("DustCloud", 0);
            ManaCost.Add("TurnEnemy", 0);
            ManaCost.Add("Heal", 0);
            ManaCost.Add("ConjureWall", 0);
            ManaCost.Add("SummonWarrior", 0);
            ManaCost.Add("SummonArcher", 0);
            ManaCost.Add("SummonMage", 0);

            return ManaCost[spellName];
        }
            //Burn
            internal static int BurnMinDamage = 5;
            internal static int BurnMaxDamage = 5;

            //Storm
            internal static int StormMinDamage = 10;
            internal static int StormMaxDamage = 10;

        // Enemies
        internal static int EnemyMinSpawnTime = 1; 
        internal static int EnemyMaxSpawnTime = 3;
        internal static int EnemyMinSpeed = 10;
        internal static int EnemyMaxSpeed = 30;
        internal static int MaxEnemies = 3;
        
        //Warrior
        internal static double WarriorAttackSpeed = 2; 
        internal static int WarriorDamage = 3;
        internal static int WarriorMaxHealth = 4;


        // University
        internal static int UniversityMaxHealth = 100;
    }
}
