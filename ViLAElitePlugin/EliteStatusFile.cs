public class EliteStatusFile
{
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public string Event { get; set; } = "";
    public int Flags { get; set; } = 0;
    public List<int>? Pips { get; set; } = new List<int>();
    public int? FireGroup { get; set; } = 0;
    public Dictionary<string, float>?  Fuel { get; set; } = new Dictionary<string, float> {{"FuelMain", 0}, {"FuelReservoir", 0}};
    public int? GuiFocus { get; set; } = 0;
    public float? Latitude { get; set; } = 0;
    public float? Longitude { get; set; } = 0;
    public int? Heading { get; set; } = 0;
    public int? Altitude { get; set; } = 0;
}