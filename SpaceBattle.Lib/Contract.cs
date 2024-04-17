public class Contract
{
    public string? type { get; set; }
    public string? gameId { get; set; }
    public int gameItemId { get; set; }
    public Dictionary<string, object>? parameters;
}
