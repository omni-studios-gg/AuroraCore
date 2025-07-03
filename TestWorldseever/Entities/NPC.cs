using System;

namespace WorldServerTest
{
    public class NPC
    {
        public string Type { get; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private readonly Random _random = new();

        public NPC(string type, int x = 0, int y = 0)
        {
            Type = type;
            X = x;
            Y = y;
        }

        public void Update()
        {
            int dx = _random.Next(-1, 2); // -1, 0, 1
            X += dx;
            Console.WriteLine($"👾 {Type} NPC moved to ({X},{Y})");
        }
    }
}