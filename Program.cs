using System;
using System.Threading;

class Program
{
    static bool useRectangle = true;
    static bool useSquare = false;
    static bool useLShape = false;
    static int screenWidth = 20;
    static int screenHeight = 20;
    static char border = '*';
    static char block = '#';
    static char empty = ' ';
    static Random random = new Random();
    static int currentX = 8;
    static int currentY = 0;
    static int blockWidth;
    static int blockHeight;
    static int rectangleRotationState = 0;
    static int lShapeRotationState = 0;
    static char[,] playfield = new char[screenHeight, screenWidth];
    static char[,] currentFigure;

    static char[,] square = new char[,] {
        { block, block },
        { block, block }
    };

    static char[,] rectangle = new char[,] {
        { block, block, block, block }
    };

    static char[,] lShape = new char[,] {
        { block, empty },
        { block, empty },
        { block, block }
    };

    static char[,] RotateMatrix(char[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        char[,] rotated = new char[cols, rows];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                rotated[j, rows - 1 - i] = matrix[i, j];
            }
        }

        return rotated;
    }

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
                ClearCompletedLines();
                CreateNewBlock();
            }

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                MoveBlock(key);
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

        int randomFigure = random.Next(3);

        useRectangle = false;
        useSquare = false;
        useLShape = false;

        if (randomFigure == 0)
        {
            useRectangle = true;
            blockWidth = 4;
            blockHeight = 1;
            currentFigure = rectangle;
        }
        else if (randomFigure == 1)
        {
            useSquare = true;
            blockWidth = 2;
            blockHeight = 2;
            currentFigure = square;
        }
        else
        {
            useLShape = true;
            blockWidth = 2;
            blockHeight = 3;
            currentFigure = lShape;
        }
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
        else if (key == ConsoleKey.UpArrow)
        {
            RotateCurrentFigure();

            if (currentX + currentFigure.GetLength(1) > screenWidth)
            {
                currentX = screenWidth - currentFigure.GetLength(1);
            }

            if (currentX < 0)
            {
                currentX = 0;
            }

            if (currentY + currentFigure.GetLength(0) > screenHeight)
            {
                currentY = screenHeight - currentFigure.GetLength(0);
            }
        }
    }

    static bool CheckCollision(int offsetX, int offsetY)
    {
        for (int i = 0; i < currentFigure.GetLength(0); i++)
        {
            for (int j = 0; j < currentFigure.GetLength(1); j++)
            {
                int y = currentY + i + offsetY;
                int x = currentX + j + offsetX;

                if (y >= screenHeight || x >= screenWidth || x < 0)
                {
                    return true; 
                }

                if (y >= 0 && currentFigure[i, j] == block && playfield[y, x] == block)
                {
                    return true; 
                }
            }
        }

        return false;
    }



    static void LockBlock()
    {
        for (int i = 0; i < currentFigure.GetLength(0); i++)
        {
            for (int j = 0; j < currentFigure.GetLength(1); j++)
            {
                int y = currentY + i;
                int x = currentX + j;

                if (y >= 0 && y < screenHeight && x >= 0 && x < screenWidth)
                {
                    if (currentFigure[i, j] == block)
                    {
                        playfield[y, x] = block;
                    }
                }
            }
        }
    }

    static void ClearCompletedLines()
    {
        for (int i = screenHeight - 1; i >= 0; i--)
        {
            bool isLineComplete = true;

            for (int j = 0; j < screenWidth; j++)
            {
                if (playfield[i, j] == empty)
                {
                    isLineComplete = false;
                    break;
                }
            }

            if (isLineComplete)
            {
                for (int j = i; j > 0; j--)
                {
                    for (int k = 0; k < screenWidth; k++)
                    {
                        playfield[j, k] = playfield[j - 1, k];
                    }
                }
            }
        }
    }


    static void RotateCurrentFigure()
    {
        currentFigure = RotateMatrix(currentFigure);

        if (useRectangle)
        {
            rectangleRotationState = (rectangleRotationState + 1) % 4;
        }
        else if (useLShape)
        {
            lShapeRotationState = (lShapeRotationState + 1) % 4;
        }
    }
}
