using System;
using SFML.Graphics;
using SFML.System;

namespace Conways_Game_of_Life
{
    static class MiscDrawables
    {
        public static VertexArray Grid { get; private set; }

        public static void InitializeGrid(int cellSizeX, int cellSizeY)
        {
            Color color = new Color(127, 127, 127);
            Grid = new VertexArray(PrimitiveType.Lines);
            for (int x = 0; x < WindowSize.Width; x += cellSizeX)
            {
                for (int y = 0; y < WindowSize.Height; y += cellSizeY)
                {
                    Grid.Append(new Vertex(new Vector2f(x, 0), color));
                    Grid.Append(new Vertex(new Vector2f(x, WindowSize.Height), color));
                    Grid.Append(new Vertex(new Vector2f(0, y), color));
                    Grid.Append(new Vertex(new Vector2f(WindowSize.Width, y), color));
                }
            }

            Grid.PrimitiveType = PrimitiveType.Lines;
        }
    }
}