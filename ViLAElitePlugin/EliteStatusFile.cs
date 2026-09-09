using System.Reflection;
using Newtonsoft.Json;

public class EliteStatusFile
{
    private const string ExposedPrefix = "Exposed";

    // Every property prefixed with "Exposed" is sent to ViLA under its name without the prefix.
    // Status.json changes about once a second, so the list is built once instead of per update.
    private static readonly (string Id, PropertyInfo Property)[] ExposedProperties = typeof(EliteStatusFile)
        .GetProperties()
        .Where(property => property.Name.StartsWith(ExposedPrefix) && property.Name != nameof(ExposedGameStarted))
        .Select(property => (IdFor(property.Name), property))
        .ToArray();

    private static readonly string GameStartedId = IdFor(nameof(ExposedGameStarted));

    // Raw values of the status file
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Event { get; set; } = "";

    // Flags is a 32 bit mask, so it does not fit into an int once the highest bit (SRVHighBeam) is set
    public long Flags { get; set; } = 0;
    public long Flags2 { get; set; } = 0;
    public List<int>? Pips { get; set; } = new List<int>();
    public int? FireGroup { get; set; } = 0;
    public Dictionary<string, float>?  Fuel { get; set; } = new Dictionary<string, float> {{"FuelMain", 0}, {"FuelReservoir", 0}};
    public int? GuiFocus { get; set; } = 0;
    public float? Latitude { get; set; } = 0;
    public float? Longitude { get; set; } = 0;
    public int? Heading { get; set; } = 0;

    // read as a double: the value is large (meters, from the average radius when far away) and
    // nothing promises it stays a whole number
    public double? Altitude { get; set; } = 0;
    public float? Cargo { get; set; } = 0;
    public string? LegalState { get; set; } = "";

    // Credits can easily exceed the int range
    public long? Balance { get; set; } = 0;
    public Dictionary<string, string>?  Destination { get; set; } = new Dictionary<string, string> {{"System", ""}, {"Body", ""}, {"Name", ""}};

    // On foot values (Odyssey), only written while on foot
    public float? Oxygen { get; set; } = 0;
    public float? Health { get; set; } = 0;
    public float? Temperature { get; set; } = 0;
    public string? SelectedWeapon { get; set; } = "";

    // not documented by Frontier, but written whenever the weapon has a readable name
    [JsonProperty("SelectedWeapon_Localised")]
    public string? SelectedWeaponLocalised { get; set; } = "";
    public float? Gravity { get; set; } = 0;
    public string? BodyName { get; set; } = "";
    public double? PlanetRadius { get; set; } = 0;

    // Calculated / expose values
    public bool ExposedGameStarted { get; set; } = false;

    // Flags
    public bool ExposedDocked { get; set; } = false;
    public bool ExposedLanded { get; set; } = false;
    public bool ExposedLandingGearDown { get; set; } = false;
    public bool ExposedShieldsUp { get; set; } = false;
    public bool ExposedSupercruise { get; set; } = false;
    public bool ExposedFlightAssistOff { get; set; } = false;
    public bool ExposedHardpointsDeployed { get; set; } = false;
    public bool ExposedInWing { get; set; } = false;
    public bool ExposedLightsOn { get; set; } = false;
    public bool ExposedCargoScoopDeployed { get; set; } = false;
    public bool ExposedSilentRunning { get; set; } = false;
    public bool ExposedScoopingFuel { get; set; } = false;
    public bool ExposedSRVHandbrake { get; set; } = false;
    public bool ExposedSRVTurret { get; set; } = false;
    public bool ExposedSRVTurretRetracted { get; set; } = false;
    public bool ExposedSRVDriveAssist { get; set; } = false;
    public bool ExposedFSDMassLocked { get; set; } = false;
    public bool ExposedFSDCharging { get; set; } = false;
    public bool ExposedFSDCooldown { get; set; } = false;
    public bool ExposedLowFuel { get; set; } = false;
    public bool ExposedOverHeating { get; set; } = false;
    public bool ExposedHasLatLong { get; set; } = false;
    public bool ExposedIsInDanger { get; set; } = false;
    public bool ExposedBeingInterdicted { get; set; } = false;
    public bool ExposedInMainShip { get; set; } = false;
    public bool ExposedInFighter { get; set; } = false;
    public bool ExposedInSRV { get; set; } = false;
    public bool ExposedHudInAnalysisMode { get; set; } = false;
    public bool ExposedNightVision { get; set; } = false;
    public bool ExposedAltitudeFromAverageRadius { get; set; } = false;
    public bool ExposedFSDJump { get; set; } = false;
    public bool ExposedSRVHighBeam { get; set; } = false;

