/// <summary>
/// The "Flags" bit mask of Status.json - the ship / SRV / fighter states.
/// </summary>
[Flags]
public enum StatusFlags : long
{
    None = 0,
    Docked = 1L << 0,
    Landed = 1L << 1,
    LandingGearDown = 1L << 2,
    ShieldsUp = 1L << 3,
    Supercruise = 1L << 4,
    FlightAssistOff = 1L << 5,
    HardpointsDeployed = 1L << 6,
    InWing = 1L << 7,
    LightsOn = 1L << 8,
    CargoScoopDeployed = 1L << 9,
    SilentRunning = 1L << 10,
    ScoopingFuel = 1L << 11,
    SRVHandbrake = 1L << 12,
    SRVTurret = 1L << 13,
    SRVTurretRetracted = 1L << 14,
    SRVDriveAssist = 1L << 15,
    FSDMassLocked = 1L << 16,
    FSDCharging = 1L << 17,
    FSDCooldown = 1L << 18,
    LowFuel = 1L << 19,
    OverHeating = 1L << 20,
    HasLatLong = 1L << 21,
    IsInDanger = 1L << 22,
    BeingInterdicted = 1L << 23,
    InMainShip = 1L << 24,
    InFighter = 1L << 25,
    InSRV = 1L << 26,
    HudInAnalysisMode = 1L << 27,
    NightVision = 1L << 28,
    AltitudeFromAverageRadius = 1L << 29,
    FSDJump = 1L << 30,

    // does not fit into an int, which is why Flags is read as a long
    SRVHighBeam = 1L << 31,
}

/// <summary>
/// The "Flags2" bit mask of Status.json - the on foot states added with Odyssey.
/// </summary>
[Flags]
public enum StatusFlags2 : long
{
    None = 0,
    OnFoot = 1L << 0,
    InTaxi = 1L << 1,
    InMultiCrew = 1L << 2,
    OnFootInStation = 1L << 3,
    OnFootOnPlanet = 1L << 4,
    AimDownSight = 1L << 5,
    LowOxygen = 1L << 6,
    LowHealth = 1L << 7,
    Cold = 1L << 8,
    Hot = 1L << 9,
    VeryCold = 1L << 10,
    VeryHot = 1L << 11,
    GlideMode = 1L << 12,
    OnFootInHangar = 1L << 13,
    OnFootSocialSpace = 1L << 14,
    OnFootExterior = 1L << 15,
    BreathableAtmosphere = 1L << 16,
    TelepresenceMultiCrew = 1L << 17,
    PhysicalMultiCrew = 1L << 18,

    // set together with FSDCharging when the jump goes to another system, on its own the ship is
    // charging for supercruise
    FSDHyperdriveCharging = 1L << 19,
    SupercruiseOverdrive = 1L << 20,
    SupercruiseAssist = 1L << 21,

    // not in the Frontier documentation, but used by EDDI and EDDiscovery
    NPCCrewActive = 1L << 22,
}
