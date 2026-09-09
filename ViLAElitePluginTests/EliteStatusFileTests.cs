using Newtonsoft.Json;
using Xunit;

namespace ViLAElitePluginTests;

public class EliteStatusFileTests
{
    private static EliteStatusFile Parse(string json)
    {
        var statusFile = JsonConvert.DeserializeObject<EliteStatusFile>(json)!;
        statusFile.ParseFlags();
        statusFile.ParseVariables();

        return statusFile;
    }

    // ---- regressions: values that do not fit into an int used to break the whole file ----

    [Fact]
    public void Balance_above_int_max_is_parsed()
    {
        Assert.Equal(4648069815L, Parse(StatusFiles.DockedWithLargeBalance).ExposedBalance);
    }

    [Fact]
    public void Flags_with_bit_31_set_is_parsed()
    {
        var status = Parse(StatusFiles.DockedWithLargeBalance);

        Assert.True(status.ExposedSRVHighBeam, "bit 31 (SRVHighBeam) should be set");
        Assert.True(status.ExposedDocked, "bit 0 (Docked) should be set");
        Assert.False(status.ExposedShieldsUp);
    }

    [Theory]
    [InlineData(@"{ ""Flags"": 2097152, ""Altitude"": 3000000000 }", 3000000000d)]
    [InlineData(@"{ ""Flags"": 2097152, ""Altitude"": 2500.75 }", 2500.75d)]
    public void Altitude_is_read_as_a_number_of_any_size(string json, double expected)
    {
        // an int property would throw on both of these and take the whole file with it
        Assert.Equal(expected, Parse(json).ExposedAltitude, 3);
    }

    // ---- flags ----

    [Theory]
    [InlineData(0, 0, false)]                 // game closed / main menu
    [InlineData(16777216, 0, true)]           // in the ship
    [InlineData(0, 1, true)]                  // on foot: only Flags2 is set
    [InlineData(0, 81929, true)]              // on foot in a concourse
    public void GameStarted_covers_ship_and_on_foot(long flags, long flags2, bool expected)
    {
        var status = new EliteStatusFile { Flags = flags, Flags2 = flags2 };
        status.ParseFlags();

        Assert.Equal(expected, status.ExposedGameStarted);
    }

    [Fact]
    public void Flags2_on_foot_states_are_parsed()
    {
        var status = Parse(StatusFiles.OnFootInStation);

        Assert.True(status.ExposedOnFoot);
        Assert.True(status.ExposedOnFootInStation);
        Assert.True(status.ExposedOnFootSocialSpace);
        Assert.True(status.ExposedBreathableAtmosphere);
        Assert.False(status.ExposedOnFootOnPlanet);
        Assert.False(status.ExposedLowOxygen);
        Assert.True(status.ExposedGameStarted);
    }

    [Fact]
    public void Ship_flags_are_parsed()
    {
        var status = Parse(StatusFiles.FlyingOverPlanet);

        Assert.True(status.ExposedInMainShip);
        Assert.True(status.ExposedHasLatLong);
        Assert.False(status.ExposedLanded);
        Assert.False(status.ExposedSRVHighBeam);
    }

    // ---- values ----

    [Fact]
    public void Ship_values_are_parsed()
    {
        var status = Parse(StatusFiles.DockedWithLargeBalance);

        Assert.Equal(4, status.ExposedSysPips);
        Assert.Equal(8, status.ExposedEngPips);
        Assert.Equal(0, status.ExposedWepPips);
        Assert.Equal(2, status.ExposedFireGroup);
        Assert.Equal(25.6f, status.ExposedFuelMain, 0.001f);
        Assert.Equal(0.42f, status.ExposedFuelReservoir, 0.001f);
        Assert.Equal(12.5f, status.ExposedCargo, 0.001f);
        Assert.Equal("Clean", status.ExposedLegalState);
        Assert.Equal("Jameson Memorial", status.ExposedDestinationName);
        Assert.Equal("3932277478106", status.ExposedDestinationSystem);
        Assert.Equal("8", status.ExposedDestinationBody);
    }

    [Fact]
    public void Position_values_are_parsed()
    {
        var status = Parse(StatusFiles.FlyingOverPlanet);

        Assert.Equal(-12.5834f, status.ExposedLatitude, 0.0001f);
        Assert.Equal(42.1234f, status.ExposedLongitude, 0.0001f);
        Assert.Equal(273, status.ExposedHeading);
        Assert.Equal(2500d, status.ExposedAltitude, 3);
        Assert.Equal("Nervi 2 a", status.ExposedBodyName);
        Assert.Equal(1206724.5d, status.ExposedPlanetRadius, 1);
    }

    [Fact]
    public void On_foot_values_are_parsed()
    {
        var status = Parse(StatusFiles.OnFootInStation);

        Assert.Equal(1.0f, status.ExposedOxygen, 0.001f);
        Assert.Equal(0.85f, status.ExposedHealth, 0.001f);
        Assert.Equal(293.15f, status.ExposedTemperature, 0.01f);
        Assert.Equal(0.16f, status.ExposedGravity, 0.001f);
        Assert.Equal("$humanoid_fists_name;", status.ExposedSelectedWeapon);
    }

