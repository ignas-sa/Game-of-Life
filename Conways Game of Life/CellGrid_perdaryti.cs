﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Windows.Forms;
 using SFML.System;

 namespace Conway_s_Game_of_Life
{
    class CellGrid
    {
        private Cell[,] cellGrid;
        private Size size;
        private Brush cAliveBrush = new SolidBrush(Color.Black),    // cState - dabartinė būsena
                      cDeadBrush = new SolidBrush(Color.White),
                      fAliveBrush = new SolidBrush(Color.DeepSkyBlue),  // fState - ateities būsena
                      fDeadBrush = new SolidBrush(Color.Red);
        
        public CellGrid(Size size, int cellColumns = 50, int cellRows = 50)
        {
            cellGrid = new Cell[cellColumns, cellRows];
            this.size = size;
            for (int x = 0; x < cellColumns; x++)
            {
                for (int y = 0; y < cellRows; y++)
                    cellGrid[x, y] = new Cell(new Vector2f(x, y));
            }
        }

        public CellGrid(Cell[,] cells)
        {
            cellGrid = cells;
        }

        public void CalculateNext()
        {
            Cell[,] futureCells = new Cell[cellGrid.GetLength(0), cellGrid.GetLength(1)];
            for (int x = 0; x < cellGrid.GetLength(0); x++)
            {
                for (int y = 0; y < cellGrid.GetLength(1); y++)
                {
                    if (cellGrid[x, y].CurrentState == State.Dead)
                    {
                        if (CountLiveNeighbors(x, y) == 3)
                            cellGrid[x, y].FutureState = State.Alive;
                    }
                    else
                    {
                        int neighbors = CountLiveNeighbors(x, y);
                        if (neighbors < 2 || neighbors > 3)
                            cellGrid[x, y].FutureState = State.Dead;
                        else
                            cellGrid[x, y].FutureState = State.Alive;
                    }
                }
            }
        }

        public void Draw(Graphics gr)
        {
            Brush b;
            for (int x = 0; x < cellGrid.GetLength(0); x++)
            {
                for (int y = 0; y < cellGrid.GetLength(1); y++)
                {
                    b = GetBrush(x, y);

                    int lX = cellGrid[x, y].Location.X,
                        lY = cellGrid[x, y].Location.Y,
                        sX = cellGrid[x, y].Size.Width,
                        sY = cellGrid[x, y].Size.Height;
                    cellGrid[x,y].Draw(gr, b);
                }
            }
        }

        private Brush GetBrush(int x, int y)
        {
            Brush brush;
            if (cellGrid[x, y].CurrentState == cellGrid[x, y].FutureState || cellGrid[x,y].FutureState == null)
            {
                if (cellGrid[x, y].CurrentState == State.Alive)
                    brush = cAliveBrush;
                else
                    brush = cDeadBrush;
            }
            else
            {
                if (cellGrid[x, y].FutureState == State.Alive)
                    brush = fAliveBrush;
                else
                    brush = fDeadBrush;
            }

            return brush;
        }

        private int CountLiveNeighbors(int cellX, int cellY)
        {
            int neighbors = (cellGrid[cellX, cellY].CurrentState == State.Alive)
                ? -1 // neskaičiuokime pačios ląstelės
                : 0;
            for (int x = cellX - 1; x <= cellX + 1; x++)
            {
                for (int y = cellY - 1; y <= cellY + 1; y++)
                {
                    if (x > 0 && x < cellGrid.GetLength(0) &&
                        y > 0 && y < cellGrid.GetLength(1))
                        if (cellGrid[x, y].CurrentState == State.Alive)
                            neighbors++;
                }
            }

            return neighbors;
        }

        public void ChangeCellOnMouseClick(MouseEventArgs e)
        {
            int x = e.X / size.Width,
                y = e.Y / size.Height;
            FlushFutureStates();
            cellGrid[x, y].CurrentState = (State)(((int)cellGrid[x, y].CurrentState + 1) % 2);
        }

        private void FlushFutureStates()
        {
            foreach (var cell in cellGrid)
                cell.FutureState = null;
        }
    }
}
