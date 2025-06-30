namespace Authserver.Models;

public class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string password_hash { get; set; }  // assuming it's stored
    public string last_ip { get; set; }
    public DateTime? last_login { get; set; }

    // ✅ Add this line:
    public DateTime created_at { get; set; }
    public string os { get; set; }
}