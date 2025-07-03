using System;

namespace WorldServerTest
{
    public class Player
    {
        public string Name { get; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Player(string name, int x = 0, int y = 0)
        {
            Name = name;
            X = x;
            Y = y;
        }

        public void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
            Console.WriteLine($"👤 {Name} moved to ({X},{Y})");
        }
    }
}