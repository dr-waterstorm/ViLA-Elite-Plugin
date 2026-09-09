# ViLA-Elite-Plugin
An Elite Dangerous Plugin for ViLA.

The ViLA-Elite-Plugin reads the Status.json of Elite Dangerous, process the data and sends various status information to ViLA. With the help of the sample config it can be easily tweaked to map the available Elite status information to any LED of an Virpil device. See the [ViLA Plugin readme](https://github.com/charliefoxtwo/ViLA) for more information.

## Prerequisites

You need to setup [ViLA](https://github.com/charliefoxtwo/ViLA) and run the game at least once before you can use this plugin. Elite will then automatically generate the folder structure and a Status.json file.

## Installation

To install the plugin download the [latest release](https://github.com/dr-waterstorm/ViLA-Elite-Plugin/releases) and unzip it to the `Plugins` folder of ViLA.

If you have any problems setting up the plugin or want to know more visit the [ViLA Wiki](https://github.com/charliefoxtwo/ViLA/wiki/Plugins).

### Example Config

To get started with an example configuration, you can copy the `EliteSettings.json`, located in the `ViLAEliteConfiguration` folder, into the ViLA `Configuration` folder. 

The example currently assumes the following button setup for the LEDs to light up the correct buttons for the Virpil Control Panel 2:

| Button | Function                 |
| ------ | ------------------------ |
| B1     | Toggle HUD Mode          |
| B2     | Toggle Cargo Scoop       |
| B3     | Toggle Hardpoints        |
| B4     | Toggle Landing Gear      |
| B5     | Deploy Heatsink          |
| B6     | Toggle Silent Running    |
| B7     | Toggle Lights            |
| B8     | Toggle Night Vision Mode |
| B9     | Open System Map          |
| B10    | Open Galaxy Map          |

And the following for the Virpil Control Panel 1:

| Button | Function                 |
| ------ | ------------------------ |
| B1     | Toggle HUD Mode          |
| B2     | Toggle Cargo Scoop       |
| B3     | Toggle Landing Gear      |
| B4     | Toggle Hardpoints        |
| B5     | Open System Map          |
| B6     | Open Galaxy Map          |
| B7     | Toggle DSS               |
| B8     | Toggle Lights            |
| B9     | Deploy Heatsink          |
| B10    | Exit FSS                 |
| B11    | Toggle Night Vision Mode |
| B12    | Toggle Silent Running    |

You can modify the file according to your needs and wishes. See the Configuration section below for more information.

## Configuration

After running ViLA with this plugin active once it will create a `config.json` inside of the plugin folder. Currently it's only possible to change the location of the Elite config folder with the `StatusLocation` value. By default it is set to the standard Elite folder and probably does not need to be changed.

### Variables

The following tables show all available variables that can be used in the configuration file. The
type column tells you what kind of value to expect - ViLA itself compares all numbers the same way,
so `int`, `long`, `float` and `double` all just mean "a number".

#### Ship, SRV and fighter states (`Flags`)

| Variable                  | Description                                                        | Type    |
| ------------------------- | ------------------------------------------------------------------ | ------- |
| GameStarted               | The game is running (see the notes below)                          | boolean |
| Docked                    | Docked on a landing pad                                            | boolean |
| Landed                    | Landed on a planet                                                 | boolean |
| LandingGearDown           | Landing gear down                                                  | boolean |
| ShieldsUp                 | Shields up                                                         | boolean |
| Supercruise               | In Supercruise                                                     | boolean |
| FlightAssistOff           | FlightAssist off                                                   | boolean |
| HardpointsDeployed        | Hardpoints deployed                                                | boolean |
| InWing                    | In wing                                                            | boolean |
| LightsOn                  | Lights on                                                          | boolean |
| CargoScoopDeployed        | Cargo scoop deployed                                               | boolean |
| SilentRunning             | Silent running                                                     | boolean |
| ScoopingFuel              | Scooping fuel                                                      | boolean |
| SRVHandbrake              | Handbrake on (SRV)                                                 | boolean |
| SRVTurret                 | Using Turrent view (SRV)                                           | boolean |
| SRVTurretRetracted        | Turret retracted (SRV, when close to ship)                         | boolean |
| SRVDriveAssist            | Drive Assist (SRV)                                                 | boolean |
| SRVHighBeam               | High beam on (SRV)                                                 | boolean |
| FSDMassLocked             | Mass Locked (FSD)                                                  | boolean |
| FSDCharging               | FSD Charging                                                       | boolean |
| FSDCooldown               | FSD in cooldown                                                    | boolean |
| FSDJump                   | FSD Jump (jumping to another system)                               | boolean |
| LowFuel                   | Low on fuel (< 25%)                                                | boolean |
| OverHeating               | Over Heating (> 100%)                                              | boolean |
| HasLatLong                | Position values are available (close to a body)                    | boolean |
| IsInDanger                | In danger                                                          | boolean |
| BeingInterdicted          | Being interdicted                                                  | boolean |
| InMainShip                | In main ship                                                       | boolean |
| InFighter                 | In fighter                                                         | boolean |
| InSRV                     | In SRV                                                             | boolean |
| HudInAnalysisMode         | In Analysis Mode (HUD)                                             | boolean |
| NightVision               | Night Vision on                                                    | boolean |
| AltitudeFromAverageRadius | Altitude above planet from average radius (used when farther away) | boolean |

#### On foot states (`Flags2`, Odyssey)

| Variable              | Description                                                | Type    |
| --------------------- | ---------------------------------------------------------- | ------- |
| OnFoot                | On foot                                                    | boolean |
| OnFootInStation       | On foot inside a station                                   | boolean |
| OnFootOnPlanet        | On foot on a planet surface                                | boolean |
| OnFootInHangar        | On foot in a hangar                                        | boolean |
| OnFootSocialSpace     | On foot in a social space (concourse)                      | boolean |
| OnFootExterior        | On foot outside                                            | boolean |
| BreathableAtmosphere  | The current atmosphere is breathable                       | boolean |
| AimDownSight          | Aiming down the sight of a weapon                          | boolean |
| LowOxygen             | Low oxygen warning                                         | boolean |
| LowHealth             | Low health warning                                         | boolean |
| Cold                  | Cold warning                                               | boolean |
| Hot                   | Hot warning                                                | boolean |
| VeryCold              | Very cold warning                                          | boolean |
| VeryHot               | Very hot warning                                           | boolean |
| GlideMode             | Gliding towards a planet surface                           | boolean |
| InTaxi                | In an Apex taxi / dropship / shuttle                       | boolean |
| InMultiCrew           | In another commander's ship (multicrew)                    | boolean |
| TelepresenceMultiCrew | Multicrew via telepresence                                 | boolean |
| PhysicalMultiCrew     | Multicrew while physically in the same ship                | boolean |
| FSDHyperdriveCharging | FSD charging for a jump to another system                  | boolean |
| SupercruiseOverdrive  | Supercruise Overcharge (SCO) active                        | boolean |
| SupercruiseAssist     | Supercruise Assist active                                  | boolean |
| NPCCrewActive         | At least one NPC crew member is on active duty             | boolean |

#### Values

| Variable          | Description                                                         | Type   |
| ----------------- | ------------------------------------------------------------------- | ------ |
| SysPips           | Sys Pips 0-8 (in half steps)                                        | int    |
| EngPips           | Eng Pips 0-8 (in half steps)                                        | int    |
| WepPips           | Wep Pips 0-8 (in half steps)                                        | int    |
| FireGroup         | Currently selected firegroup, starting at 0 for A                   | int    |
| GuiFocus          | Shows the selected GUI screen, see list below for details           | int    |
| FuelMain          | Fuel in main tank - mass in tons                                    | float  |
| FuelReservoir     | Fuel in reservoir tank - mass in tons                               | float  |
| Cargo             | Cargo mass in tonns                                                 | float  |
| LegalState        | Current legal state, see list below for details                     | string |
| Balance           | Current credits balance                                             | long   |
| DestinationSystem | Destination: System address                                         | string |
| DestinationBody   | Destination: Body number                                            | string |
| DestinationName   | Destination: Name, can be a symbol like `$MULTIPLAYER_SCENARIO42_TITLE;` | string |
| DestinationNameLocalised | Destination: readable name, when the game provides one       | string |
| Latitude          | Latitude in degrees, only set when `HasLatLong` is true             | float  |
| Longitude         | Longitude in degrees, only set when `HasLatLong` is true            | float  |
| Heading           | Heading in degrees (0-359), only set when `HasLatLong` is true      | int    |
| Altitude          | Altitude above the surface in meters                                | double |
| BodyName          | Name of the body you are on or next to                              | string |
| PlanetRadius      | Radius of that body in meters                                       | double |

#### On foot values (Odyssey)

| Variable       | Description                                                              | Type   |
| -------------- | ------------------------------------------------------------------------ | ------ |
| Oxygen         | Remaining oxygen, `0.0` - `1.0`                                          | float  |
| Health         | Remaining health, `0.0` - `1.0`                                          | float  |
| Temperature    | Ambient temperature in Kelvin                                            | float  |
| Gravity        | Gravity of the current body, relative to 1 g                             | float  |
| SelectedWeapon | Name of the equipped weapon, e.g. `$humanoid_fists_name;`                | string |
| SelectedWeaponLocalised | Readable name of that weapon, e.g. `Unarmed`                    | string |

#### Notes on the variables

- **`GameStarted` is the only variable that is sent while the game is closed.** Everything else is
  0 / empty in that situation and says nothing, so it is not sent at all.
- **Nothing turns an LED off by itself.** ViLA only writes to the device when a condition matches;
  a condition that *stops* matching produces no command at all, so the LED keeps the last color it
  was given - also when you quit the game. If you want the LEDs to go dark with the game closed,
  add an action with a `GameStarted` `EqualTo` `false` condition and the color `000000`. The
  example configuration does not do this yet.
- **On foot the ship states are all cleared**, and `Flags` can be 0 altogether (that is what the
  game writes while you stand in a station), so `GameStarted` looks at `Flags2` as well.
- **Only changed values are sent to ViLA.** ViLA writes the LED command to the device every time it
  receives a value, so sending everything on every write of `Status.json` (about once a second)
  would keep the USB connection busy for nothing. Every value is still refreshed at least every 30
  seconds, because ViLA keeps a single state for all of its plugins and another plugin clearing it
  would otherwise leave the Elite LEDs stuck.
- **`Balance` can be bigger than a comparison value may be.** The value itself is read as a 64 bit
  number, but ViLA parses whole numbers in the configuration as 32 bit ints, so a comparison value
  has to stay below 2,147,483,647. Compare against e.g. `1000000000` instead.
- **`Heading` is normalized to 0-359.** On foot the game reports -180 to 180, in a ship 0 to 359.
- The game only writes some values in the situation they belong to: the on foot values while you
  are on foot, and the position values while `HasLatLong` is true. The plugin reports `0` for the
  others, so `Oxygen` reads `0` while you sit in your ship - use `OnFoot` / `HasLatLong` in an
  `and` condition if that matters for your LED.


#### GUI focus details

| Value | Description                          |
| ----- | ------------------------------------ |
| 0     | No GUI screen focused                |
| 1     | Internals Panel (right side)         |
| 2     | Externals Panel (left side)          |
| 3     | Comms Panel (top)                    |
| 4     | Roles Panel (bottom)                 |
| 5     | Station services                     |
| 6     | Galaxy Map                           |
| 7     | System Map                           |
| 8     | Orrery                               |
| 9     | Full Specturm System Scanner (FSS)   |
| 10    | Detailed Surface Scanner (SAA / DSS) |
| 11    | Codex                                |

#### Legal states


| Value             | Description                                         |
| ----------------- | --------------------------------------------------- |
| `Clean`           | Clean, everything is fine for now                   |
| `IllegalCargo`    | Carrying illegal cargo                              |
| `Speeding`        | Speeding (when approaching a station / landing pad) |
| `Wanted`          | Wanted in the current system                        |
| `Hostile`         | Hostile, in enemy Powerplay system                  |
| `PassengerWanted` | Passerger is wanted                                 |
| `Warrant`         | Warrant, scanned by AWS with outstanding bounties   |
| `Allied`          | Allied with the current minor faction               |
| `Thargoid`        | Thargoid                                            |

The game is not limited to this list, other values have been seen in the wild (`None`, `Lawless`,
`Enemy`, `WantedEnemy`, `Hunter`), so compare against what your own status file actually contains.



More information on most variables can be found on the [Elite Journal Status File overview](https://elite-journal.readthedocs.io/en/latest/Status%20File.html) (or its [markdown source](https://github.com/Lombra/elite-api-docs/blob/master/docs/Status%20File.md)) and the [Elite Journal Documentation PDF Manual](https://hosting.zaonce.net/community/journal/v32/Journal_Manual-v32.pdf). Frontier does not document everything: `Flags2` bit 22 (`NPCCrewActive`), `DestinationNameLocalised` and `SelectedWeaponLocalised` come from what [EDDI](https://github.com/EDCD/EDDI) and [EDDiscovery](https://github.com/EDDiscovery/EliteDangerousCore) read out of real status files.

## Roadmap:

### Currently planned

#### Initial Release

- [x] Fix high power draw of FileWatcher
- [x] Update readme with all available config values and how to install
- [x] Use of non-flag values in Status.json such as firegroups, mass, heat, pips, etc
- [x] Main / Dev branch with automated releases
- [x] Add Example Configuration for Virpil Control Panel #2
- [x] Add Example Configuration for Virpil Control Panel #1

#### Long Term

- [ ] Add various options for example "Heat warnings" (light all LEDs red), etc
- [x] Add all other missing values from Status.json (heading, altitude, etc)
- [x] Add on foot values
- [ ] Add an example configuration that uses the on foot values
- [ ] Read the journal files as well, for everything that is not in Status.json (ship type, current
      system, cargo contents, ...)

## Development

The plugin is a .NET 6 class library, so a `dotnet build` is all it takes:

```
dotnet build
dotnet test
```

`dotnet test` runs the unit tests in `ViLAElitePluginTests`. They parse a set of `Status.json`
samples and check what the plugin would send to ViLA, including the values that do not fit into a
32 bit int (`Balance`, `Altitude`) and the `Flags` bit 31.

### Releasing

Bump `"version"` in `ViLAElitePlugin/manifest.json` and push to `main`. The GitHub action builds,
tests and packs the plugin and creates a release tagged with exactly that version - ViLA only
recognizes a plugin update when the release tag parses as a version number, and it compares it
against the `version` from the installed `manifest.json`. If a release for that version already
exists, the action publishes nothing and says so in the run summary.

### Dependencies

The plugin targets **net6.0 on purpose**: ViLA is a net6.0 executable and loads the plugin into its
own process, so a newer target framework cannot be loaded. `Newtonsoft.Json` stays on the version
`plugin_manifest.xml` declares as provided by ViLA - that is what keeps it out of the release zip.
Both should only move when ViLA itself moves. `ViLA.PluginBase` is on the latest published version.

### How ViLA handles what the plugin sends

Useful to know before changing the sending side, all of it from
[ViLA's source](https://github.com/charliefoxtwo/ViLA):

- ViLA keeps **one** state dictionary for all of its plugins, keyed by variable name, and it is not
  synchronized - so this plugin parses one status file at a time and never calls `ClearState`,
  which would drop the other plugins' values too.
- Values are read back with a cast to whatever type the configured condition needs. Sending a type
  that cannot be converted only fails inside ViLA, when somebody uses that variable - hence the
  type tests in `ViLAElitePluginTests/ViLaCompatibilityTests.cs`.
- ViLA writes to the device for every value it receives, whether it changed or not, and it writes
  nothing for a condition that stops matching. That is why the plugin filters out unchanged values
  and why an LED keeps its color until some condition matches again.
- A whole number in a configuration is read as a 32 bit int, and a value that does not fit makes
  ViLA discard the whole configuration file with only a warning.
