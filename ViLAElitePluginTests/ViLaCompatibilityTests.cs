using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ViLAElitePluginTests;

/// <summary>
/// ViLA stores whatever the plugin sends in a Dictionary&lt;string, dynamic&gt; and reads it back as
/// the type the configured trigger needs - DoubleTrigger does "State.TryGetValue(Id, out double
/// value)", which casts the stored dynamic to double. A value type the runtime binder cannot
/// convert would only blow up inside ViLA, at the moment somebody uses that variable in a
/// configuration, so the exposed types are checked here.
/// </summary>
public class ViLaCompatibilityTests
{
    /// <summary>Copy of ViLA's Core/State.cs (charliefoxtwo/ViLA).</summary>
    private class State : Dictionary<string, dynamic>
    {
        public bool TryGetValue<T>(string key, out T? value)
        {
            value = default;
            if (ContainsKey(key))
            {
                value = (T) this[key];
                return true;
            }

            return false;
        }
    }

    private static State StateFor(string json)
    {
        var statusFile = JsonConvert.DeserializeObject<EliteStatusFile>(json)!;
        statusFile.ParseFlags();
        statusFile.ParseVariables();

        var translator = new RecordingTranslator();
        statusFile.SendExposedValues(translator);

        var state = new State();
        foreach (var (id, value) in translator.Sent)
        {
            state[id] = value!;
        }

        return state;
    }

    [Theory]
    [InlineData("Balance", 4648069815d)]   // long
    [InlineData("FuelMain", 25.6d)]        // float
    [InlineData("SysPips", 4d)]            // int
    [InlineData("Cargo", 12.5d)]           // float
    public void Numbers_can_be_read_as_double(string id, double expected)
    {
        var state = StateFor(StatusFiles.DockedWithLargeBalance);

        Assert.True(state.TryGetValue<double>(id, out var value), $"{id} was never sent");
        Assert.Equal(expected, value, 3);
    }

    [Fact]
    public void Planet_radius_can_be_read_as_double()
    {
        var state = StateFor(StatusFiles.FlyingOverPlanet);

        Assert.True(state.TryGetValue<double>("PlanetRadius", out var value));
        Assert.Equal(1206724.5d, value, 1);
    }

    [Fact]
    public void Altitude_can_be_read_as_double()
    {
        var state = StateFor(StatusFiles.FlyingOverPlanet);

        Assert.True(state.TryGetValue<double>("Altitude", out var value));
        Assert.Equal(2500d, value, 3);
    }

    [Fact]
    public void Booleans_and_strings_survive_the_cast()
    {
        var state = StateFor(StatusFiles.DockedWithLargeBalance);

        Assert.True(state.TryGetValue<bool>("Docked", out var docked) && docked);
        Assert.True(state.TryGetValue<string>("LegalState", out var legalState));
        Assert.Equal("Clean", legalState);
    }

    [Fact]
    public void On_foot_values_survive_the_cast()
    {
        var state = StateFor(StatusFiles.OnFootInStation);

        Assert.True(state.TryGetValue<bool>("OnFoot", out var onFoot) && onFoot);
        Assert.True(state.TryGetValue<double>("Health", out var health));
        Assert.Equal(0.85d, health, 3);
        Assert.True(state.TryGetValue<string>("SelectedWeapon", out var weapon));
        Assert.Equal("$humanoid_fists_name;", weapon);
    }

    /// <summary>
    /// ViLA's Core/TriggerConverter reads a whole number from the configuration with
    /// JToken.Value&lt;int&gt;(), so a comparison value above int.MaxValue cannot be used even
    /// though Balance itself can be that big. This only pins the Newtonsoft behaviour ViLA relies
    /// on - it cannot notice a change on ViLA's side, so the readme spells the limit out.
    /// </summary>
    [Fact]
    public void Config_comparison_values_are_limited_to_int()
    {
        var config = Newtonsoft.Json.Linq.JObject.Parse(@"{ ""tooBig"": 5000000000, ""fine"": 1000000000 }");

        Assert.Throws<OverflowException>(() => config.Property("tooBig")!.Value.Value<int>());
        Assert.Equal(1000000000, config.Property("fine")!.Value.Value<int>());
    }
}
