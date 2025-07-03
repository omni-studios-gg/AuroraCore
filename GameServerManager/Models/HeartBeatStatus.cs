namespace GameServerManager.Models;

public class HeartBeatStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string IP { get; set; }
    public int Port { get; set; }
    public DateTime LastSeen { get; set; }
}