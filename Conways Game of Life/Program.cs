using System;
using System.Timers;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace Conways_Game_of_Life
{
    public static class WindowSize
    {
        public static int Width, Height;
    }
    class Program
    {
        public static RenderWindow window;
        private static Timer timer = new Timer();
        private static CellGrid cellGrid;
        
        static void Main(string[] args)
        {
            int width = 800, height = 450;
            WindowSize.Width = width;
            WindowSize.Height = height;
            window = new RenderWindow(new VideoMode((uint)width, (uint)height), "Conway's Game of Life");
            cellGrid = new CellGrid();

            timer.Enabled = false;
            timer.Elapsed += TimerTick;
            window.Closed += OnClose;
        }

        private static void TimerTick(object sender, ElapsedEventArgs e)
        {
            cellGrid.CalculateNext();
            Draw();
        }

        private static void Draw()
        {
            window.Clear();
            cellGrid.Draw();
            window.Display();
        }

        private static void OnClose(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}