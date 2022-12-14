public class EliteStatusFile
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
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

    public void parseRawFlags () {
        var ED_LandingGearDown = 0x00000004;
        var ED_ScoopingFuel = 0x00000800;
        var ED_ShieldsUp = 0x00000008;
        var ED_Supercruise = 0x00000010;

        var valid = (this.Flags != 0);
        var LandingGearDown = (this.Flags & ED_LandingGearDown) != 0;
        var ScoopingFuel  = (this.Flags & ED_ScoopingFuel) != 0;
        var ShieldsUp = (this.Flags & ED_ShieldsUp) != 0;
        var Supercruise = (this.Flags & ED_Supercruise) != 0;

        Console.WriteLine($"LandingGearDown {LandingGearDown}");
        Console.WriteLine($"ScoopingFuel {ScoopingFuel}");
        Console.WriteLine($"ShieldsUp {ShieldsUp}");
        Console.WriteLine($"Supercruise {Supercruise}");
    } 
}