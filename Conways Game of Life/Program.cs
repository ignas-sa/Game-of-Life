﻿using System;
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
        private static bool mouseButtonPressed = false;
        private static int deltaInterval = 5;
        
        static void Main(string[] args)
        {
            int width = 800, height = 450;
            WindowSize.Width = width;
            WindowSize.Height = height;
            int cellSizeX = 8, cellSizeY = 8;
            window = new RenderWindow(new VideoMode((uint)width, (uint)height), "Conway's Game of Life");
            cellGrid = new CellGrid(cellSizeX, cellSizeY);

            timer.Enabled = false;
            timer.Interval = 20;
            timer.AutoReset = true;
            timer.Elapsed += TimerTick;
            window.Closed += OnClose;
            window.MouseButtonPressed += OnMousePressed;
            window.MouseButtonReleased += OnMouseReleased;
            window.KeyPressed += OnKeyPressed;

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.White);
                cellGrid.Draw();
                window.Display();
            }
        }

        private static void TimerTick(object sender, ElapsedEventArgs e)
        {
            cellGrid.CalculateNext();
        }

        private static void OnClose(object sender, EventArgs e)
        {
            window.Close();
        }

        private static void OnMousePressed(object sender, MouseButtonEventArgs e)
        {
            mouseButtonPressed = true;
            if (!timer.Enabled)
                cellGrid.ChangeCellFromMouse(e);
        }

        private static void OnMouseReleased(object sender, MouseButtonEventArgs e)
        {
            mouseButtonPressed = false;
        }

        private static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Space:
                    timer.Enabled = !timer.Enabled;
                    if (!timer.Enabled)
                        cellGrid.PurgeFutureStates();
                    break;
                case Keyboard.Key.Add:
                    timer.Interval += deltaInterval;
                    break;
                case Keyboard.Key.Subtract:
                    timer.Interval -= deltaInterval;
                    break;
                case Keyboard.Key.F1:    // help screen
                    break;
            }
        }
    }
}