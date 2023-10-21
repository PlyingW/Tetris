using System;
using System.Threading;

class Program
{
    static int screenWidth = 20;
    static int screenHeight = 20;
    static char border = '*';
    static char block = '#';
    static char empty = ' ';
    static Random random = new Random();
    static int currentX = 8;
    static int currentY = 0;
    static int blockWidth = 4;
    static int blockHeight = 2;
    static char[,] playfield = new char[screenHeight, screenWidth];

    
    static char[,] figure1 = new char[,] {
        { block, block },
        { block, block },
    };

    static char[,] figure2 = new char[,] {
        { block, block, block, block }
    };

   
    static bool useFigure1 = true;

    static void Main()
    {
        Console.CursorVisible = false;
        Console.WindowHeight = screenHeight + 1;
        Console.WindowWidth = screenWidth + 1;

        InitializePlayfield();
        CreateNewBlock();

        while (true)
        {
            Console.Clear();
            DrawBorder();
            DrawPlayfield();
            DrawBlock(currentX, currentY);

            if (currentY + blockHeight < screenHeight && !CheckCollision(0, 1))
            {
                currentY++;
            }
            else
            {
                
                LockBlock();
                CreateNewBlock();
            }

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Spacebar)
                {
                    
                    useFigure1 = !useFigure1;
                }
                else
                {
                    MoveBlock(key);
                }
            }

            Thread.Sleep(200);
        }
    }

    static void InitializePlayfield()
    {
        for (int i = 0; i < screenHeight; i++)
        {
            for (int j = 0; j < screenWidth; j++)
            {
                playfield[i, j] = empty;
            }
        }
    }

    static void DrawBorder()
    {
        for (int i = 0; i < screenHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(border);
            Console.SetCursorPosition(screenWidth, i);
            Console.Write(border);
        }
        for (int i = 0; i < screenWidth; i++)
        {
            Console.SetCursorPosition(i, screenHeight);
            Console.Write(border);
        }
    }

    static void DrawPlayfield()
    {
        for (int i = 0; i < screenHeight; i++)
        {
            for (int j = 0; j < screenWidth; j++)
            {
                Console.SetCursorPosition(j, i);
                Console.Write(playfield[i, j]);
            }
        }
    }

    static void DrawBlock(int x, int y)
    {
        char[,] currentFigure = useFigure1 ? figure1 : figure2;

        for (int i = 0; i < currentFigure.GetLength(0); i++)
        {
            for (int j = 0; j < currentFigure.GetLength(1); j++)
            {
                Console.SetCursorPosition(x + j, y + i);
                if (currentFigure[i, j] == block)
                {
                    Console.Write(block);
                }
            }
        }
    }

    static void CreateNewBlock()
    {
        currentX = 8;
        currentY = 0;
        blockWidth = useFigure1 ? 2 : 4;
        blockHeight = useFigure1 ? 2 : 1;
        useFigure1 = !useFigure1;
    }

    static void MoveBlock(ConsoleKey key)
    {
        if (key == ConsoleKey.A && currentX > 0 && !CheckCollision(-1, 0))
        {
            currentX--;
        }
        else if (key == ConsoleKey.D && currentX + blockWidth < screenWidth && !CheckCollision(1, 0))
        {
            currentX++;
        }
    }

    static bool CheckCollision(int offsetX, int offsetY)
    {
        for (int i = 0; i < blockHeight; i++)
        {
            for (int j = 0; j < blockWidth; j++)
            {
                if (currentY + i + offsetY >= screenHeight || currentX + j + offsetX >= screenWidth || currentX + j + offsetX < 0 || playfield[currentY + i + offsetY, currentX + j + offsetX] == block)
                {
                    return true;
                }
            }
        }
        return false;
    }

    static void LockBlock()
    {
        for (int i = 0; i < blockHeight; i++)
        {
            for (int j = 0; j < blockWidth; j++)
            {
                if (currentY + i < screenHeight)
                {
                    playfield[currentY + i, currentX + j] = block;
                }
            }
        }
    }
}
