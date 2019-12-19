﻿using System;
using System.Collections.Generic;
using System.Text;
 using Conways_Game_of_Life;
 using Conway_s_Game_of_Life;
 using SFML.Graphics;
 using SFML.System;


 namespace Conway_s_Game_of_Life
{
    enum State
    {
        Dead = 0,
        Alive = 1
    }

    class Cell
    {
        public State? CurrentState { get; set; }
        public State? FutureState = null;
        public bool DrawnOver = false;
        
        private RectangleShape cellRect;
        public Cell(Vector2f location, Vector2f size, State state = State.Dead)
        {
            CurrentState = state;
            cellRect = new RectangleShape(size);
            cellRect.Position = location;
        }

        public void Draw(Color color)
        {
            cellRect.FillColor = color;
            Program.window.Draw(cellRect);
        }
    }
}
