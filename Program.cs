using System;
using System.Threading;

class Program
{
    static void Main()
    {
        int windowHeight = 20;  
        int windowWidth = 40;   

        
        Console.WindowHeight = windowHeight;
        Console.WindowWidth = windowWidth;
        Console.BufferHeight = windowHeight;
        Console.BufferWidth = windowWidth;

        int squareSize = 5;     
        int squareX = windowWidth / 2 - squareSize / 2; 
        int squareY = 0;         
        int groundY = windowHeight - 1;

        while (true)
        {
         
            DrawSquare(squareX, squareY, squareSize);

            
            squareY++;

            
            if (squareY + squareSize >= groundY)
            {
                ClearSquare(squareX, squareY - 1, squareSize);
            }

           
            Thread.Sleep(100);
        }
    }

 
    static void DrawSquare(int x, int y, int size)
    {
        for (int i = y; i < y + size; i++)
        {
            Console.SetCursorPosition(x, i);
            for (int j = x; j < x + size; j++)
            {
                Console.Write("■");
            }
        }
    }

    
    static void ClearSquare(int x, int y, int size)
    {
        for (int i = y; i < y + size; i++)
        {
            Console.SetCursorPosition(x, i);
            for (int j = x; j < x + size; j++)
            {
                Console.Write(" ");
            }
        }
    }
}
