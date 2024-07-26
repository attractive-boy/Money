// Models/UserTotal.cs
using SQLite;

public class UserTotal
{
    [PrimaryKey]
    public string Username { get; set; }
    public decimal Total { get; set; }

    public string Order { get; set; }
    public string Amount { get; set; }
    public string Settlement { get; set; }
}