    // Flags2 (Odyssey, on foot)
    public bool ExposedOnFoot { get; set; } = false;
    public bool ExposedInTaxi { get; set; } = false;
    public bool ExposedInMultiCrew { get; set; } = false;
    public bool ExposedOnFootInStation { get; set; } = false;
    public bool ExposedOnFootOnPlanet { get; set; } = false;
    public bool ExposedAimDownSight { get; set; } = false;
    public bool ExposedLowOxygen { get; set; } = false;
    public bool ExposedLowHealth { get; set; } = false;
    public bool ExposedCold { get; set; } = false;
    public bool ExposedHot { get; set; } = false;
    public bool ExposedVeryCold { get; set; } = false;
    public bool ExposedVeryHot { get; set; } = false;
    public bool ExposedGlideMode { get; set; } = false;
    public bool ExposedOnFootInHangar { get; set; } = false;
    public bool ExposedOnFootSocialSpace { get; set; } = false;
    public bool ExposedOnFootExterior { get; set; } = false;
    public bool ExposedBreathableAtmosphere { get; set; } = false;
    public bool ExposedTelepresenceMultiCrew { get; set; } = false;
    public bool ExposedPhysicalMultiCrew { get; set; } = false;
    public bool ExposedFSDHyperdriveCharging { get; set; } = false;
    public bool ExposedSupercruiseOverdrive { get; set; } = false;
    public bool ExposedSupercruiseAssist { get; set; } = false;
    public bool ExposedNPCCrewActive { get; set; } = false;

    // Other
    public int ExposedSysPips { get; set; } = 0;
    public int ExposedEngPips { get; set; } = 0;
    public int ExposedWepPips { get; set; } = 0;
    public int ExposedFireGroup { get; set; } = 0;
    public int ExposedGuiFocus { get; set; } = 0;
    public float ExposedFuelMain { get; set; } = 0;
    public float ExposedFuelReservoir { get; set; } = 0;
    public float ExposedCargo { get; set; } = 0;
    public string ExposedLegalState { get; set; } = "";
    public long ExposedBalance { get; set; } = 0;
    public string ExposedDestinationSystem { get; set; } = "";
    public string ExposedDestinationBody { get; set; } = "";
    public string ExposedDestinationName { get; set; } = "";
    public string ExposedDestinationNameLocalised { get; set; } = "";
    public float ExposedLatitude { get; set; } = 0;
    public float ExposedLongitude { get; set; } = 0;
    public int ExposedHeading { get; set; } = 0;
    public double ExposedAltitude { get; set; } = 0;

    // On foot (Odyssey)
    public float ExposedOxygen { get; set; } = 0;
    public float ExposedHealth { get; set; } = 0;
    public float ExposedTemperature { get; set; } = 0;
    public float ExposedGravity { get; set; } = 0;
    public string ExposedSelectedWeapon { get; set; } = "";
    public string ExposedSelectedWeaponLocalised { get; set; } = "";
    public string ExposedBodyName { get; set; } = "";
    public double ExposedPlanetRadius { get; set; } = 0;

