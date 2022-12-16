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
    public bool ExposedGameStarted { get; set; } = false;
    public bool? ExposedDocked { get; set; } = false;
    public bool? ExposedLanded { get; set; } = false;
    public bool? ExposedLandingGearDown { get; set; } = false;
    public bool? ExposedShieldsUp { get; set; } = false;
    public bool? ExposedSupercruise { get; set; } = false;
    public bool? ExposedFlightAssistOff { get; set; } = false;
    public bool? ExposedHardpointsDeployed { get; set; } = false;
    public bool? ExposedInWing { get; set; } = false;
    public bool? ExposedLightsOn { get; set; } = false;
    public bool? ExposedCargoScoopDeployed { get; set; } = false;
    public bool? ExposedSilentRunning { get; set; } = false;
    public bool? ExposedScoopingFuel { get; set; } = false;
    public bool? ExposedSRVHandbrake { get; set; } = false;
    public bool? ExposedSRVTurret { get; set; } = false;
    public bool? ExposedSRVTurretRetracted { get; set; } = false;
    public bool? ExposedSRVDriveAssist { get; set; } = false;
    public bool? ExposedFSDMassLocked { get; set; } = false;
    public bool? ExposedFSDCharging { get; set; } = false;
    public bool? ExposedFSDCooldown { get; set; } = false;
    public bool? ExposedLowFuel { get; set; } = false;
    public bool? ExposedOverHeating { get; set; } = false;
    public bool? ExposedHasLatLong { get; set; } = false;
    public bool? ExposedIsInDanger { get; set; } = false;
    public bool? ExposedBeingInterdicted { get; set; } = false;
    public bool? ExposedInMainShip { get; set; } = false;
    public bool? ExposedInFighter { get; set; } = false;
    public bool? ExposedInSRV { get; set; } = false;
    public bool? ExposedHudInAnalysisMode { get; set; } = false;
    public bool? ExposedNightVision { get; set; } = false;
    public bool? ExposedAltitudeFromAverageRadius { get; set; } = false;
    public bool? ExposedFSDJump { get; set; } = false;
    public bool? ExposedSRVHighBeam { get; set; } = false;

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

        this.ExposedGameStarted = (this.Flags != 0);
        this.ExposedDocked = (this.Flags & ED_Docked) != 0 ? true : false;
        this.ExposedLanded = (this.Flags & ED_Landed) != 0 ? true : false;
        this.ExposedLandingGearDown = (this.Flags & ED_LandingGearDown) != 0 ? true : false;
        this.ExposedShieldsUp = (this.Flags & ED_ShieldsUp) != 0 ? true : false;
        this.ExposedSupercruise = (this.Flags & ED_Supercruise) != 0 ? true : false;
        this.ExposedFlightAssistOff = (this.Flags & ED_FlightAssistOff) != 0 ? true : false;
        this.ExposedHardpointsDeployed = (this.Flags & ED_HardpointsDeployed) != 0 ? true : false;
        this.ExposedInWing = (this.Flags & ED_InWing) != 0 ? true : false;
        this.ExposedLightsOn = (this.Flags & ED_LightsOn) != 0 ? true : false;
        this.ExposedCargoScoopDeployed = (this.Flags & ED_CargoScoopDeployed) != 0 ? true : false;
        this.ExposedSilentRunning = (this.Flags & ED_SilentRunning) != 0 ? true : false;
        this.ExposedScoopingFuel = (this.Flags & ED_ScoopingFuel) != 0 ? true : false;
        this.ExposedSRVHandbrake = (this.Flags & ED_SRVHandbrake) != 0 ? true : false;
        this.ExposedSRVTurret = (this.Flags & ED_SRVTurret) != 0 ? true : false;
        this.ExposedSRVTurretRetracted = (this.Flags & ED_SRVTurretRetracted) != 0 ? true : false;
        this.ExposedSRVDriveAssist = (this.Flags & ED_SRVDriveAssist) != 0 ? true : false;
        this.ExposedFSDMassLocked = (this.Flags & ED_FSDMassLocked) != 0 ? true : false;
        this.ExposedFSDCharging = (this.Flags & ED_FSDCharging) != 0 ? true : false;
        this.ExposedFSDCooldown = (this.Flags & ED_FSDCooldown) != 0 ? true : false;
        this.ExposedLowFuel = (this.Flags & ED_LowFuel) != 0 ? true : false;
        this.ExposedOverHeating = (this.Flags & ED_OverHeating) != 0 ? true : false;
        this.ExposedHasLatLong = (this.Flags & ED_HasLatLong) != 0 ? true : false;
        this.ExposedIsInDanger = (this.Flags & ED_IsInDanger) != 0 ? true : false;
        this.ExposedBeingInterdicted = (this.Flags & ED_BeingInterdicted) != 0 ? true : false;
        this.ExposedInMainShip = (this.Flags & ED_InMainShip) != 0 ? true : false;
        this.ExposedInFighter = (this.Flags & ED_InFighter) != 0 ? true : false;
        this.ExposedInSRV = (this.Flags & ED_InSRV) != 0 ? true : false;
        this.ExposedHudInAnalysisMode = (this.Flags & ED_HudInAnalysisMode) != 0 ? true : false;
        this.ExposedNightVision = (this.Flags & ED_NightVision) != 0 ? true : false;
        this.ExposedAltitudeFromAverageRadius = (this.Flags & ED_AltitudeFromAverageRadius) != 0 ? true : false;
        this.ExposedFSDJump = (this.Flags & ED_FSDJump) != 0 ? true : false;
        this.ExposedSRVHighBeam = (this.Flags & ED_SRVHighBeam) != 0 ? true : false;
    }

    public void updateAllIntProperties (IStatusTranslator statusTranslator)
    {
        // only update if the game is running
        if (this.ExposedGameStarted)
        {
            PropertyInfo[] properties = typeof(EliteStatusFile).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                // currently simply use all the ints available
                if (property.Name.StartsWith("Exposed"))
                {
                    statusTranslator.FromStatusFile(property.Name.Remove(0,7), property.GetValue(this, null));
                }
            }
        }
    }
}