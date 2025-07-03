using MySql.Data.MySqlClient;
using PlayerManagerServer.Models;

public class InventoryDB
{
    private readonly string _connStr;

    public InventoryDB(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default");
    }

    public List<InventoryItem> LoadInventory(int playerId)
    {
        var items = new List<InventoryItem>();

        using var conn = new MySqlConnection(_connStr);
        conn.Open();

        string sql = "SELECT item_id, quantity FROM inventory WHERE player_id = @playerId";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@playerId", playerId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            items.Add(new InventoryItem
            {
                ItemId = reader.GetInt32("item_id"),
                Quantity = reader.GetInt32("quantity")
            });
        }

        return items;
    }
}