    public void ParseFlags ()
    {
        var flags = (StatusFlags) this.Flags;
        var flags2 = (StatusFlags2) this.Flags2;

        // Both masks are 0 while the game is not in play. Flags alone is not enough: on foot none
        // of the ship states apply, so only Flags2 is set.
        this.ExposedGameStarted = flags != StatusFlags.None || flags2 != StatusFlags2.None;

        this.ExposedDocked = flags.HasFlag(StatusFlags.Docked);
        this.ExposedLanded = flags.HasFlag(StatusFlags.Landed);
        this.ExposedLandingGearDown = flags.HasFlag(StatusFlags.LandingGearDown);
        this.ExposedShieldsUp = flags.HasFlag(StatusFlags.ShieldsUp);
        this.ExposedSupercruise = flags.HasFlag(StatusFlags.Supercruise);
        this.ExposedFlightAssistOff = flags.HasFlag(StatusFlags.FlightAssistOff);
        this.ExposedHardpointsDeployed = flags.HasFlag(StatusFlags.HardpointsDeployed);
        this.ExposedInWing = flags.HasFlag(StatusFlags.InWing);
        this.ExposedLightsOn = flags.HasFlag(StatusFlags.LightsOn);
        this.ExposedCargoScoopDeployed = flags.HasFlag(StatusFlags.CargoScoopDeployed);
        this.ExposedSilentRunning = flags.HasFlag(StatusFlags.SilentRunning);
        this.ExposedScoopingFuel = flags.HasFlag(StatusFlags.ScoopingFuel);
        this.ExposedSRVHandbrake = flags.HasFlag(StatusFlags.SRVHandbrake);
        this.ExposedSRVTurret = flags.HasFlag(StatusFlags.SRVTurret);
        this.ExposedSRVTurretRetracted = flags.HasFlag(StatusFlags.SRVTurretRetracted);
        this.ExposedSRVDriveAssist = flags.HasFlag(StatusFlags.SRVDriveAssist);
        this.ExposedFSDMassLocked = flags.HasFlag(StatusFlags.FSDMassLocked);
        this.ExposedFSDCharging = flags.HasFlag(StatusFlags.FSDCharging);
        this.ExposedFSDCooldown = flags.HasFlag(StatusFlags.FSDCooldown);
        this.ExposedLowFuel = flags.HasFlag(StatusFlags.LowFuel);
        this.ExposedOverHeating = flags.HasFlag(StatusFlags.OverHeating);
        this.ExposedHasLatLong = flags.HasFlag(StatusFlags.HasLatLong);
        this.ExposedIsInDanger = flags.HasFlag(StatusFlags.IsInDanger);
        this.ExposedBeingInterdicted = flags.HasFlag(StatusFlags.BeingInterdicted);
        this.ExposedInMainShip = flags.HasFlag(StatusFlags.InMainShip);
        this.ExposedInFighter = flags.HasFlag(StatusFlags.InFighter);
        this.ExposedInSRV = flags.HasFlag(StatusFlags.InSRV);
        this.ExposedHudInAnalysisMode = flags.HasFlag(StatusFlags.HudInAnalysisMode);
        this.ExposedNightVision = flags.HasFlag(StatusFlags.NightVision);
        this.ExposedAltitudeFromAverageRadius = flags.HasFlag(StatusFlags.AltitudeFromAverageRadius);
        this.ExposedFSDJump = flags.HasFlag(StatusFlags.FSDJump);
        this.ExposedSRVHighBeam = flags.HasFlag(StatusFlags.SRVHighBeam);

        this.ExposedOnFoot = flags2.HasFlag(StatusFlags2.OnFoot);
        this.ExposedInTaxi = flags2.HasFlag(StatusFlags2.InTaxi);
        this.ExposedInMultiCrew = flags2.HasFlag(StatusFlags2.InMultiCrew);
        this.ExposedOnFootInStation = flags2.HasFlag(StatusFlags2.OnFootInStation);
        this.ExposedOnFootOnPlanet = flags2.HasFlag(StatusFlags2.OnFootOnPlanet);
        this.ExposedAimDownSight = flags2.HasFlag(StatusFlags2.AimDownSight);
        this.ExposedLowOxygen = flags2.HasFlag(StatusFlags2.LowOxygen);
        this.ExposedLowHealth = flags2.HasFlag(StatusFlags2.LowHealth);
        this.ExposedCold = flags2.HasFlag(StatusFlags2.Cold);
        this.ExposedHot = flags2.HasFlag(StatusFlags2.Hot);
        this.ExposedVeryCold = flags2.HasFlag(StatusFlags2.VeryCold);
        this.ExposedVeryHot = flags2.HasFlag(StatusFlags2.VeryHot);
        this.ExposedGlideMode = flags2.HasFlag(StatusFlags2.GlideMode);
        this.ExposedOnFootInHangar = flags2.HasFlag(StatusFlags2.OnFootInHangar);
        this.ExposedOnFootSocialSpace = flags2.HasFlag(StatusFlags2.OnFootSocialSpace);
        this.ExposedOnFootExterior = flags2.HasFlag(StatusFlags2.OnFootExterior);
        this.ExposedBreathableAtmosphere = flags2.HasFlag(StatusFlags2.BreathableAtmosphere);
        this.ExposedTelepresenceMultiCrew = flags2.HasFlag(StatusFlags2.TelepresenceMultiCrew);
        this.ExposedPhysicalMultiCrew = flags2.HasFlag(StatusFlags2.PhysicalMultiCrew);
        this.ExposedFSDHyperdriveCharging = flags2.HasFlag(StatusFlags2.FSDHyperdriveCharging);
        this.ExposedSupercruiseOverdrive = flags2.HasFlag(StatusFlags2.SupercruiseOverdrive);
        this.ExposedSupercruiseAssist = flags2.HasFlag(StatusFlags2.SupercruiseAssist);
        this.ExposedNPCCrewActive = flags2.HasFlag(StatusFlags2.NPCCrewActive);
    }

