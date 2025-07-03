using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWorldServer
{
    // Zones are just integer IDs for simplicity
    public enum ZoneId
    {
        Zone1 = 1,
        Zone2 = 2,
    }

    public class Player
    {
        public string Name { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public ZoneId Zone { get; set; } = ZoneId.Zone1;

        public void Move(int dx, int dy)
        {
            PositionX += dx;
            PositionY += dy;
            Console.WriteLine($"{Name} moved to ({PositionX},{PositionY}) in zone {Zone}");
            UpdateZone();
        }

        private void UpdateZone()
        {
            // Simple zone logic: if X >= 10, go to Zone2, else Zone1
            Zone = PositionX >= 10 ? ZoneId.Zone2 : ZoneId.Zone1;
        }
    }

    public class NPC
    {
        public string Type { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public ZoneId Zone { get; set; } = ZoneId.Zone1;

        private Random rnd = new();

        public void Update()
        {
            int dx = rnd.Next(-1, 2);
            PositionX += dx;
            UpdateZone();
            Console.WriteLine($"{Type} NPC moved to ({PositionX},{PositionY}) in zone {Zone}");
        }

        private void UpdateZone()
        {
            Zone = PositionX >= 10 ? ZoneId.Zone2 : ZoneId.Zone1;
        }
    }

    class Program
    {
        static List<Player> players = new()
        {
            new Player { Name = "PlayerOne", PositionX = 0, PositionY = 0 },
            new Player { Name = "PlayerTwo", PositionX = 5, PositionY = 5 }
        };

        static List<NPC> npcs = new()
        {
            new NPC { Type = "Goblin", PositionX = 10, PositionY = 0 },
            new NPC { Type = "Dragon", PositionX = -5, PositionY = 3 }
        };

        static bool running = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Zone World Server...");
            Task.Run(GameLoop);

            Console.WriteLine("Press Q to quit.");

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Q)
                {
                    running = false;
                    break;
                }
            }

            Console.WriteLine("Shutting down...");
            Thread.Sleep(1000);
        }

        static async Task GameLoop()
        {
            const int tickRateMs = 1000;

            while (running)
            {
                Console.WriteLine("Tick: " + DateTime.Now);

                // Move playerOne right by 1
                players[0].Move(1, 0);
                players[1].Move(1, 0);

                // Update NPCs
                foreach (var npc in npcs)
                    npc.Update();

                // Send updates per player limited to their zone:
                foreach (var player in players)
                {
                    Console.WriteLine($"Sending update to {player.Name} for zone {player.Zone}");

                    // Find all players and NPCs in the same zone
                    var visiblePlayers = players.FindAll(p => p.Zone == player.Zone);
                    var visibleNpcs = npcs.FindAll(n => n.Zone == player.Zone);

                    // For simplicity just print info
                    Console.WriteLine($" -> Players visible to {player.Name}:");
                    foreach (var vp in visiblePlayers)
                        Console.WriteLine($"    {vp.Name} at ({vp.PositionX},{vp.PositionY})");

                    Console.WriteLine($" -> NPCs visible to {player.Name}:");
                    foreach (var vn in visibleNpcs)
                        Console.WriteLine($"    {vn.Type} at ({vn.PositionX},{vn.PositionY})");
                }

                await Task.Delay(tickRateMs);
            }
        }
    }
}