    [Fact]
    public void On_foot_on_a_planet_is_parsed_from_a_real_capture()
    {
        var status = Parse(StatusFiles.OnFootOnPlanet);

        Assert.True(status.ExposedGameStarted);
        Assert.True(status.ExposedOnFoot);
        Assert.True(status.ExposedOnFootOnPlanet);
        Assert.True(status.ExposedCold);
        Assert.True(status.ExposedHasLatLong);
        Assert.False(status.ExposedOnFootInStation);

        Assert.Equal("Unarmed", status.ExposedSelectedWeaponLocalised);
        Assert.Equal("$humanoid_fists_name;", status.ExposedSelectedWeapon);
        Assert.Equal(163.5271f, status.ExposedTemperature, 0.001f);
        Assert.Equal(0.101595f, status.ExposedGravity, 0.000001f);
        Assert.Equal("Nervi 2 a", status.ExposedBodyName);
    }

    [Theory]
    [InlineData(-165, 195)]   // on foot the game reports -180 to 180
    [InlineData(273, 273)]
    [InlineData(-1, 359)]
    [InlineData(360, 0)]
    public void Heading_is_normalised_to_0_359(int reported, int expected)
    {
        var status = new EliteStatusFile { Heading = reported };
        status.ParseVariables();

        Assert.Equal(expected, status.ExposedHeading);
    }

    [Fact]
    public void Supercruise_and_crew_flags_are_parsed()
    {
        // Flags2 bit 20 (SCO), bit 21 (Supercruise Assist), bit 22 (NPC crew on duty)
        var status = new EliteStatusFile { Flags2 = (1L << 20) | (1L << 22) };
        status.ParseFlags();

        Assert.True(status.ExposedSupercruiseOverdrive);
        Assert.False(status.ExposedSupercruiseAssist);
        Assert.True(status.ExposedNPCCrewActive);
    }

    [Fact]
    public void Localised_destination_name_is_parsed()
    {
        var json = @"{ ""Flags"": 16777216, ""Destination"": { ""System"": 3932277478106, ""Body"": 0,"
            + @" ""Name"": ""$MULTIPLAYER_SCENARIO42_TITLE;"", ""Name_Localised"": ""Nav Beacon"" } }";

        var status = Parse(json);

        Assert.Equal("$MULTIPLAYER_SCENARIO42_TITLE;", status.ExposedDestinationName);
        Assert.Equal("Nav Beacon", status.ExposedDestinationNameLocalised);
    }

    [Fact]
    public void Missing_values_fall_back_to_defaults()
    {
        var status = Parse(StatusFiles.GameNotRunning);

        Assert.Equal(0L, status.ExposedBalance);
        Assert.Equal(0, status.ExposedFireGroup);
        Assert.Equal("", status.ExposedLegalState);
        Assert.Equal("", status.ExposedBodyName);
        Assert.Equal("", status.ExposedDestinationName);
    }

    // ---- what actually gets sent ----

    [Fact]
    public void Exposed_values_are_sent_without_the_prefix()
    {
        var translator = new RecordingTranslator();
        Parse(StatusFiles.DockedWithLargeBalance).SendExposedValues(translator);

        Assert.DoesNotContain(translator.SentIds, id => id.StartsWith("Exposed"));
        Assert.Contains("GameStarted", translator.SentIds);
        Assert.Contains("Balance", translator.SentIds);
        Assert.Contains("SRVHighBeam", translator.SentIds);
        Assert.Contains("OnFoot", translator.SentIds);
        Assert.Contains("Oxygen", translator.SentIds);
        Assert.Equal(4648069815L, translator.ValueOf("Balance"));
    }

    [Fact]
    public void Every_id_is_sent_only_once_per_update()
    {
        var translator = new RecordingTranslator();
        Parse(StatusFiles.DockedWithLargeBalance).SendExposedValues(translator);

        Assert.Equal(translator.SentIds.Distinct().Count(), translator.Sent.Count);
    }

    [Fact]
    public void Closed_game_only_sends_GameStarted()
    {
        var translator = new RecordingTranslator();
        Parse(StatusFiles.GameNotRunning).SendExposedValues(translator);

        // the other values are all 0 / empty and say nothing about the game, but a configuration
        // has to be able to react to the game being gone
        var sent = Assert.Single(translator.Sent);
        Assert.Equal("GameStarted", sent.Key);
        Assert.Equal(false, sent.Value);
    }

    [Fact]
    public void Running_game_sends_GameStarted_as_true()
    {
        var translator = new RecordingTranslator();
        Parse(StatusFiles.OnFootInStation).SendExposedValues(translator);

        Assert.Equal(true, translator.ValueOf("GameStarted"));
    }
}
