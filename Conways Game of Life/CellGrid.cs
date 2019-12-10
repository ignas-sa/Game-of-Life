using Conway_s_Game_of_Life;
using SFML.Graphics;
using SFML.System;

namespace Conways_Game_of_Life
{
    public static class CellGridSize
    {
        public static int Width, Height;
    }
    class CellGrid
    {
        private Cell[,] cellGrid { get; }
        
        public CellGrid(int cellColumns = 400, int cellRows = 225)
        {
            CellGridSize.Width = cellColumns;
            CellGridSize.Height = cellRows;
            cellGrid = new Cell[cellColumns, cellRows];
            for (int x = 0; x < cellColumns; x++)
            {
                for (int y = 0; y < cellRows; y++)
                    cellGrid[x, y] = new Cell(new Vector2f(x, y));
            }
        }

        public void CalculateNext()
        {
            PushToCurrent();
            for (int x = 0; x < cellGrid.GetLength(0); x++)
            {
                for (int y = 0; y < cellGrid.GetLength(1); y++)
                {
                    if (cellGrid[x, y].CurrentState == State.Dead && CountNeighbours(x, y) == 3)
                        cellGrid[x, y].FutureState = State.Alive;
                    else
                    {
                        int neighbours = CountNeighbours(x, y);
                        if (neighbours < 2 || neighbours > 3)
                            cellGrid[x, y].FutureState = State.Dead;
                        else
                            cellGrid[x, y].FutureState = State.Alive;
                    }
                }
            }
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
            bool canPush = true;
            for (int x = 0; x < cellGrid.GetLength(0) && canPush; x++)
            {
                for (int y = 0; y < cellGrid.GetLength(1) && canPush; y++)
                    if (cellGrid[x, y].FutureState == null)
                        canPush = false;
            }

            if (canPush)
            {
                foreach (var cell in cellGrid)
                    cell.CurrentState = cell.FutureState;
            }
        }

        private int CountNeighbours(int X, int Y)
        {
            int neighbours = (cellGrid[X, Y].CurrentState == State.Alive)
                ? -1
                : 0;
            for (int x = X - 1; x < X - 2; x++)
            {
                for (int y = Y - 1; y < Y - 2; y++)
                {
                    if (x >= 0 && x < cellGrid.GetLength(0) &&
                        y >= 0 && y < cellGrid.GetLength(1))
                        if (cellGrid[x, y].CurrentState == State.Alive)
                            neighbours++;
                }
            }

            return neighbours;
        }

        private void PurgeFutureStates()
        {
            foreach (var cell in cellGrid)
                cell.FutureState = null;
        }
    }
}