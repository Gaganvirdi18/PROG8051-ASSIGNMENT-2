// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
// See https://aka.ms/new-console-template for more information
using System;

class Position
{
    public int X { get; } // This is for to check the position of the Player in the board 
    public int Y { get; } // This is for to check the position of the Player in the

    public Position(int x, int y)// aDDED POSITIONS OF PALYERS
    {
        X = x;    // To intialise thees  values by creating the object with the values
        Y = y;
    }
}

class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }
    public Position[] PositionHistory { get; } = new Position[30]; // Assuming a maximum of 30 positions as board is 6X6.

    public int currentPositionIndex = 0; // To check the Index of the array of the board

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
        PositionHistory[currentPositionIndex] = position;
    }

    public void Move(char direction)
    {
        int newX = Position.X; // Fetch the value of the postion class attribute X
        int newY = Position.Y;

        switch (direction)
        {
            case 'U':
                newY--;
                break;
            case 'D':
                newY++;
                break;
            case 'L':
                newX--;
                break;
            case 'R':
                newX++;
                break;
            default:
                Console.WriteLine("Invalid direction. Please use U, D, L, or R."); // If user enter the values rathar than U/L/D/R
                return;
        }

        if (currentPositionIndex + 1 < PositionHistory.Length)
        {
            currentPositionIndex++;
            PositionHistory[currentPositionIndex] = new Position(newX, newY);
            Position = PositionHistory[currentPositionIndex];
        }
    }
}

class Cell
{
    public string Occupant { get; set; }
}

class Board
{
    private Cell[,] Grid;

    public Board()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        Grid = new Cell[6, 6];

        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                Grid[x, y] = new Cell { Occupant = "-" };
            }
        }

        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";
        //Initialise the gem values 
        Grid[4, 3].Occupant = "G";
        Grid[4, 1].Occupant = "G";
        Grid[3, 3].Occupant = "G";
        Grid[1, 5].Occupant = "G";
        Grid[5, 3].Occupant = "G";
        Grid[2, 1].Occupant = "G";
        Grid[3, 3].Occupant = "G";
        Grid[3, 1].Occupant = "G";

        // intialise the obstackles
        Grid[1, 2].Occupant = "O";
        Grid[3, 4].Occupant = "O";
        Grid[0, 4].Occupant = "O";
        Grid[2, 4].Occupant = "O";
    }

    public void Display()
 {
     Console.WriteLine("******************************************");

     for (int y = 0; y < 6; y++)
     {
         for (int x = 0; x < 6; x++)
         {
             Console.Write($" | {Grid[x, y].Occupant} | ");
         }
         Console.WriteLine("\n******************************************");
     }
 }


    public void UpdateBoardWithPlayers(Player player1, Player player2)
    {
        int previousX1 = player1.PositionHistory[player1.currentPositionIndex].X;
        int previousY1 = player1.PositionHistory[player1.currentPositionIndex].Y;

        int previousX2 = player2.PositionHistory[player2.currentPositionIndex].X;
        int previousY2 = player2.PositionHistory[player2.currentPositionIndex].Y;

        if (previousX1 >= 0 && previousX1 < 6 && previousY1 >= 0 && previousY1 < 6)
        {
            Grid[previousX1, previousY1].Occupant = "-";
        }

        if (previousX2 >= 0 && previousX2 < 6 && previousY2 >= 0 && previousY2 < 6)
        {
            Grid[previousX2, previousY2].Occupant = "-";
        }

        Grid[player1.Position.X, player1.Position.Y].Occupant = "P1";
        Grid[player2.Position.X, player2.Position.Y].Occupant = "P2";
    }


    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U':
                newY--;
                break;
            case 'D':
                newY++;
                break;
            case 'L':
                newX--;
                break;
            case 'R':
                newX++;
                break;
            default:
                Console.WriteLine("Invalid direction. Please use U, D, L, or R.");
                return false;
        }


        if (newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && Grid[newX, newY].Occupant != "O")
        {


            Grid[player.Position.X, player.Position.Y].Occupant = "-";




            return true;
        }
        else
        {
            Console.WriteLine("Invalid move. Obstacle or out of bounds.");
            return false;
        }
    }

    public void CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.GemCount++; //update the game 
            Grid[player.Position.X, player.Position.Y].Occupant = "-";
        }
    }
}

class Game
{
    private Board Board;
    private Player Player1;
    private Player Player2;
    private Player CurrentTurn;
    private int TotalTurns;

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    public void Start()
    {
        while (!IsGameOver())
        {
            Console.WriteLine($"Turn {TotalTurns + 1}: {CurrentTurn.Name}'s turn");
            Board.UpdateBoardWithPlayers(Player1, Player2);
            Board.Display();

            Console.Write("Choose the direction in the upper case letter (U/D/L/R): ");
            char direction = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (Board.IsValidMove(CurrentTurn, direction))
            {
                CurrentTurn.Move(direction);
                Board.CollectGem(CurrentTurn);

                Console.WriteLine($"{CurrentTurn.Name} moved {direction}");
                Board.UpdateBoardWithPlayers(Player1, Player2);
                Board.Display();

                SwitchTurn();
                TotalTurns++;
            }
        }

        AnnounceWinner();
    }

    private void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    private bool IsGameOver()
    {
        return TotalTurns >= 30;
    }

    private void AnnounceWinner()
    {
        Console.WriteLine("Game over!");
        Console.WriteLine($"{Player1.Name} collected {Player1.GemCount} gems. Movements: {GetPlayerMovements(Player1)}");
        Console.WriteLine($"{Player2.Name} collected {Player2.GemCount} gems. Movements: {GetPlayerMovements(Player2)}");

        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine($"{Player1.Name} wins!");
        }
        else if (Player1.GemCount < Player2.GemCount)
        {
            Console.WriteLine($"{Player2.Name} wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }

    private string GetPlayerMovements(Player player)
    {
        string movements = "";
        for (int i = 0; i <= player.currentPositionIndex; i++)
        {
            movements += $"({player.PositionHistory[i].X},{player.PositionHistory[i].Y}) ";
        }
        return movements;
    }
}

class Program
{
    static void Main()
    {
        Game gemHuntersGame = new Game();
        gemHuntersGame.Start();
    }
}

