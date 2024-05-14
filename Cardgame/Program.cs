using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the random number generator
            Random num = new Random();

            // Prompt for the number of players and ensure it's between 2 and 4
            Console.WriteLine("Enter the number of players (2-4):");
            int playerCount = int.Parse(Console.ReadLine());
            playerCount = Math.Max(2, Math.Min(4, playerCount));

            // Initialize arrays to track active players and their totals
            bool[] activePlayers = new bool[playerCount];
            Array.Fill(activePlayers, true);
            int activeCount = playerCount;

            // Main game loop continues until only one player remains
            while (activeCount > 1)
            {
                // Draw the computer's first card
                int computerCard1 = DrawCard(num);
                Console.WriteLine("Computer's first card: " + DisplayCardValue(computerCard1));
                int computerTotal = computerCard1;
                bool anyTwoCards = false;

                // Arrays to store each player's total points and choices
                int[] playerTotals = new int[playerCount];
                int[] choices = new int[playerCount];

                // Loop through each player for their turn
                for (int i = 0; i < playerCount; i++)
                {
                    if (activePlayers[i])
                    {
                        Console.WriteLine($"Player {i + 1}, do you want to draw 1 or 2 cards? (Enter 1 or 2):");
                        int choice = int.Parse(Console.ReadLine());
                        choices[i] = choice;

                        int playerCard1 = DrawCard(num);
                        playerTotals[i] = AdjustAceValue(playerCard1);
                        Console.WriteLine($"Player {i + 1} drew: {DisplayCardValue(playerCard1)}");

                        // Handle the case where the player chooses to draw two cards
                        if (choice == 2)
                        {
                            int playerCard2 = DrawCard(num);
                            playerTotals[i] += AdjustAceValue(playerCard2);
                            Console.WriteLine($"and {DisplayCardValue(playerCard2)}");
                            anyTwoCards = true;
                        }
                    }
                }

                // Draw an additional card for the computer if any player drew two cards
                if (anyTwoCards)
                {
                    int computerCard2 = DrawCard(num);
                    computerTotal += AdjustAceValue(computerCard2);
                    Console.WriteLine($"Computer draws another card: {DisplayCardValue(computerCard2)}");
                }

                // Show the computer's total card value
                Console.WriteLine($"Computer's total card value: {computerTotal}");

                // Determine and announce the outcome of the round
                for (int i = 0; i < playerCount; i++)
                {
                    if (activePlayers[i])
                    {
                        // Determine if a player is eliminated or wins the round
                        if ((choices[i] == 1 && playerTotals[i] < computerCard1) ||
                            (choices[i] == 2 && playerTotals[i] > computerTotal))
                        {
                            Console.WriteLine($"Player {i + 1} wins this round!");
                        }
                        else
                        {
                            Console.WriteLine($"Player {i + 1} is eliminated!");
                            activePlayers[i] = false;
                            activeCount--;
                        }
                    }
                }

                // Prompt for the next round if more than one player remains
                if (activeCount > 1)
                {
                    Console.WriteLine("Next round begins...");
                }
            }

            // Announce the game winner
            for (int i = 0; i < playerCount; i++)
            {
                if (activePlayers[i])
                {
                    Console.WriteLine($"Player {i + 1} wins the game!");
                    break;
                }
            }
        }

        // Helper method to draw a card
        static int DrawCard(Random num)
        {
            return num.Next(1, 14); // Draws a card from 1 to 13
        }

        // Helper method to display card values
        static string DisplayCardValue(int cardValue)
        {
            return cardValue switch
            {
                13 => "King",
                12 => "Queen",
                11 => "Jack",
                1 => "Ace",
                _ => cardValue.ToString()
            };
        }

        // Helper method to adjust the value of face cards and Aces
        static int AdjustAceValue(int cardValue)
        {
            return cardValue == 1 ? 11 : cardValue > 10 ? 10 : cardValue;
        }
    }
}
