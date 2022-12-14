using System.Reflection;

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

    // Calculated values
    public int? FlagsValid { get; set; } = 0;
    public int? Docked { get; set; } = 0;
    public int? Landed { get; set; } = 0;
    public int? LandingGearDown { get; set; } = 0;
    public int? ShieldsUp { get; set; } = 0;
    public int? Supercruise { get; set; } = 0;
    public int? FlightAssistOff { get; set; } = 0;
    public int? HardpointsDeployed { get; set; } = 0;
    public int? InWing { get; set; } = 0;
    public int? LightsOn { get; set; } = 0;
    public int? CargoScoopDeployed { get; set; } = 0;
    public int? SilentRunning { get; set; } = 0;
    public int? ScoopingFuel { get; set; } = 0;
    public int? SRVHandbrake { get; set; } = 0;
    public int? SRVTurret { get; set; } = 0;
    public int? SRVTurretRetracted { get; set; } = 0;
    public int? SRVDriveAssist { get; set; } = 0;
    public int? FSDMassLocked { get; set; } = 0;
    public int? FSDCharging { get; set; } = 0;
    public int? FSDCooldown { get; set; } = 0;
    public int? LowFuel { get; set; } = 0;
    public int? OverHeating { get; set; } = 0;
    public int? HasLatLong { get; set; } = 0;
    public int? IsInDanger { get; set; } = 0;
    public int? BeingInterdicted { get; set; } = 0;
    public int? InMainShip { get; set; } = 0;
    public int? InFighter { get; set; } = 0;
    public int? InSRV { get; set; } = 0;
    public int? HudInAnalysisMode { get; set; } = 0;
    public int? NightVision { get; set; } = 0;
    public int? AltitudeFromAverageRadius { get; set; } = 0;
    public int? FSDJump { get; set; } = 0;
    public int? SRVHighBeam { get; set; } = 0;

    public void parseRawFlags () {
        var ED_Docked = 0x00000001;
        var ED_Landed = 0x00000002;
        var ED_LandingGearDown = 0x00000004;
        var ED_ShieldsUp = 0x00000008;
        var ED_Supercruise = 0x00000010;
        var ED_FlightAssistOff = 0x00000020;
        var ED_HardpointsDeployed = 0x00000040;
        var ED_InWing = 0x00000080;

        var ED_LightsOn = 0x00000100;
        var ED_CargoScoopDeployed = 0x00000200;
        var ED_SilentRunning = 0x00000400;
        var ED_ScoopingFuel = 0x00000800;
        var ED_SRVHandbrake = 0x00001000;
        var ED_SRVTurret = 0x00002000;
        var ED_SRVTurretRetracted = 0x00004000;
        var ED_SRVDriveAssist = 0x00008000;

        var ED_FSDMassLocked = 0x00010000;
        var ED_FSDCharging = 0x00020000;
        var ED_FSDCooldown = 0x00040000;
        var ED_LowFuel = 0x00080000;
        var ED_OverHeating = 0x00100000;
        var ED_HasLatLong = 0x00200000;
        var ED_IsInDanger = 0x00400000;
        var ED_BeingInterdicted = 0x00800000;

        var ED_InMainShip = 0x01000000;
        var ED_InFighter = 0x02000000;
        var ED_InSRV = 0x04000000;
        var ED_HudInAnalysisMode = 0x08000000;
        var ED_NightVision = 0x10000000;

        var ED_AltitudeFromAverageRadius = 0x20000000;
        var ED_FSDJump = 0x40000000;
        var ED_SRVHighBeam = 0x80000000;

        this.FlagsValid = (this.Flags != 0 ? 1 : 0);
        this.Docked = (this.Flags & ED_Docked) != 0 ? 1 : 0;
        this.Landed = (this.Flags & ED_Landed) != 0 ? 1 : 0;
        this.LandingGearDown = (this.Flags & ED_LandingGearDown) != 0 ? 1 : 0;
        this.ShieldsUp = (this.Flags & ED_ShieldsUp) != 0 ? 1 : 0;
        this.Supercruise = (this.Flags & ED_Supercruise) != 0 ? 1 : 0;
        this.FlightAssistOff = (this.Flags & ED_FlightAssistOff) != 0 ? 1 : 0;
        this.HardpointsDeployed = (this.Flags & ED_HardpointsDeployed) != 0 ? 1 : 0;
        this.InWing = (this.Flags & ED_InWing) != 0 ? 1 : 0;
        this.LightsOn = (this.Flags & ED_LightsOn) != 0 ? 1 : 0;
        this.CargoScoopDeployed = (this.Flags & ED_CargoScoopDeployed) != 0 ? 1 : 0;
        this.SilentRunning = (this.Flags & ED_SilentRunning) != 0 ? 1 : 0;
        this.ScoopingFuel = (this.Flags & ED_ScoopingFuel) != 0 ? 1 : 0;
        this.SRVHandbrake = (this.Flags & ED_SRVHandbrake) != 0 ? 1 : 0;
        this.SRVTurret = (this.Flags & ED_SRVTurret) != 0 ? 1 : 0;
        this.SRVTurretRetracted = (this.Flags & ED_SRVTurretRetracted) != 0 ? 1 : 0;
        this.SRVDriveAssist = (this.Flags & ED_SRVDriveAssist) != 0 ? 1 : 0;
        this.FSDMassLocked = (this.Flags & ED_FSDMassLocked) != 0 ? 1 : 0;
        this.FSDCharging = (this.Flags & ED_FSDCharging) != 0 ? 1 : 0;
        this.FSDCooldown = (this.Flags & ED_FSDCooldown) != 0 ? 1 : 0;
        this.LowFuel = (this.Flags & ED_LowFuel) != 0 ? 1 : 0;
        this.OverHeating = (this.Flags & ED_OverHeating) != 0 ? 1 : 0;
        this.HasLatLong = (this.Flags & ED_HasLatLong) != 0 ? 1 : 0;
        this.IsInDanger = (this.Flags & ED_IsInDanger) != 0 ? 1 : 0;
        this.BeingInterdicted = (this.Flags & ED_BeingInterdicted) != 0 ? 1 : 0;
        this.InMainShip = (this.Flags & ED_InMainShip) != 0 ? 1 : 0;
        this.InFighter = (this.Flags & ED_InFighter) != 0 ? 1 : 0;
        this.InSRV = (this.Flags & ED_InSRV) != 0 ? 1 : 0;
        this.HudInAnalysisMode = (this.Flags & ED_HudInAnalysisMode) != 0 ? 1 : 0;
        this.NightVision = (this.Flags & ED_NightVision) != 0 ? 1 : 0;
        this.AltitudeFromAverageRadius = (this.Flags & ED_AltitudeFromAverageRadius) != 0 ? 1 : 0;
        this.FSDJump = (this.Flags & ED_FSDJump) != 0 ? 1 : 0;
        this.SRVHighBeam = (this.Flags & ED_SRVHighBeam) != 0 ? 1 : 0;
    }

    public void updateAllIntProperties (IStatusTranslator statusTranslator)
    {
        PropertyInfo[] properties = typeof(EliteStatusFile).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            // currently simply use all the ints available
            if (property.PropertyType == typeof(int?))
            {
                statusTranslator.FromStatusFile(property.Name, property.GetValue(this, null));
            }
        }
    }
}