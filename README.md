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
| Variable                  | Description                                               | Type    |
| ------------------------- | --------------------------------------------------------- | ------- |
| GameStarted               | The game has started (flags in the Status.json are not 0) | boolean |
| Docked                    | Docked on a landing pad                                   | boolean |
| Landed                    | Landed on a planet                                        | boolean |
| LandingGearDown           | Landing gear down                                         | boolean |
| ShieldsUp                 | Shields up                                                | boolean |
| Supercruise               | In  Supercruise                                           | boolean |
| FlightAssistOff           | FlightAssist off                                          | boolean |
| HardpointsDeployed        | Hardpoints deployed                                       | boolean |
| InWing                    | In wing                                                   | boolean |
| LightsOn                  | Lights on                                                 | boolean |
| CargoScoopDeployed        | Cargo scoop deployed                                      | boolean |
| SilentRunning             | Silent running                                            | boolean |
| ScoopingFuel              | Scooping fuel                                             | boolean |
| SRVHandbrake              | Handbrake on (SRV)                                        | boolean |
| SRVTurret                 | Using Turrent view (SRV)                                  | boolean |
| SRVTurretRetracted        | Turret retracted (SRV, when close to ship)                | boolean |
| SRVDriveAssist            | Drive Assist (SRV)                                        | boolean |
| FSDMassLocked             | Mass Locked (FSD)                                         | boolean |
| FSDCharging               | FSD Charging                                              | boolean |
| FSDCooldown               | FSD in cooldown                                           | boolean |
| LowFuel                   | Low on fuel (< 25%)                                       | boolean |
| OverHeating               | Over Heating (> 100%)                                     | boolean |
| HasLatLong                | ?                                                         | boolean |
| IsInDanger                | ?                                                         | boolean |
| BeingInterdicted          | Being interdicted                                         | boolean |
| InMainShip                | In main ship                                              | boolean |
| InFighter                 | In fighter                                                | boolean |
| InSRV                     | In SRV                                                    | boolean |
| HudInAnalysisMode         | In Analysis Mode (HUD)                                    | boolean |
| NightVision               | Night Vision on                                           | boolean |
| AltitudeFromAverageRadius | ?                                                         | boolean |
| FSDJump                   | ?                                                         | boolean |
| SRVHighBeam               | ?                                                         | boolean |
| SysPips                   | Sys Pips 0-8 (in half steps)                              | int     |
| EngPips                   | Eng Pips 0-8 (in half steps)                              | int     |
| WepPips                   | Wep Pips 0-8 (in half steps)                              | int     |

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
