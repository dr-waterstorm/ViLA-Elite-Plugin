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

The following tables shows all available variables that can be used in the configuration file.
| Variable                  | Description                                                        | Type    |
| ------------------------- | ------------------------------------------------------------------ | ------- |
| GameStarted               | The game has started (flags in the Status.json are not 0)          | boolean |
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
| FSDMassLocked             | Mass Locked (FSD)                                                  | boolean |
| FSDCharging               | FSD Charging                                                       | boolean |
| FSDCooldown               | FSD in cooldown                                                    | boolean |
| LowFuel                   | Low on fuel (< 25%)                                                | boolean |
| OverHeating               | Over Heating (> 100%)                                              | boolean |
| HasLatLong                | ?                                                                  | boolean |
| IsInDanger                | ?                                                                  | boolean |
| BeingInterdicted          | Being interdicted                                                  | boolean |
| InMainShip                | In main ship                                                       | boolean |
| InFighter                 | In fighter                                                         | boolean |
| InSRV                     | In SRV                                                             | boolean |
| HudInAnalysisMode         | In Analysis Mode (HUD)                                             | boolean |
| NightVision               | Night Vision on                                                    | boolean |
| AltitudeFromAverageRadius | Altitude above planet from average radius (used when farther away) | boolean |
| FSDJump                   | FSD Jump (jumping to another system)                               | boolean |
| SRVHighBeam               | ?                                                                  | boolean |
| SysPips                   | Sys Pips 0-8 (in half steps)                                       | int     |
| EngPips                   | Eng Pips 0-8 (in half steps)                                       | int     |
| WepPips                   | Wep Pips 0-8 (in half steps)                                       | int     |
| FireGroup                 | Currently selected firegroup number                                | int     |
| GuiFocus                  | Shows the selected GUI screen, see list below for details          | int     |
| FuelMain                  | Fuel in main tank - mass in tons                                   | float   |
| FuelReservoir             | Fuel in reservoir tank - mass in tons                              | float   |
| Cargo                     | Cargo mass in tonns                                                | int     |
| LegalState                | Current legal state, see list below for details                    | int     |
| Balance                   | Current credits balance                                            | int     |
| DestinationSystem         | Destination: System                                                | string  |
| DestinationBody           | Destination: Body                                                  | string  |
| DestinationName           | Destination: Name                                                  | string  |


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



More information on most variables can be found on the [Elite Journal Status.json overview](https://elite-journal.readthedocs.io/en/latest/Status%20File/#status-file) and [Elite Journal Documentation PDF Manual](https://hosting.zaonce.net/community/journal/v32/Journal_Manual-v32.pdf).

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
- [ ] Add all other missing values from Status.json (heading, altitude, etc) - do we actually need them?
- [ ] Add on foot values
