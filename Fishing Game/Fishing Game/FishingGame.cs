using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fishing_Game
{
    public class Game
    {
        //Field
        static Inventory inv = new Inventory();
        static FishingRod current_rod;
        static Bait current_bait;
        static Pond pond = new Pond();

        public static void Main()
        {
            
            int day_of_competition = 1;
            
            Console.WriteLine("Welcome to the Travis County Fishing Competition! You have 31 days to catch a \n" +
                              "150 lbs Carp.");
            Console.ReadLine();
            Console.WriteLine("Upgrading your rod will allow you to hold on to bigger catches, while upgrading your bait will " +
                              "allow you to catch bigger and better fish.");
            Console.ReadLine();
            Console.WriteLine("At the end of each day, you can sell your catches to earn more money. \n" +
                              "To begin press Enter");
            Console.ReadLine();
                

            bool loop = true;
            while (loop == true)
            {
                //Opening text each turn
                Console.WriteLine("\n\n\n\n\nWelcome to day {0} of the Travis County Fishing Competition!\n", 
                                    day_of_competition);

                //Choose a Rod
                current_rod = inv.DisplayRods();

                //Choose a Bait
                current_bait = inv.DisplayBait();
                
                //Begin the day's fishing and return the day's catches
                inv.catch_of_the_day = pond.Cast(current_rod, current_bait);

                //Display the day's catches and sell them
                inv.sell_fish();

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();

                //Check for win
                if (inv.goal_reached == true)
                {
                    Console.WriteLine("You win! You caught a 150 lbs carp in " + day_of_competition + " days!" +
                                      "\n\nPlease press Enter to exit");
                    Console.ReadLine();
                    break;
                }
                
                //Ending Sequence
                if (day_of_competition >= 31)
                {
                    Console.WriteLine("Sometimes the fish just aren't biting...Maybe next month!\n\nPlease press Enter to exit");
                    Console.ReadLine();
                    loop = false;
                }
                else
                    day_of_competition ++;
            }

        }

    }

    public class Inventory
    {
        public List<FishingRod>rods = new List<FishingRod>();
        public List<Bait> current_bait = new List<Bait>();
        public List<Fish> catch_of_the_day = new List<Fish>();
        public double wallet;
        public bool goal_reached;


        //Fills the inventory with potential rods and baits
        public Inventory()
        {
            rods.Add(new FishingRod("Stick", 0, 5, true));
            rods.Add(new FishingRod("Light Fiberglass", 10, 25, false));
            rods.Add(new FishingRod("Heavy Fiberglass", 75, 50, false));
            rods.Add(new FishingRod("Light Graphite", 200, 75, false));
            rods.Add(new FishingRod("Heavy Graphite", 600, 200, false));
            
            current_bait.Add(new Bait("Gum",           0, 0, 1, 1));
            current_bait.Add(new Bait("Worm",          1, 1, 1, 2));
            current_bait.Add(new Bait("Night Crawler", 5, 1, 1, 5));
            current_bait.Add(new Bait("Shrimp",       10, 2, 2, 9));
            current_bait.Add(new Bait("Minnow",       20, 2, 3, 10));
            
            wallet = 0;
        }

        public FishingRod DisplayRods()
        {
            bool loop = true;
            while (loop == true)
            {
                byte counter = 1;
                Console.WriteLine("   " + "Rod" + "\t\t\t" + " Cost" + "\t" + "     Max Poundage\n");
                foreach (FishingRod rod in rods)
                {
                    StringBuilder amount_of_space = new StringBuilder(" ");
                    amount_of_space.Length = (20 - rod.name.Length) + 4;
                    string printout = counter + " " + rod.name + amount_of_space + "$" + rod.cost + "\t\t";
                    if (rod.name == "Heavy Graphite")
                        printout += "NO MAX";
                    else
                        printout += rod.max_poundage + " lbs";

                    Console.WriteLine(printout);
                    counter++;
                }
                Console.WriteLine("\n" + "Choose a Rod: " + "\t\t\t     Wallet: $" + Math.Round(wallet, 2));

                string rodchoice = Console.ReadLine();
                string[] possiblechoices = { "1", "2", "3", "4", "5" };


                foreach (string choice in possiblechoices)
                {
                    if (rodchoice == choice)
                    {
                        Byte bytechoice = Convert.ToByte(choice);
                        foreach (FishingRod rod in rods) rod.current = false;

                        FishingRod chosenrod = rods[bytechoice - 1];
                            if (chosenrod.cost <= wallet)
                            {
                                chosenrod.current = true;
                                wallet -= chosenrod.cost;
                                chosenrod.cost = 0;
                                return chosenrod;  //Return the chosen rod  
                            } 
                        
                    }
                }
            }

        return rods[0];
        }

        public Bait DisplayBait()
        {

            bool loop = true;
            while (loop == true)
            {
                byte counter = 1;
                Console.WriteLine("\n\n   " + "Bait" + "\t\t\t" + " Cost\n");
                foreach (Bait bait in current_bait)
                {
                    StringBuilder amount_of_space = new StringBuilder(" ");
                    amount_of_space.Length = (20 - bait.name.Length) + 4;
                    Console.WriteLine(counter + " " + bait.name + amount_of_space + "$" + bait.cost);
                    counter++;
                }
                Console.WriteLine("\n" + "Choose Today's Bait:" + "\t\t     Wallet: $" + Math.Round(wallet, 2));

                string baitchoice = Console.ReadLine();
                string[] possiblechoices = { "1", "2", "3", "4", "5" };

                foreach (string choice in possiblechoices)
                {
                    if (baitchoice == choice)
                    {
                        Byte bytechoice = Convert.ToByte(choice);
                        foreach (Bait bait in current_bait)
                        {
                            bait.current = false;
                        }
                        Bait chosenbait = current_bait[bytechoice - 1];

                        if (chosenbait.cost <= wallet)
                        {
                            chosenbait.current = true;
                            wallet -= chosenbait.cost;
                            return chosenbait; // Return the chosen bait

                        }
                    }
                }
            }
        return current_bait[0];
        }

        public void sell_fish()
        {
            Console.WriteLine("\n\nToday's Haul: \n");
            double total_money = 0;
            string printout;

            foreach (Fish fish in catch_of_the_day)
            {
                printout = "\t" + fish.name + ":\t";
                if (fish.name != "Catfish") if (fish.name != "Sunfish")
                    printout += "\t";
                printout += Math.Round(fish.weight, 2) + " lbs\t\t";
                printout += "$" + Math.Round((fish.price_per_pound * fish.weight), 2);
                Console.WriteLine(printout);
                total_money += (fish.price_per_pound * fish.weight);

                if (fish.name == "Carp")
                    if (fish.weight >= 150)
                        goal_reached = true;
            }

            wallet += total_money;
            printout = "\n\t\tTotal: $" + Math.Round(total_money, 2) + "\t\tWallet: $" + Math.Round(wallet, 2);
            Console.WriteLine(printout);
        }
    }
    
    public class FishingRod
    {
        //Field
        public string name;
        public int cost;
        public int max_poundage;
        public bool acquired;
        public bool current = false;
        
        //Constructor
        public FishingRod(string name, int cost, int max_poundage, bool acquired)
        {
            this.name = name;
            this.cost = cost;
            this.max_poundage = max_poundage;
            this.acquired = acquired;
        }


    }

    public class Bait
    {
        public string name;
        public int cost;
        public int catch_chance_increase;
        public int min_roll;
        public int max_roll;
        public bool current;

        //Constructor
        public Bait(string name, int cost, int catch_chance_increase, int min_roll, int max_roll)
        {
            this.name = name;
            this.cost = cost;
            this.catch_chance_increase = catch_chance_increase;
            this.min_roll = min_roll;
            this.max_roll = max_roll;
        }

    }

    public class Fish
    {
        public string name;
        public int min_catch_roll;
        public int max_catch_roll;
        public double min_weight;
        public double max_weight;
        public double weight;
        public double price_per_pound;

        public Fish(string name, int min_catch_roll, int max_catch_roll, double min_weight, 
                    double max_weight, double price_per_pound)
        {
            this.name = name;
            this.min_catch_roll = min_catch_roll;
            this.max_catch_roll = max_catch_roll;
            this.min_weight = min_weight;
            this.max_weight = max_weight;
            this.price_per_pound = price_per_pound;
        }
    }

    public class Pond
    {
        public List<Fish> pondfish = new List<Fish>();
        public List<Fish> caughtfish = new List<Fish>();
        Random RandomInt = new Random();
        Random RandomDouble = new Random();

        //Fills the pond with types of Fish
        public Pond()
        {
            pondfish.Add(new Fish("Sunfish",    1,      1,      .3,     5,  1));
            pondfish.Add(new Fish("Trout",      2,      3,       3,    20,  1.2));
            pondfish.Add(new Fish("Catfish",    4,      6,       5,    50,  1.4));
            pondfish.Add(new Fish("Bass",       7,      9,      20,    90,  1.6));
            pondfish.Add(new Fish("Carp",       10,    10,      80,   200,  1.8));
        }
        
        //Determines what the player catches based on what the current rod and bait is this round
        public List<Fish> Cast(FishingRod current_rod, Bait current_bait)
        {
            caughtfish.Clear();
            string caught = "";
            double weight = 0;
            for (byte i = 1; i <= 20; i++)
            {
                int randomcatch = RandomInt.Next(1, 20 + 1);        //Check if player catches a fish at all
                if (randomcatch + current_bait.catch_chance_increase >= 20)
                {
                    int whichfish = RandomInt.Next(current_bait.min_roll, current_bait.max_roll + 1);  //Determines the kind of fish caught
                    foreach (Fish fish in pondfish)
                    {
                        if (whichfish >= fish.min_catch_roll)
                        {
                            if (whichfish <= fish.max_catch_roll)
                            {
                                Fish landedfish = new Fish(fish.name, fish.min_catch_roll, 
                                                           fish.max_catch_roll, fish.min_weight, 
                                                           fish.max_weight, fish.price_per_pound);
                                landedfish.weight = landedfish.min_weight + RandomDouble.NextDouble() * 
                                                   (landedfish.max_weight - landedfish.min_weight);
                                if (landedfish.weight >= current_rod.max_poundage)
                                {
                                    caught = "Your rod is too small, the line broke!";
                                    weight = 0;
                                }
                                else
                                {
                                    caughtfish.Add(landedfish);
                                    caught = landedfish.name;
                                    weight = landedfish.weight;
                                }
                            }
                        }
                    }
                    
                }
                else
                {
                    caught = "No Bite";
                    weight = 0;
                    
                }

                string printout = ("Chance " + i + "..." + caught);
                Thread.Sleep(120);
                if (weight != 0)
                    printout += (", " + Math.Round(weight, 2) + " lbs");
                Console.WriteLine(printout);

            }
            return caughtfish;
        }
    }
}
