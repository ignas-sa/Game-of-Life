using System;
using Conway_s_Game_of_Life;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Conways_Game_of_Life
{
    public static class CellGridSize
    {
        public static int Width, Height;
    }
    class CellGrid
    {
        private Cell[,] cellGrid { get; }
        private Cell[,] futureCellGrid { get; }
        
        public CellGrid(int sizeX, int sizeY)
        {
            int cellColumns = WindowSize.Width / sizeX,
                cellRows = WindowSize.Height / sizeY;
            CellGridSize.Width = cellColumns;
            CellGridSize.Height = cellRows;
            Vector2f size = new Vector2f(sizeX, sizeY);
            cellGrid = new Cell[cellColumns, cellRows];
            futureCellGrid = new Cell[cellColumns, cellRows];
            for (int x = 0; x < cellColumns; x++)
            {
                for (int y = 0; y < cellRows; y++)
                    cellGrid[x, y] = new Cell(new Vector2f(sizeX*x, sizeY*y), size);
            }
        }

        public void CalculateNext()
        {
            for (int x = 0; x < cellGrid.GetLength(0); x++)
            {
                for (int y = 0; y < cellGrid.GetLength(1); y++)
                {
                    int neighbours = CountNeighbours(x, y);
                    if (cellGrid[x, y].CurrentState == State.Alive)
                        cellGrid[x, y].FutureState = (neighbours == 2 || neighbours == 3)
                            ? State.Alive
                            : State.Dead;
                    else
                        cellGrid[x, y].FutureState = (neighbours == 3)
                            ? State.Alive
                            : State.Dead;
                }
            }
            PushToCurrent();
        }

        public void Draw()
        {
            Color color;
            foreach (var cell in cellGrid)
            {
                color = GetColor(cell);
                cell.Draw(color);
            }
        }
        
        public void PurgeFutureStates()
        {
            foreach (var cell in cellGrid)
                cell.FutureState = null;
        }

        public void ChangeCellFromMouse(MouseButtonEventArgs e)
        {
            int X = e.X / (WindowSize.Width / CellGridSize.Width),    // get mouse coordinates, relative to cells
                Y = e.Y / (WindowSize.Height / CellGridSize.Height);
            if (X < cellGrid.GetLength(0) && Y < cellGrid.GetLength(1))
                InvertCellState(ref cellGrid[X, Y]);
        }

        private void InvertCellState(ref Cell cell)
        {
            cell.CurrentState = (cell.CurrentState != null)
                ? (State) (((int) cell.CurrentState + 1) % 2)
                : State.Alive;
        }

        private Color GetColor(Cell cell)
        {
            Color color;
            
            if (cell.CurrentState == cell.FutureState || cell.FutureState == null)
                color = (cell.CurrentState == State.Alive)
                    ? Color.Black
                    : Color.White;
            else
                color = (cell.FutureState == State.Alive)
                    ? Color.Cyan
                    : Color.Red;

            return color;
        }

        private void PushToCurrent()
        {
            foreach (var cell in cellGrid)
                cell.CurrentState = cell.FutureState;
        }

        private int CountNeighbours(int X, int Y)
        {
            int neighbours = 0 - (int)cellGrid[X, Y].CurrentState;
            for (int x = X - 1; x < X + 2; x++)
            {
                for (int y = Y - 1; y < Y + 2; y++)
                {
                    if (x >= 0 && x < cellGrid.GetLength(0) &&
                        y >= 0 && y < cellGrid.GetLength(1))
                        if (cellGrid[x, y].CurrentState == State.Alive)
                            neighbours++;
                }
            }

            return neighbours;
        }
    }
}