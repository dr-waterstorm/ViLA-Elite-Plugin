namespace ViLAElitePluginTests;

/// <summary>
/// Status.json samples. The numbers are the interesting part: Balance and Flags both used to
/// overflow an int, which stopped the whole file from being parsed.
/// </summary>
public static class StatusFiles
{
    /// <summary>
    /// Docked in a ship, with a credit balance above int.MaxValue and bit 31 (SRVHighBeam) set.
    /// Flags 2147483649 == 0x80000001.
    /// </summary>
    public const string DockedWithLargeBalance = @"{
        ""timestamp"": ""2026-09-09T09:12:00Z"",
        ""event"": ""Status"",
        ""Flags"": 2147483649,
        ""Flags2"": 0,
        ""Pips"": [ 4, 8, 0 ],
        ""FireGroup"": 2,
        ""GuiFocus"": 0,
        ""Fuel"": { ""FuelMain"": 25.6, ""FuelReservoir"": 0.42 },
        ""Cargo"": 12.5,
        ""LegalState"": ""Clean"",
        ""Balance"": 4648069815,
        ""Destination"": { ""System"": 3932277478106, ""Body"": 8, ""Name"": ""Jameson Memorial"" }
    }";

    /// <summary>
    /// Flying over a planet surface, so the position values are filled in.
    /// Flags 18874368 == InMainShip | HasLatLong.
    /// </summary>
    public const string FlyingOverPlanet = @"{
        ""timestamp"": ""2026-09-09T09:13:00Z"",
        ""event"": ""Status"",
        ""Flags"": 18874368,
        ""Flags2"": 0,
        ""Pips"": [ 2, 8, 2 ],
        ""FireGroup"": 0,
        ""GuiFocus"": 0,
        ""Fuel"": { ""FuelMain"": 15.25, ""FuelReservoir"": 0.31 },
        ""Cargo"": 0.0,
        ""LegalState"": ""Clean"",
        ""Latitude"": -12.5834,
        ""Longitude"": 42.1234,
        ""Heading"": 273,
        ""Altitude"": 2500,
        ""BodyName"": ""Nervi 2 a"",
        ""PlanetRadius"": 1206724.5,
        ""Balance"": 1000,
        ""Destination"": { ""System"": 0, ""Body"": 0, ""Name"": """" }
    }";

    /// <summary>
    /// On foot in a station concourse. Flags is 0 here - none of the ship states apply - so only
    /// Flags2 tells us the game is running at all.
    /// Flags2 81929 == OnFoot | OnFootInStation | OnFootSocialSpace | BreathableAtmosphere.
    /// </summary>
    public const string OnFootInStation = @"{
        ""timestamp"": ""2026-09-09T09:14:00Z"",
        ""event"": ""Status"",
        ""Flags"": 0,
        ""Flags2"": 81929,
        ""Oxygen"": 1.0,
        ""Health"": 0.85,
        ""Temperature"": 293.15,
        ""SelectedWeapon"": ""$humanoid_fists_name;"",
        ""Gravity"": 0.16,
        ""LegalState"": ""Clean"",
        ""BodyName"": ""Jameson Memorial"",
        ""Balance"": 4648069815,
        ""Destination"": { ""System"": 0, ""Body"": 0, ""Name"": """" }
    }";

    /// <summary>
    /// On foot on a planet surface. Captured from a real game (via EDDI's test data), which is why
    /// Heading is negative and SelectedWeapon_Localised shows up - neither is in Frontier's docs.
    /// Flags2 273 == OnFoot | OnFootOnPlanet | Cold, Flags 2097152 == HasLatLong.
    /// </summary>
    public const string OnFootOnPlanet =
        @"{""timestamp"":""2021-05-01T21:30:38Z"",""event"":""Status"",""Flags"":2097152,""Flags2"":273,"
        + @"""Oxygen"":1.0,""Health"":1.0,""Temperature"":163.5271,""SelectedWeapon"":""$humanoid_fists_name;"","
        + @"""SelectedWeapon_Localised"":""Unarmed"",""Gravity"":0.101595,""LegalState"":""Clean"","
        + @"""Latitude"":40.741016,""Longitude"":65.076881,""Heading"":-165,""BodyName"":""Nervi 2 a""}";

    /// <summary>
    /// What the game writes once it is closed / sitting in the main menu - Flags is 0 and there is
    /// no Flags2 key at all.
    /// </summary>
    public const string GameNotRunning =
        @"{""timestamp"":""2025-10-30T20:54:38Z"",""event"":""Status"",""Flags"":0}";
}
