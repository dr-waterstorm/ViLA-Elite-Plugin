# ViLA-Elite-Plugin
An Elite Dangerous Plugin for ViLA.

The ViLA-Elite-Plugin reads the Status.json of Elite Dangerous, process the data and sends various status information to ViLA. With the help of the sample config it can be easily tweaked to map the available Elite status information to any LED of an Virpil device. See the [ViLA Plugin readme](https://github.com/charliefoxtwo/ViLA) for more information.

## Prerequisites

You need to setup [ViLA](https://github.com/charliefoxtwo/ViLA) and run the game at least once before you can use this plugin. Elite will then automatically generate the folder structure and a Status.json file.

## Installation

To install the plugin download the latest release and unzip it to the `Plugins` folder of ViLA. To get started with an example you can copy the `EliteSettings.json` located in the `ViLAEliteConfiguration` folder into the ViLA `Configuration` folder. Modify the file according to your needs and wishes. See the Configuration section below for more information.

If you have any problems setting up the plugin or want to know more visit the [ViLA Wiki](https://github.com/charliefoxtwo/ViLA/wiki/Plugins).

## Configuration

After running ViLA with this plugin active once it will create a `config.json` inside of the plugin folder. Currently it's only possible to change the location of the Elite config folder with the `StatusLocation` value. By default it is set to the standard Elite folder and probably does not need to be changed.

The following tables shows all available variables that can be used in the configuration file.
| Variable                  | Description                                               |
| ------------------------- | --------------------------------------------------------- |
| GameStarted               | The game has started (flags in the Status.json are not 0) |
| Docked                    | Docked on a landing pad                                   |
| Landed                    | Landed on a planet                                        |
| LandingGearDown           | Landing gear down                                         |
| ShieldsUp                 | Shields up                                                |
| Supercruise               | In  Supercruise                                           |
| FlightAssistOff           | FlightAssist off                                          |
| HardpointsDeployed        | Hardpoints deployed                                       |
| InWing                    | In wing                                                   |
| LightsOn                  | Lights on                                                 |
| CargoScoopDeployed        | Cargo scoop deployed                                      |
| SilentRunning             | Silent running                                            |
| ScoopingFuel              | Scooping fuel                                             |
| SRVHandbrake              | Handbrake on (SRV)                                        |
| SRVTurret                 | Using Turrent view (SRV)                                  |
| SRVTurretRetracted        | Turret retracted (SRV, when close to ship)                |
| SRVDriveAssist            | Drive Assist (SRV)                                        |
| FSDMassLocked             | Mass Locked (FSD)                                         |
| FSDCharging               | FSD Charging                                              |
| FSDCooldown               | FSD in cooldown                                           |
| LowFuel                   | Low on fuel (< 25%)                                       |
| OverHeating               | Over Heating (> 100%)                                     |
| HasLatLong                | ?                                                         |
| IsInDanger                | ?                                                         |
| BeingInterdicted          | Being interdicted                                         |
| InMainShip                | In main ship                                              |
| InFighter                 | In fighter                                                |
| InSRV                     | In SRV                                                    |
| HudInAnalysisMode         | In Analysis Mode (HUD)                                    |
| NightVision               | Night Vision on                                           |
| AltitudeFromAverageRadius | ?                                                         |
| FSDJump                   | ?                                                         |
| SRVHighBeam               | ?                                                         |

More information on most variables can be found on the [Elite Journal documenation](https://elite-journal.readthedocs.io/en/latest/Status%20File/#status-file)

## Roadmap:

### Currently planned

- [x] Fix high power draw of FileWatcher
- [x] Update readme with all available config values and how to install
- [ ] Better Example Configuration
- [ ] Main / Dev branch with automated releases
- [ ] Use of non-flag values in Status.json such as firegroups, mass, heat, pips, etc
- [ ] Add various options for example "Heat warnings" (light all LEDs red), etc

### Long Term

- [ ] Add on foot values?
