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

    [Ignore]
    public bool IsEditable1 { get; set; } = true; // 标记输入框是否可编辑
    [Ignore]
    public bool IsEditable2 { get; set; } = false; // 标记输入框是否可编辑
}