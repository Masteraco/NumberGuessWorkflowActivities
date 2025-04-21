// See https://aka.ms/new-console-template for more information
Console.WriteLine("Wellcome to the guess number game!");
Console.WriteLine("Choose 1. Play, 2. Exit");
string? input = Console.ReadLine();
int choice = 0;
if (!string.IsNullOrEmpty(input))
{
    choice = Int32.Parse(input);
}
do
{
    int target = new System.Random().Next(1, 100 + 1);
    int maxChances = 5;
    int chances = 0;
    while (chances < maxChances)
    {
        Console.WriteLine("Please enter your guess:");
        input = Console.ReadLine();
        chances++;
        if (!string.IsNullOrEmpty(input))
        {
            // Check if the input is a valid integer
            int guess = Int32.Parse(input);
            if (guess < target)
            {
                Console.WriteLine("Your guess is too low.");
                continue;
            }
            else if (guess > target)
            {
                Console.WriteLine("Your guess is too high.");
                continue;
            }
            else
            {
                Console.WriteLine("Congratulations! You guessed the number!");
                choice = 2;
                break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            continue;
        }
    }
} while (choice != 2);
