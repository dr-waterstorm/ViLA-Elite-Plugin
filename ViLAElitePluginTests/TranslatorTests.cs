using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ViLAElitePluginTests;

public class TranslatorTests
{
    private sealed class Harness
    {
        public List<KeyValuePair<string, object>> Received { get; } = new();

        public Exception? ThrowOnce { get; set; }

        public Translator Translator { get; }

        public Harness(TimeSpan? resendInterval = null)
        {
            Translator = new Translator(NullLogger<Translator>.Instance, Receive, resendInterval);
        }

        private void Receive(string id, dynamic value)
        {
            if (ThrowOnce != null)
            {
                var error = ThrowOnce;
                ThrowOnce = null;
                throw error;
            }

            Received.Add(new KeyValuePair<string, object>(id, (object) value));
        }
    }

    [Fact]
    public void Unchanged_values_are_not_sent_again()
    {
        var harness = new Harness();

        harness.Translator.FromStatusFile("ShieldsUp", true);
        harness.Translator.FromStatusFile("ShieldsUp", true);
        harness.Translator.FromStatusFile("ShieldsUp", true);

        // ViLA re-sends the LED command for every value it receives, so repeating ourselves here
        // means pointless USB traffic
        Assert.Single(harness.Received);
    }

    [Fact]
    public void Changed_values_are_sent()
    {
        var harness = new Harness();

        harness.Translator.FromStatusFile("FuelMain", 25.6f);
        harness.Translator.FromStatusFile("FuelMain", 25.6f);
        harness.Translator.FromStatusFile("FuelMain", 25.5f);

        Assert.Equal(2, harness.Received.Count);
        Assert.Equal(25.5f, harness.Received.Last().Value);
    }

    [Fact]
    public void Values_of_every_exposed_type_are_forwarded()
    {
        var harness = new Harness();

        harness.Translator.FromStatusFile("Balance", 4648069815L);
        harness.Translator.FromStatusFile("FuelMain", 25.6f);
        harness.Translator.FromStatusFile("SysPips", 4);
        harness.Translator.FromStatusFile("Docked", true);
        harness.Translator.FromStatusFile("LegalState", "Clean");
        harness.Translator.FromStatusFile("PlanetRadius", 1206724.5d);

        Assert.Equal(6, harness.Received.Count);
        Assert.Equal(4648069815L, harness.Received[0].Value);
        Assert.Equal("Clean", harness.Received[4].Value);
    }

    [Fact]
    public void Null_is_ignored()
    {
        var harness = new Harness();

        harness.Translator.FromStatusFile<string?>("LegalState", null);

        Assert.Empty(harness.Received);
    }

    [Fact]
    public void Unchanged_values_are_sent_again_after_the_resend_interval()
    {
        // any other ViLA plugin can clear the state dictionary this plugin writes into, so a value
        // is refreshed from time to time even when the game did not change it
        var harness = new Harness(TimeSpan.Zero);

        harness.Translator.FromStatusFile("ShieldsUp", true);
        harness.Translator.FromStatusFile("ShieldsUp", true);

        Assert.Equal(2, harness.Received.Count);
    }

    [Fact]
    public void ForgetSentValues_makes_the_next_update_send_everything()
    {
        var harness = new Harness();

        harness.Translator.FromStatusFile("ShieldsUp", true);
        harness.Translator.ForgetSentValues();
        harness.Translator.FromStatusFile("ShieldsUp", true);

        Assert.Equal(2, harness.Received.Count);
    }

    [Fact]
    public void A_value_ViLA_could_not_handle_is_not_remembered_as_sent()
    {
        // ViLA evaluates conditions and writes to the device inside the callback and handles no
        // errors itself - a broken LED config or a device that just went away throws in here
        var harness = new Harness { ThrowOnce = new InvalidOperationException("device is gone") };

        harness.Translator.FromStatusFile("ShieldsUp", true);
        Assert.Empty(harness.Received);

        // without the retry the LED would stay wrong until the value changes on its own
        harness.Translator.FromStatusFile("ShieldsUp", true);
        Assert.Single(harness.Received);
        Assert.Equal(true, harness.Received.Single().Value);
    }

    [Fact]
    public void One_failing_value_does_not_stop_the_others()
    {
        var harness = new Harness { ThrowOnce = new InvalidOperationException("bad color") };

        harness.Translator.FromStatusFile("ShieldsUp", true);
        harness.Translator.FromStatusFile("Docked", true);
        harness.Translator.FromStatusFile("LightsOn", true);

        Assert.Equal(2, harness.Received.Count);
    }
}
