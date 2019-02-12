using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace project1
{
    class Program
    {
        //Creating shortcut for Json file path
        private const string FilePath = @"C:\Users\matjara\Documents\CWEB2010\Project_One_JAM\project1\players.JSON";

        static void Main(string[] args)
        {
            //Global variables
            int row, column;
            int NumOfPicks = 0;
            double moneyAvailable = 95000000;
            string effectiveCost = "You have made Cost Effective draft choices, Congratulations Coach!.\n";
            bool effectiveDraft = false;


            //Taking data from the "Players" Json file
            Players[,] playerData = JsonConvert.DeserializeObject<Players[,]>(File.ReadAllText(FilePath));

            //Array for positions of players
            string[] position = new string[playerData.GetLength(0)];
            for (int n = 0; n < playerData.GetLength(0); n++)
            {
                for (int i = 0; i < playerData.GetLength(1); i++)
                {
                    position[n] = playerData[n, i].Position;
                }
            }

            List<int> rankPick = new List<int>();
            
            //Greeting message with dollar amount and key presses
            Greeting(moneyAvailable);
            KeyCapture(out ConsoleKey sentinel, ref NumOfPicks);
            //Main while loop invoking methods when user did not press Escape
            while (sentinel != ConsoleKey.Escape)
            {
                OutputGrid(playerData, position);
                row = GetRow(position, moneyAvailable);
                CheckRow(ref row);
                OutputPositionGrid(playerData, row, position);
                column = GetColumn(ref rankPick, playerData, row, position, moneyAvailable);
                CheckColumn(ref column);
                AccumPrice(playerData, ref moneyAvailable, row, column, ref NumOfPicks);
                CostEffective(ref rankPick, ref NumOfPicks, ref moneyAvailable, ref effectiveDraft, effectiveCost);
                KeyCapture(out sentinel, ref NumOfPicks);
            }
            //Invoking the closing message output after user pressed X or all 5 picks are used
            OutputPrice(playerData, NumOfPicks, moneyAvailable, ref effectiveDraft, effectiveCost, position);
            Console.WriteLine("Press any key to exit program.");
            Console.ReadKey();
        } //End of main
        static void Greeting(double moneyAvailable)//Greeting message to user
        {
            Console.Write("Welcome Coach, to the 2019 NFL Draft!\nYou will begin the draft with, ");
            ColoredConsoleWrite(ConsoleColor.Green, $"{moneyAvailable.ToString("c")}");
            Console.Write("!\nYou will only have 5 draft picks.");
            Console.WriteLine("\n\nYou can not the same player more than once.\nYou will also not be able to continue drafting players when you have insuffucent funds.");
        }
        //Capture Key Method
        static void KeyCapture(out ConsoleKey key, ref int NumOfPicks)
        {
            Console.WriteLine("To draft a player, press any key.\nIf you would like to end the draft, press Escape.");
            key = Console.ReadKey().Key;
            if (NumOfPicks == 5)//If statement to catch if user is out of picks
            {
                Console.Clear();
                Console.WriteLine("You have no draft picks remaining.\nThe draft is now ending. Please press any key to continue.");
                Console.ReadKey();
                key = ConsoleKey.Escape;//Ends the while loop
            }
        }
        //Output Grid for the user to see all the player data
       
        static void OutputPositionGrid(Players[,] players, int row, string[] pos)
        {
            Console.Clear();
            CenterConsoleWrite(ConsoleColor.White, "");
            for (int i = 0; i < players.GetLength(1); i++)
            {
                CenterConsoleWrite(ConsoleColor.White, $"{players[row, i].Rank}");//Outputting rank names to top of the table
            }
            Console.WriteLine("\n");
            Console.Write($"{pos[row]}".PadRight(20));// Outputting the position based on the row the user selected
            for (int b = 0; b < 3; b++)
            {
                //For loop writing names for each position from array data
                for (int x = 0; x < players.GetLength(1); x++)
                {
                    if (players[row, x].draftedPlayer == false)
                    {
                        if (b == 0)
                        {
                            CenterConsoleWrite(ConsoleColor.White, $"{players[row, x].Name}");
                        }
                        else if (b == 1)
                        {
                            CenterConsoleWrite(ConsoleColor.White, $"{players[row, x].School}");
                        }
                        else if (b == 2)
                        {
                            CenterConsoleWrite(ConsoleColor.Green, $"{players[row, x].Salary.ToString("c")}");
                        }
                    }
                    else
                    {
                        if (b == 0)
                        {
                            CenterConsoleWrite(ConsoleColor.DarkRed, $"{players[row, x].Name}");
                        }
                        else if (b == 1)
                        {
                            CenterConsoleWrite(ConsoleColor.DarkRed, $"{players[row, x].School}");
                        }
                        else if (b == 2)
                        {
                            CenterConsoleWrite(ConsoleColor.DarkRed, $"{players[row, x].Salary.ToString("c")}");
                        }
                    }
                }
                Console.WriteLine("");
                CenterConsoleWrite(ConsoleColor.White, "");
            }
            Console.WriteLine("");
        }
        static void OutputGrid(Players[,] players, string[] pos)
        {
            Console.Clear();
            Console.Write("Position".PadRight(20));
            for (int i = 0; i < players.GetLength(1); i++)
            {
                CenterConsoleWrite(ConsoleColor.White, $"{players[i, i].Rank}");
            }
            Console.WriteLine("\n");
            for (int i = 0; i < players.GetLength(0); i++)
            {
                Console.Write($"{pos[i]}".PadRight(20));
                for (int b = 0; b < 3; b++)
                {
                    //For loop writing names for each position from array data
                    for (int x = 0; x < players.GetLength(1); x++)
                    {
                        if (players[i, x].draftedPlayer == false)
                        {
                            if (b == 0)
                            {
                                CenterConsoleWrite(ConsoleColor.White, $"{players[i, x].Name}");
                            }
                            else if (b == 1)
                            {
                                CenterConsoleWrite(ConsoleColor.White, $"{players[i, x].School}");
                            }
                            else if (b == 2)
                            {
                                CenterConsoleWrite(ConsoleColor.Green, $"{players[i, x].Salary.ToString("c")}");
                            }
                        }
                        else
                        {
                            if (b == 0)
                            {
                                CenterConsoleWrite(ConsoleColor.DarkRed, $"{players[i, x].Name}");
                            }
                            else if (b == 1)
                            {
                                CenterConsoleWrite(ConsoleColor.DarkRed, $"{players[i, x].School}");
                            }
                            else if (b == 2)
                            {
                                CenterConsoleWrite(ConsoleColor.DarkRed, $"{players[i, x].Salary.ToString("c")}");
                            }
                        }
                    }
                    Console.WriteLine("");
                    CenterConsoleWrite(ConsoleColor.White, "");
                }
                Console.WriteLine("");
            }
        }
        //Creating options for user to select 
        static int GetRow(string[] pos, double cost)
        {
            //Remaining Money
            Console.Write($"You have ");
            ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{ cost.ToString("c")}");
            Console.Write(" remaining.\n\n");
            int row;
            Console.WriteLine("Please select the position of the player you would like to draft.\nThen press enter.\n");
            for (int i = 0; i < pos.Length; i++)
            {
                Console.WriteLine($"  {i + 1}.) {pos[i]}");
            }
            try//Error catching
            {
                row = Convert.ToInt32(Console.ReadLine());
                return row = row - 1;
            }
            catch
            {
                return row = -1;
            }
        }
        //Taking input for the rank of player the Coach picks
        static int GetColumn(ref List<int> rankPick, Players[,] players, int row, string[] pos, double price)
        {
            int column;
            Console.Write("You have ");
            ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{ price.ToString("c")}");
            Console.Write(" remaining.\n\n");
            Console.WriteLine($"You have selected:\n{row + 1}.) {pos[row]}\n");
            Console.WriteLine("Please enter the rank of the player you would like to draft.\nThen press enter.\n");
            //This loop is allowing the coach to see options for the selection of the rank of the player
            for (int i = 0; i < players.GetLength(1); i++)
            {
                Console.WriteLine($"  {i + 1}.) {players[i, i].Rank}");
            }
            try
            {
                column = Convert.ToInt32(Console.ReadLine());
                rankPick.Add(column);
                return column = column - 1;
            }
            catch
            {
                return column = -1;
            }
        }
        //Limits the coach from entering anything outside of the range of numbers displayed
        static void CheckRow(ref int row)
        {
            while ((row < 0) || (row > 7))
            {
                try
                {
                    Console.WriteLine("Invalid option, please enter a number between 1 and 8.");
                    row = (Convert.ToInt32(Console.ReadLine()) - 1);
                }
                catch
                {
                    row = -1;
                }
            }
        }
        static void CheckColumn(ref int column)
        {
            while ((column < 0) || (column > 4))
            {
                try
                {
                    Console.WriteLine("Invalid option, please enter a number between 1 and 5.");
                    column = (Convert.ToInt32(Console.ReadLine()) - 1);
                }
                catch
                {
                    column = -1;
                }
            }
        }
        //Calculates if player has been picked, cost after player has been picked, and if they have enough to select the player
        static void AccumPrice(Players[,] players, ref double accum, int row, int column, ref int NumOfPicks)
        {
            if (players[row, column].draftedPlayer == false)
            {
                if (accum >= players[row, column].Salary)
                {
                    Console.Clear();
                    accum -= players[row, column].Salary;
                    Console.Write($"You have selected {players[row, column].Name} from {players[row, column].School} for ");
                    ColoredConsoleWrite(ConsoleColor.Green, $"{players[row, column].Salary.ToString("c")}");
                    Console.Write("!\nYou have ");
                    ColoredConsoleWrite(ConsoleColor.Green, $"{ accum.ToString("c")} ");
                    Console.WriteLine("remaining.\n");
                    players[row, column].draftedPlayer = true;
                    NumOfPicks++;
                }
                else//If the user can not afford the player
                {
                    Console.Clear();
                    ColoredConsoleWrite(ConsoleColor.DarkRed, "Insufficient Funds, you have ");
                    ColoredConsoleWrite(ConsoleColor.Green, $"{accum.ToString("c")}");
                    ColoredConsoleWrite(ConsoleColor.DarkRed, " remaining. Please select a player within your price range.\n");
                    return;
                }
            }
            else//If the player has already been picked
            {
                ColoredConsoleWrite(ConsoleColor.DarkRed, "This player has already been picked, choose a different player.\n");
                return;
            }
        }
        //Method to determine if the user has had a cost effective draft
        static void CostEffective(ref List<int> rankPick, ref int pick, ref double accum, ref bool effectiveDraft, string effectiveCost)
        {
            if (pick == 3)
            {
               
                rankPick.Sort();
                rankPick.Reverse();
                for (int i = 0; i < rankPick.Count; i++)
                {
                    if (rankPick[i] > 3) 
                    {
                        break;
                    }
                    else
                    {
                        if (accum > 30000000)
                        {
                            ColoredConsoleWrite(ConsoleColor.DarkYellow, effectiveCost);
                            effectiveDraft = true;
                            break;
                        }
                    }
                }
            }
        }
        // Below outputs final message to the user
        static void OutputPrice(Players[,] players, int NumOfPicks, double moneyAvailable, ref bool effectiveDraft, string effectiveCost, string[] position)
        {
            Console.Clear();
            if (NumOfPicks == 0)
            {
                Console.WriteLine("Please come back when you are ready to draft!");
            }
            else
            {
                if (effectiveDraft == true)//Outputs message to the Coach if they had an effective draft
                {
                    Console.Write("Congratulations, ");
                    ColoredConsoleWrite(ConsoleColor.DarkYellow, $"{effectiveCost}");
                }
                else
                {
                    Console.WriteLine("Congratulations Coach you have completed your draft!");
                }
                //Output message telling the Coach their remaining amount
                Console.Write("Based on your selections, you have ");
                ColoredConsoleWrite(ConsoleColor.DarkGreen, $"{moneyAvailable.ToString("c")} ");
                Console.WriteLine("remianing for, free agents and bonuses.\nYou have drafted:\n");
                for (int a = 0; a < 5; a++)
                {
                    //Formatting the output to in the final message of who the user drafted
                    for (int i = 0; i < players.GetLength(0); i++)
                    {
                        for (int x = 0; x < players.GetLength(1); x++)
                        {
                            if (players[i, x].draftedPlayer == true)
                            {
                                if (a == 0)
                                {
                                    CenterConsoleWrite(ConsoleColor.White, $"{players[i, x].Rank}");
                                }
                                else if (a == 1)
                                {
                                    CenterConsoleWrite(ConsoleColor.White, $"{position[i]}");
                                }
                                else if (a == 2)
                                {
                                    CenterConsoleWrite(ConsoleColor.White, $"{players[i, x].Name}");
                                }
                                else if (a == 3)
                                {
                                    CenterConsoleWrite(ConsoleColor.White, $"{players[i, x].School}");
                                }
                                else if (a == 4)
                                {
                                    CenterConsoleWrite(ConsoleColor.DarkGreen, $"{players[i, x].Salary.ToString("c")}");
                                }
                            }
                        }
                    }
                    Console.WriteLine("");
                }
            }
            Console.WriteLine("\nThis program will now close.");
        }
        //Method found online 
        public static void ColoredConsoleWrite(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }
        public static void CenterConsoleWrite(ConsoleColor color, string text)
        {
            int length = 20;
            int stringLength = text.Length;
            int buffer = (length - stringLength) / 2;
            string spacer = "";
            for (int i = 0; i < buffer; i++)
            {
                spacer += " ";
            }
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (stringLength % 2 == 0)
            {
                Console.Write(spacer + text + spacer);
            }
            else
            {
                Console.Write(" " + spacer + text + spacer);
            }
            Console.ForegroundColor = originalColor;
        }
    }



    class Players
    {
        public string Name { get; set; }
        public string School { get; set; }
        public string Rank { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }
        public bool draftedPlayer = false;

    }
}