    public void ParseVariables ()
    {
        // Simple variables
        this.ExposedFireGroup = this.FireGroup.GetValueOrDefault(0);
        this.ExposedGuiFocus = this.GuiFocus.GetValueOrDefault(0);
        this.ExposedCargo = this.Cargo.GetValueOrDefault(0);
        this.ExposedBalance = this.Balance.GetValueOrDefault(0);
        this.ExposedLegalState = this.LegalState != null ? this.LegalState : "";
        this.ExposedLatitude = this.Latitude.GetValueOrDefault(0);
        this.ExposedLongitude = this.Longitude.GetValueOrDefault(0);
        this.ExposedAltitude = this.Altitude.GetValueOrDefault(0);
        this.ExposedOxygen = this.Oxygen.GetValueOrDefault(0);
        this.ExposedHealth = this.Health.GetValueOrDefault(0);
        this.ExposedTemperature = this.Temperature.GetValueOrDefault(0);
        this.ExposedGravity = this.Gravity.GetValueOrDefault(0);
        this.ExposedSelectedWeapon = this.SelectedWeapon != null ? this.SelectedWeapon : "";
        this.ExposedSelectedWeaponLocalised = this.SelectedWeaponLocalised != null ? this.SelectedWeaponLocalised : "";
        this.ExposedBodyName = this.BodyName != null ? this.BodyName : "";
        this.ExposedPlanetRadius = this.PlanetRadius.GetValueOrDefault(0);

        // on foot the game reports -180 to 180, in a ship 0 to 359 - always report 0 to 359
        this.ExposedHeading = ((this.Heading.GetValueOrDefault(0) % 360) + 360) % 360;

        // Variables that need parsing
        if (this.Pips != null && this.Pips.Count == 3) {
            this.ExposedSysPips = this.Pips[0];
            this.ExposedEngPips = this.Pips[1];
            this.ExposedWepPips = this.Pips[2];
        }

        if (this.Fuel != null)
        {
            if (this.Fuel.ContainsKey("FuelMain"))
            {
                this.ExposedFuelMain = this.Fuel["FuelMain"];
            }
            if (this.Fuel.ContainsKey("FuelReservoir"))
            {
                this.ExposedFuelReservoir = this.Fuel["FuelReservoir"];
            }
        }

        if (this.Destination != null)
        {
            if (this.Destination.ContainsKey("System"))
            {
                this.ExposedDestinationSystem = this.Destination["System"];
            }
            if (this.Destination.ContainsKey("Body"))
            {
                this.ExposedDestinationBody = this.Destination["Body"];
            }
            if (this.Destination.ContainsKey("Name"))
            {
                this.ExposedDestinationName = this.Destination["Name"];
            }
            if (this.Destination.ContainsKey("Name_Localised"))
            {
                this.ExposedDestinationNameLocalised = this.Destination["Name_Localised"];
            }
        }
    }

    /// <summary>
    /// Sends every exposed value to ViLA. The translator drops the ones that did not change.
    /// </summary>
    public void SendExposedValues (IStatusTranslator statusTranslator)
    {
        // Always sent, even with the game closed: it is the only way a configuration can react to
        // the game not running at all.
        statusTranslator.FromStatusFile(GameStartedId, this.ExposedGameStarted);

        // Everything else says nothing about the game once it is closed, the values are simply 0.
        if (!this.ExposedGameStarted)
        {
            return;
        }

        foreach ((string id, PropertyInfo property) in ExposedProperties)
        {
            statusTranslator.FromStatusFile(id, property.GetValue(this, null));
        }
    }

    private static string IdFor (string propertyName) => propertyName.Substring(ExposedPrefix.Length);
}
