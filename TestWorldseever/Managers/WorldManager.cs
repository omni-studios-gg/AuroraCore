using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorldServerTest
{
    public class WorldManager
    {
        private readonly List<Player> _players = new();
        private readonly List<NPC> _npcs = new();
        private bool _running = true;
        private const int TickRateMs = 1000;

        public async Task StartAsync()
        {
            Console.WriteLine("✅ World Server Started.");

            InitSampleData();
            _ = Task.Run(InputLoop); // Handle console input
            await GameLoop();
        }

        private void InitSampleData()
        {
            _players.Add(new Player("PlayerOne"));
            _players.Add(new Player("PlayerTwo", 5, 5));

            _npcs.Add(new NPC("Wolf", 3, 0));
            _npcs.Add(new NPC("Orc", -2, 1));
        }

        private async Task GameLoop()
        {
            while (_running)
            {
                Console.WriteLine($"\n🕒 Tick: {DateTime.UtcNow:T}");

                foreach (var player in _players)
                    player.Move(1, 0); // Example logic

                foreach (var npc in _npcs)
                    npc.Update();

                await Task.Delay(TickRateMs);
            }
        }

        private async Task InputLoop()
        {
            while (_running)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("🛑 Stopping World...");
                    _running = false;
                }
                await Task.Delay(50);
            }
        }
    }
}