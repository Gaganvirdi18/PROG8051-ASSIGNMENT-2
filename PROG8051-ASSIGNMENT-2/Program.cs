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
    public Position[] LocationHistory { get; } = new Position[30]; // Assuming a maximum of 30 positions as board is 6X6.

    public int current_Position_Index = 0; // To check the Index of the array of the board

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
        LocationHistory[current_Position_Index] = position;
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

        if (current_Position_Index + 1 < LocationHistory.Length)
        {
            current_Position_Index++;
            LocationHistory[current_Position_Index] = new Position(newX, newY);
            Position = LocationHistory[current_Position_Index];
        }
    }
}

class Cell
{
    public string Holder { get; set; }
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
                Grid[x, y] = new Cell { Holder = "-" };
            }
        }

        Grid[0, 0].Holder = "P1";
        Grid[5, 5].Holder = "P2";

        //Initialise the gem values 
        Grid[4, 3].Holder = "G";
        Grid[1, 1].Holder = "G";
        Grid[3, 3].Holder = "G";
        Grid[2, 5].Holder = "G";
        Grid[5, 3].Holder = "G";
        Grid[2, 2].Holder = "G";
        Grid[3, 3].Holder = "G";
        Grid[5, 4].Holder = "G";

        // intialise the obstackles
        Grid[1, 2].Holder = "O";
        Grid[3, 2].Holder = "O";
        Grid[1, 4].Holder = "O";
        Grid[3, 4].Holder = "O";
    }
    /*
        public void Display()
        {

            Console.WriteLine("__________________________________________");

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    Console.Write($" | {Grid[x, y].Holder} | ");
                }
                Console.WriteLine("\n_________________________________________");
            }
        }
    */
    public void Display()
    {
        Console.WriteLine("******************************************");

        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                Console.Write($" | {Grid[x, y].Holder} | ");
            }
            Console.WriteLine("\n******************************************");
        }
    }

    public void UpdateBoardWithPlayers(Player player1, Player player2)
    {
        int prevX1 = player1.LocationHistory[player1.current_Position_Index].X;
        int prevY1 = player1.LocationHistory[player1.current_Position_Index].Y;

        int prevX2 = player2.LocationHistory[player2.current_Position_Index].X;
        int prevY2 = player2.LocationHistory[player2.current_Position_Index].Y;

        if (prevX1 >= 0 && prevX1 < 6 && prevY1 >= 0 && prevY1 < 6)
        {
            Grid[prevX1, prevY1].Holder = "-";
        }

        if (prevX2 >= 0 && prevX2 < 6 && prevY2 >= 0 && prevY2 < 6)
        {
            Grid[prevX2, prevY2].Holder = "-";
        }

        Grid[player1.Position.X, player1.Position.Y].Holder = "P1";
        Grid[player2.Position.X, player2.Position.Y].Holder = "P2";
         if ((player1.Position.X == player2.Position.X) &&(player1.Position.Y == player2.Position.Y)) // if players are in same locations 
 {
     Grid[player2.Position.X, player2.Position.Y].Holder = "P1, P2";
 }
    }


    public bool IsTrueMove(Player player, char direction)
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
                System.Threading.Thread.Sleep(2000);
                return false;
        }


        if (newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && Grid[newX, newY].Holder != "O")
        {


            Grid[player.Position.X, player.Position.Y].Holder = "-";




            return true;
        }
        else
        {
            Console.WriteLine("Invalid move. Obstacle or out of limits.");
            System.Threading.Thread.Sleep(2000);
            return false;
        }
    }

    public void CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Holder == "G")
        {
            player.GemCount++; //update the game 
            Grid[player.Position.X, player.Position.Y].Holder = "-";
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
            Console.Clear();// to clear the previous value of board

            Console.WriteLine($"Turn {TotalTurns + 1}: {CurrentTurn.Name}'s turn");
            Board.UpdateBoardWithPlayers(Player1, Player2);
            
            Board.Display();

            
            Console.Write("Select the direction in upper case letters (U/D/L/R):  ");
            char direction = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (Board.IsTrueMove(CurrentTurn, direction))
            {
                CurrentTurn.Move(direction);
                Board.CollectGem(CurrentTurn);

                if ( direction=='U')
                    Console.WriteLine($"{CurrentTurn.Name} moved Upward direction");
                else  if (direction == 'L')
                    Console.WriteLine($"{CurrentTurn.Name} moved Left direction");
                else if(direction == 'R')
                    Console.WriteLine($"{CurrentTurn.Name} moved Right direction");
                else Console.WriteLine($"{CurrentTurn.Name} moved Downward direction");
                System.Threading.Thread.Sleep(1000); // TO SHOW PLAYER IS MOVING IN WHICH DIRECTION

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

         Console.WriteLine($"{Player1.Name} collected {Player1.GemCount} gems.\n");
         Console.WriteLine($"{Player2.Name} collected {Player2.GemCount} gems. \n");
        

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
            Console.WriteLine("The game ends in a tie between the two players!");
        }
    }

   
}

class Program
{
    static void Main ()
    {
        Console.WriteLine("Welcome to Gem Hunters Game!");
        System.Threading.Thread.Sleep(2000);
        Console.WriteLine("Let the game begin!\n");
        System.Threading.Thread.Sleep(1000);

        Game gemHuntersGame = new Game();
        gemHuntersGame.Start();
    }
}
